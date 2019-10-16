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

        int spawnId = 0;
        if (attackerPiece.hero != null)
        {
            attackerHero = Instantiate(prefabHero, transform);
            attackerHero.Initialize(attackerPiece.hero, attackerPiece.owner, spawnId);
        }

        spawnId = 1;
        if (defenderPiece.hero != null)
        {
            defenderHero = Instantiate(prefabHero, transform);
            defenderHero.Initialize(defenderPiece.hero, defenderPiece.owner, spawnId);
        }

        spawnId = 2;
        if (attackerPiece.units != null)
        {
            foreach (var item in attackerPiece.units)
            {
                CombatUnitPiece uc = Instantiate(prefabUnit, transform);
                uc.Initialize(attackerPiece.owner, item, spawnId);
                attackerUnits.Add(uc);

                spawnId += 2;
            }
        }

        spawnId = 3;
        if (defenderPiece.units != null)
        {
            foreach (var item in defenderPiece.units)
            {
                CombatUnitPiece uc = Instantiate(prefabUnit, transform);
                uc.Initialize(defenderPiece.owner, item, spawnId, true);
                defenderUnits.Add(uc);

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

    public void Pathfind(AbstractCombatPiece piece, CombatTile targetTile,
        bool needGroundAccess = true, bool needWaterAccess = false, bool needLavaAccess = false)
    {
        Pathfinder.FindPath(piece.currentTile, targetTile, Pathfinder.HexHeuristic,
            needGroundAccess, needWaterAccess, needLavaAccess,
            out PathfindResults pathfindResults);
        piece.SetPath(pathfindResults, targetTile);
    }

    public Hero GetHero(Player player)
    {
        CombatManager cm = CombatManager.Instance;
        if (player == cm.attacker) return attackerHero?.hero;
        if (player == cm.defender) return defenderHero?.hero;
        return null;
    }

    public bool GetPieceList(Player owner, bool enemyPieces, out List<CombatUnitPiece> list)
    {
        list = null;
        CombatManager cm = CombatManager.Instance;
        if (owner == cm.attacker)
        {
            if (enemyPieces) list = defenderUnits;
            else list = attackerUnits;
        }
        else if (owner == cm.defender)
        {
            if (enemyPieces) list = attackerUnits;
            else list = defenderUnits;
        }
        return list != null;
    }

    public List<CombatUnitPiece> GetIdlePieces(List<CombatUnitPiece> pieces)
    {
        List<CombatUnitPiece> result = new List<CombatUnitPiece>();
        foreach (var item in pieces)
        {
            if (item.IsIdle()) result.Add(item);
        }
        return result;
    }

    public List<CombatUnitPiece> GetActivePieces(List<CombatUnitPiece> pieces)
    {
        List<CombatUnitPiece> result = new List<CombatUnitPiece>();
        foreach (var item in pieces)
        {
            if (!item.isDead) result.Add(item);
        }
        return result;
    }
}
