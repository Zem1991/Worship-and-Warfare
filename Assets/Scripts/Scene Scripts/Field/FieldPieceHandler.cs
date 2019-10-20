using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldPieceHandler : MonoBehaviour
{
    public readonly int MAX_UNITS = 5;

    [Header("Pieces")]
    public List<PartyPiece2> partyPieces;

    public void Remove()
    {
        if (partyPieces != null)
        {
            foreach (var item in partyPieces)
            {
                Destroy(item.gameObject);
            }
        }
        partyPieces = new List<PartyPiece2>();
    }

    public void Create(PieceData[] pieceData)
    {
        Remove();

        DatabaseManager db = DatabaseManager.Instance;
        DBHandler_Hero dbHeroes = db.heroes;
        DBHandler_Unit dbUnits = db.units;

        PlayerManager pm = PlayerManager.Instance;

        FieldManager fm = FieldManager.Instance;
        FieldMap fieldMap = fm.mapHandler.map;
        PartyPiece2 prefabPiece = AllPrefabs.Instance.fieldPartyPiece;

        Hero prefabHero = AllPrefabs.Instance.hero;
        Unit prefabUnit = AllPrefabs.Instance.unit;

        partyPieces = new List<PartyPiece2>();

        foreach (var item in pieceData)
        {
            int posX = item.mapPosition[0];
            int posY = item.mapPosition[1];

            Vector3 pos = new Vector3(posX, 0, posY);
            Quaternion rot = Quaternion.identity;

            PartyPiece2 newPiece = Instantiate(prefabPiece, pos, rot, transform);
            partyPieces.Add(newPiece);

            Player owner = pm.allPlayers[item.ownerId];

            Hero hero = null;
            if (item.hero != null)
            {
                HeroData heroData = item.hero;

                int dbId = heroData.heroId;
                DB_Hero dbData = dbHeroes.Select(dbId);

                hero = Instantiate(prefabHero, newPiece.transform);
                hero.Initialize(heroData, dbData);
            }

            List<Unit> units = new List<Unit>();
            if (item.units != null)
            {
                if (item.units.Length > MAX_UNITS) Debug.LogWarning("There are more units than the piece can store!");
                int totalUnits = Mathf.Min(item.units.Length, MAX_UNITS);

                for (int i = 0; i < totalUnits; i++)
                {
                    UnitData unitData = item.units[i];

                    int dbId = unitData.unitId;
                    DB_Unit dbData = dbUnits.Select(dbId);

                    Unit unit = Instantiate(prefabUnit, newPiece.transform);
                    unit.Initialize(unitData, dbData);
                    units.Add(unit);
                }
            }
            
            Vector2Int id = new Vector2Int(posX, posY);
            newPiece.currentTile = fieldMap.tiles[id];
            newPiece.currentTile.occupantPiece = newPiece;
            newPiece.Initialize(owner, hero, units);
        }
    }

    public void RemovePiece(PartyPiece2 piece)
    {
        partyPieces.Remove(piece);
        piece.currentTile.occupantPiece = null;
        Destroy(piece.gameObject);
    }

    public void Pathfind(PartyPiece2 piece, FieldTile targetTile,
        bool needGroundAccess = true, bool needWaterAccess = false, bool needLavaAccess = false)
    {
        Pathfinder.FindPath(piece.currentTile, targetTile, Pathfinder.OctoHeuristic,
            needGroundAccess, needWaterAccess, needLavaAccess,
            out PathfindResults pathfindResults);
        piece.IMP_GetPieceMovement().SetPath(pathfindResults, targetTile);
    }

    public void PartiesAreInteracting(PartyPiece2 sender, PartyPiece2 receiver)
    {
        if (sender.IPO_GetOwner() == receiver.IPO_GetOwner())
        {
            GameManager.Instance.PerformExchange(sender, receiver);
        }
        else
        {
            GameManager.Instance.GoToCombat(sender, receiver);
        }
    }

    public List<PartyPiece2> GetIdlePieces(List<PartyPiece2> pieces)
    {
        List<PartyPiece2> result = new List<PartyPiece2>();
        foreach (var item in pieces)
        {
            if (item.ICP_IsIdle()) result.Add(item);
        }
        return result;
    }

    public List<PartyPiece2> GetPlayerPieces(Player player)
    {
        List<PartyPiece2> result = new List<PartyPiece2>();
        foreach (var item in partyPieces)
        {
            if (item.IPO_GetOwner() == player) result.Add(item);
        }
        return result;
    }

    public IEnumerator YieldForIdlePieces(List<PartyPiece2> pieces)
    {
        while (GetIdlePieces(pieces).Count != pieces.Count)
        {
            yield return null;
        }
    }
}
