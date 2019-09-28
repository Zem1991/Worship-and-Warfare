using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPieceHandler : MonoBehaviour
{
    [Header("Attacker")]
    public CombatHeroPiece attackerHero;
    public List<CombatUnitPiece> attackerUnits;

    [Header("Defender")]
    public CombatHeroPiece defenderHero;
    public List<CombatUnitPiece> defenderUnits;

    void Update()
    {
        if (CombatManager.Instance.combatStarted)
        {
            CheckBattleEnd();
        }
    }

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
        attackerUnits = new List<CombatUnitPiece>();

        if (defenderUnits != null)
        {
            foreach (var item in defenderUnits)
            {
                Destroy(item.gameObject);
            }
        }
        defenderUnits = new List<CombatUnitPiece>();
    }

    public void Create(FieldPiece attackerPiece, FieldPiece defenderPiece)
    {
        Remove();
        CombatHeroPiece prefabHero = AllPrefabs.Instance.combatHeroPiece;
        CombatUnitPiece prefabUnit = AllPrefabs.Instance.combatUnitPiece;

        if (attackerPiece.hero != null)
        {
            attackerHero = Instantiate(prefabHero, transform);
            attackerHero.Initialize(attackerPiece.hero);
            attackerHero.owner = attackerPiece.owner;
        }

        if (defenderPiece.hero != null)
        {
            defenderHero = Instantiate(prefabHero, transform);
            defenderHero.Initialize(defenderPiece.hero);
            defenderHero.owner = defenderPiece.owner;
        }

        if (attackerPiece.units != null)
        {
            foreach (var item in attackerPiece.units)
            {
                CombatUnitPiece uc = Instantiate(prefabUnit, transform);
                uc.Initialize(item);
                uc.owner = attackerPiece.owner;
                attackerUnits.Add(uc);
            }
        }

        if (defenderPiece.units != null)
        {
            foreach (var item in defenderPiece.units)
            {
                CombatUnitPiece uc = Instantiate(prefabUnit, transform);
                uc.Initialize(item, true);
                uc.owner = defenderPiece.owner;
                defenderUnits.Add(uc);
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

    public List<CombatUnitPiece> GetActiveUnits(List<CombatUnitPiece> units)
    {
        List<CombatUnitPiece> result = new List<CombatUnitPiece>();
        foreach (var item in units)
        {
            if (!item.isDead) result.Add(item);
        }
        return result;
    }

    public List<CombatUnitPiece> GetIdleUnits(List<CombatUnitPiece> units)
    {
        List<CombatUnitPiece> result = new List<CombatUnitPiece>();
        foreach (var item in units)
        {
            if (item.IsIdle()) result.Add(item);
        }
        return result;
    }

    public void Pathfind(AbstractCombatPiece piece, CombatTile targetTile,
        bool needGroundAccess = true, bool needWaterAccess = false, bool needLavaAccess = false)
    {
        Pathfinder.FindPath(piece.currentTile, targetTile, Pathfinder.HexHeuristic,
            needGroundAccess, needWaterAccess, needLavaAccess,
            out List<PathNode> result, out float pathCost);
        piece.SetPath(result, Mathf.CeilToInt(pathCost), targetTile);
    }

    private void CheckBattleEnd()
    {
        int attackerActive = GetActiveUnits(attackerUnits).Count;
        int defenderActive = GetActiveUnits(defenderUnits).Count;

        if (attackerActive > 0 && defenderActive <= 0)
        {
            int attackerIdle = GetIdleUnits(attackerUnits).Count;
            if (attackerIdle >= attackerActive) CombatManager.Instance.CombatEnd(CombatResult.ATTACKER_WON);
        }
        else if (defenderActive > 0 && attackerActive <= 0)
        {
            int defenderIdle = GetIdleUnits(defenderUnits).Count;
            if (defenderIdle >= defenderActive) CombatManager.Instance.CombatEnd(CombatResult.DEFENDER_WON);
        }
    }
}
