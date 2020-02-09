using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldPieceHandler : MonoBehaviour
{
    public readonly int MAX_UNITS = 5;

    [Header("Pieces")]
    public List<TownPiece2> townPieces;
    public List<PartyPiece2> partyPieces;
    public List<PickupPiece2> pickupPieces;

    public void RemoveAll()
    {
        if (partyPieces != null)
        {
            foreach (var item in partyPieces) Destroy(item.gameObject);
        }
        partyPieces = new List<PartyPiece2>();

        if (pickupPieces != null)
        {
            foreach (var item in pickupPieces) Destroy(item.gameObject);
        }
        pickupPieces = new List<PickupPiece2>();
    }

    public void CreateAll(List<TownData> towns, List<PartyData> parties, List<PickupData> pickups)
    {
        RemoveAll();

        CreateTowns(towns);
        CreateParties(parties);
        CreatePickups(pickups);
    }

    private void CreateTowns(List<TownData> towns)
    {
        if (towns == null) return;

        AbstractDBContentHandler<DB_Faction> dbFactions = DBHandler_Faction.Instance;
        AbstractDBContentHandler<DB_TownBuilding> dbTownBuildings = DBHandler_TownBuilding.Instance;

        PlayerManager pm = PlayerManager.Instance;
        FieldManager fm = FieldManager.Instance;
        FieldMap fieldMap = fm.mapHandler.map;

        TownPiece2 prefabPiece = AllPrefabs.Instance.fieldTownPiece;
        Town prefabTown = AllPrefabs.Instance.town;
        TownBuilding prefabTownBuilding = AllPrefabs.Instance.townBuilding;

        townPieces = new List<TownPiece2>();

        foreach (var townData in towns)
        {
            int posX = townData.mapPosition[0];
            int posY = townData.mapPosition[1];

            Vector2Int tileId = new Vector2Int(posX, posY);
            FieldTile fieldTile = fieldMap.tiles[tileId];
            Vector3 pos = fieldTile.transform.position;
            Quaternion rot = Quaternion.identity;

            Player owner = pm.allPlayers[townData.ownerId - 1];

            string factionId = townData.factionId;
            DB_Faction dbFactionData = dbFactions.Select(factionId);

            TownPiece2 newPiece = Instantiate(prefabPiece, pos, rot, transform);
            townPieces.Add(newPiece);

            Town town = Instantiate(prefabTown, newPiece.transform);
            town.Initialize(dbFactionData, townData.townName);

            foreach (var townBuildingData in townData.townBuildings)
            {
                string townBuildingId = townBuildingData.townBuildingId;
                DB_TownBuilding dbTownBuildingData = dbTownBuildings.Select(townBuildingId);

                TownBuilding townBldg = Instantiate(prefabTownBuilding, town.transform);
                townBldg.Initialize(dbTownBuildingData);

                town.buildings.Add(townBldg);
            }

            newPiece.currentTile = fieldTile;
            newPiece.currentTile.occupantPiece = newPiece;
            newPiece.Initialize(owner, town);
        }
    }

    private void CreateParties(List<PartyData> parties)
    {
        if (parties == null) return;

        AbstractDBContentHandler<DB_Hero> dbHeroes = DBHandler_Hero.Instance;
        AbstractDBContentHandler<DB_Unit> dbUnits = DBHandler_Unit.Instance;

        PlayerManager pm = PlayerManager.Instance;
        FieldManager fm = FieldManager.Instance;
        FieldMap fieldMap = fm.mapHandler.map;

        PartyPiece2 prefabPiece = AllPrefabs.Instance.fieldPartyPiece;
        Hero prefabHero = AllPrefabs.Instance.hero;
        Unit prefabUnit = AllPrefabs.Instance.unit;

        partyPieces = new List<PartyPiece2>();

        foreach (var partyData in parties)
        {
            int posX = partyData.mapPosition[0];
            int posY = partyData.mapPosition[1];

            Vector2Int tileId = new Vector2Int(posX, posY);
            FieldTile fieldTile = fieldMap.tiles[tileId];
            Vector3 pos = fieldTile.transform.position;
            Quaternion rot = Quaternion.identity;

            PartyPiece2 newPiece = Instantiate(prefabPiece, pos, rot, transform);
            partyPieces.Add(newPiece);

            Player owner = pm.allPlayers[partyData.ownerId - 1];

            Hero hero = null;
            if (partyData.hero != null)
            {
                HeroData heroData = partyData.hero;

                string heroId = heroData.id;
                DB_Hero dbData = dbHeroes.Select(heroId);

                hero = Instantiate(prefabHero, newPiece.transform);
                hero.Initialize(dbData, heroData.experienceData, heroData.inventoryData);
            }

            List<Unit> units = new List<Unit>();
            if (partyData.units != null)
            {
                if (partyData.units.Length > MAX_UNITS) Debug.LogWarning("There are more units than the piece can store!");
                int totalUnits = Mathf.Min(partyData.units.Length, MAX_UNITS);

                for (int i = 0; i < totalUnits; i++)
                {
                    UnitData unitData = partyData.units[i];

                    string unitId = unitData.id;
                    DB_Unit dbData = dbUnits.Select(unitId);

                    Unit unit = Instantiate(prefabUnit, newPiece.transform);
                    unit.Initialize(dbData, unitData.stackData);
                    units.Add(unit);
                }
            }

            newPiece.currentTile = fieldTile;
            newPiece.currentTile.occupantPiece = newPiece;
            newPiece.Initialize(owner, hero, units);
        }
    }

    private void CreatePickups(List<PickupData> pickups)
    {
        if (pickups == null) return;

        AbstractDBContentHandler<DB_Artifact> dbArtifacts = DBHandler_Artifact.Instance;
        AbstractDBContentHandler<DB_Unit> dbUnits = DBHandler_Unit.Instance;

        FieldManager fm = FieldManager.Instance;
        FieldMap fieldMap = fm.mapHandler.map;

        PickupPiece2 prefabPiece = AllPrefabs.Instance.fieldPickupPiece;

        pickupPieces = new List<PickupPiece2>();

        foreach (var pData in pickups)
        {
            int posX = pData.mapPosition[0];
            int posY = pData.mapPosition[1];

            Vector2Int tileId = new Vector2Int(posX, posY);
            FieldTile fieldTile = fieldMap.tiles[tileId];
            Vector3 pos = fieldTile.transform.position;
            Quaternion rot = Quaternion.identity;

            PickupPiece2 newPiece = Instantiate(prefabPiece, pos, rot, transform);
            pickupPieces.Add(newPiece);

            newPiece.currentTile = fieldTile;
            newPiece.currentTile.occupantPiece = newPiece;
            switch (pData.pickupType)
            {
                case PickupType.RESOURCE:
                    Debug.LogError("No support for resource pickups!");
                    break;
                case PickupType.ARTIFACT:
                    newPiece.Initialize(dbArtifacts.Select(pData.artifactId));
                    break;
                case PickupType.UNIT:
                    newPiece.Initialize(dbUnits.Select(pData.unitId), pData.unitAmount);
                    break;
            }
        }
    }

    public void RemovePiece(PartyPiece2 piece)
    {
        partyPieces.Remove(piece);
        piece.currentTile.occupantPiece = null;
        Destroy(piece.gameObject);
    }

    public void RemovePickup(PickupPiece2 pickup)
    {
        pickupPieces.Remove(pickup);
        pickup.currentTile.occupantPiece = null;
        Destroy(pickup.gameObject);
    }

    public bool Pathfind(PartyPiece2 piece, FieldTile targetTile,
        bool needGroundAccess = true, bool needWaterAccess = false, bool needLavaAccess = false)
    {
        bool result = Pathfinder.FindPath(piece.currentTile, targetTile,
            Pathfinder.OctoHeuristic, needGroundAccess, needWaterAccess, needLavaAccess,
            out PathfindResults pathfindResults);
        piece.pieceMovement.SetPath(pathfindResults, targetTile);
        return result;
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
            if (item.pieceOwner.GetOwner() == player) result.Add(item);
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
