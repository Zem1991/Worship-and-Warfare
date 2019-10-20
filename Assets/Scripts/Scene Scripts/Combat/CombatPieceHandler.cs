using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPieceHandler : MonoBehaviour
{
    [Header("Attacker")]
    public CombatantHeroPiece2 attackerHero;
    public List<CombatantUnitPiece2> attackerUnits;
    public List<AbstractCombatantPiece2> attackerPieces;

    [Header("Defender")]
    public CombatantHeroPiece2 defenderHero;
    public List<CombatantUnitPiece2> defenderUnits;
    public List<AbstractCombatantPiece2> defenderPieces;

    public void Remove()
    {
        if (attackerHero) Destroy(attackerHero.gameObject);
        if (defenderHero) Destroy(defenderHero.gameObject);

        if (attackerUnits != null)
        {
            foreach (var item in attackerUnits)
            {
                Destroy(item.gameObject);
            }
        }
        attackerUnits = new List<CombatantUnitPiece2>();
        attackerPieces = new List<AbstractCombatantPiece2>();

        if (defenderUnits != null)
        {
            foreach (var item in defenderUnits)
            {
                Destroy(item.gameObject);
            }
        }
        defenderUnits = new List<CombatantUnitPiece2>();
        defenderPieces = new List<AbstractCombatantPiece2>();
    }

    public void Create(PartyPiece2 attackerPiece, PartyPiece2 defenderPiece)
    {
        Remove();
        CombatantHeroPiece2 prefabHero = AllPrefabs.Instance.combatHeroPiece;
        CombatantUnitPiece2 prefabUnit = AllPrefabs.Instance.combatUnitPiece;

        int spawnId = 0;
        if (attackerPiece.partyHero != null)
        {
            attackerHero = Instantiate(prefabHero, transform);
            attackerHero.Initialize(attackerPiece.partyHero, attackerPiece.IPO_GetOwner(), spawnId);
            //attackerPieces.Add(attackerHero); //TODO THIS LATER
        }

        spawnId = 1;
        if (defenderPiece.partyHero != null)
        {
            defenderHero = Instantiate(prefabHero, transform);
            defenderHero.Initialize(defenderPiece.partyHero, defenderPiece.IPO_GetOwner(), spawnId);
            //defenderPieces.Add(defenderHero); //TODO THIS LATER
        }

        spawnId = 2;
        if (attackerPiece.partyUnits != null)
        {
            foreach (var item in attackerPiece.partyUnits)
            {
                CombatantUnitPiece2 uc = Instantiate(prefabUnit, transform);
                uc.Initialize(attackerPiece.IPO_GetOwner(), item, spawnId);
                attackerUnits.Add(uc);
                attackerPieces.Add(uc);

                spawnId += 2;
            }
        }

        spawnId = 3;
        if (defenderPiece.partyUnits != null)
        {
            foreach (var item in defenderPiece.partyUnits)
            {
                CombatantUnitPiece2 uc = Instantiate(prefabUnit, transform);
                uc.Initialize(defenderPiece.IPO_GetOwner(), item, spawnId, true);
                defenderUnits.Add(uc);
                defenderPieces.Add(uc);

                spawnId += 2;
            }
        }
    }

    //public void RemovePiece(FieldPiece piece)
    //{
    //    throw new NotImplementedException();
    //}

    public void InitialHeroPositions(CombatMap map)
    {
        CombatTile attackerHeroTile = map.attackerHeroTile;
        CombatTile defenderHeroTile = map.defenderHeroTile;

        if (attackerHero)
        {
            attackerHero.transform.position = attackerHeroTile.transform.position;
            attackerHero.currentTile = attackerHeroTile;
            attackerHeroTile.occupantPiece = attackerHero;
        }

        if (defenderHero)
        {
            defenderHero.transform.position = defenderHeroTile.transform.position;
            defenderHero.currentTile = defenderHeroTile;
            defenderHeroTile.occupantPiece = defenderHero;
        }
    }

    public void InitialUnitPositions(CombatMap map)
    {
        List<CombatTile> attackerHeroTiles = map.attackerStartTiles;
        List<CombatTile> defenderHeroTiles = map.defenderStartTiles;

        if (attackerUnits != null)
        {
            for (int i = 0; i < attackerUnits.Count; i++)
            {
                attackerUnits[i].transform.position = attackerHeroTiles[i].transform.position;
                attackerUnits[i].currentTile = attackerHeroTiles[i];
                attackerHeroTiles[i].occupantPiece = attackerUnits[i];
            }
        }

        if (defenderUnits != null)
        {
            for (int i = 0; i < attackerUnits.Count; i++)
            {
                defenderUnits[i].transform.position = defenderHeroTiles[i].transform.position;
                defenderUnits[i].currentTile = defenderHeroTiles[i];
                defenderHeroTiles[i].occupantPiece = defenderUnits[i];
            }
        }
    }

    public void Pathfind(AbstractCombatantPiece2 piece, CombatTile targetTile,
        bool needGroundAccess = true, bool needWaterAccess = false, bool needLavaAccess = false)
    {
        Pathfinder.FindPath(piece.currentTile, targetTile, Pathfinder.HexHeuristic,
            needGroundAccess, needWaterAccess, needLavaAccess,
            out PathfindResults pathfindResults);
        piece.IMP_GetPieceMovement().SetPath(pathfindResults, targetTile);
    }

    public CombatantHeroPiece2 GetHero(Player player)
    {
        CombatManager cm = CombatManager.Instance;
        if (player == cm.attacker) return attackerHero;
        if (player == cm.defender) return defenderHero;
        return null;
    }

    public bool GetPieceList(Player owner, bool enemyPieces, out List<AbstractCombatantPiece2> list)
    {
        list = null;
        CombatManager cm = CombatManager.Instance;
        if (owner == cm.attacker)
        {
            if (enemyPieces) list = defenderPieces;
            else list = attackerPieces;
        }
        else if (owner == cm.defender)
        {
            if (enemyPieces) list = attackerPieces;
            else list = defenderPieces;
        }
        return list != null;
    }

    public List<AbstractCombatantPiece2> GetIdlePieces(List<AbstractCombatantPiece2> pieces)
    {
        List<AbstractCombatantPiece2> result = new List<AbstractCombatantPiece2>();
        foreach (var item in pieces)
        {
            if (item.ICP_IsIdle()) result.Add(item);
        }
        return result;
    }

    public List<AbstractCombatantPiece2> GetActivePieces(List<AbstractCombatantPiece2> pieces)
    {
        List<AbstractCombatantPiece2> result = new List<AbstractCombatantPiece2>();
        foreach (var item in pieces)
        {
            if (!item.isDead) result.Add(item);
        }
        return result;
    }
}
