using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPieceHandler : MonoBehaviour
{
    [Header("Attacker")]
    public CombatantHeroPiece2 attackerHero;
    public List<CombatantUnitPiece2> attackerUnits;
    public List<AbstractCombatPiece2> attackerPieces;

    [Header("Defender")]
    public CombatantHeroPiece2 defenderHero;
    public List<CombatantUnitPiece2> defenderUnits;
    public List<AbstractCombatPiece2> defenderPieces;

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
        attackerPieces = new List<AbstractCombatPiece2>();

        if (defenderUnits != null)
        {
            foreach (var item in defenderUnits)
            {
                Destroy(item.gameObject);
            }
        }
        defenderUnits = new List<CombatantUnitPiece2>();
        defenderPieces = new List<AbstractCombatPiece2>();
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
            attackerHero.Initialize(attackerPiece.partyHero, attackerPiece.GetOwner(), spawnId, false);
            attackerPieces.Add(attackerHero);
        }

        spawnId = 1;
        if (defenderPiece.partyHero != null)
        {
            defenderHero = Instantiate(prefabHero, transform);
            defenderHero.Initialize(defenderPiece.partyHero, defenderPiece.GetOwner(), spawnId, true);
            defenderPieces.Add(defenderHero);
        }

        spawnId = 2;
        if (attackerPiece.partyUnits != null)
        {
            foreach (var unit in attackerPiece.partyUnits)
            {
                CombatantUnitPiece2 uc = Instantiate(prefabUnit, transform);
                uc.Initialize(unit, attackerPiece.GetOwner(), spawnId, false);
                attackerUnits.Add(uc);
                attackerPieces.Add(uc);

                spawnId += 2;
            }
        }

        spawnId = 3;
        if (defenderPiece.partyUnits != null)
        {
            foreach (var unit in defenderPiece.partyUnits)
            {
                CombatantUnitPiece2 uc = Instantiate(prefabUnit, transform);
                uc.Initialize(unit, defenderPiece.GetOwner(), spawnId, true);
                defenderUnits.Add(uc);
                defenderPieces.Add(uc);

                spawnId += 2;
            }
        }
    }

    public void InitialPositions(CombatMap map)
    {
        InitialPosition(attackerPieces, map.attackerStartTiles);
        InitialPosition(defenderPieces, map.defenderStartTiles);
    }

    private void InitialPosition(List<AbstractCombatPiece2> combatants, List<CombatTile> tiles)
    {
        int middle = tiles.Count / 2;
        for (int i = 0; i < combatants.Count; i++)
        {
            int modifier = (i + 1) / 2;
            if (i % 2 == 1) modifier *= -1;

            AbstractCombatPiece2 combatPiece = combatants[i];
            CombatTile tile = tiles[middle + modifier];

            combatPiece.transform.position = tile.transform.position;
            combatPiece.currentTile = tile;
            tile.occupantPiece = combatPiece;
        }
    }

    public void Pathfind(AbstractCombatantPiece2 piece, CombatTile targetTile,
        bool needGroundAccess = true, bool needWaterAccess = false, bool needLavaAccess = false)
    {
        Pathfinder.FindPath(piece.currentTile, targetTile, Pathfinder.HexHeuristic,
            needGroundAccess, needWaterAccess, needLavaAccess,
            out PathfindResults pathfindResults);
        piece.pieceMovement.SetPath(pathfindResults, targetTile);
    }

    public CombatantHeroPiece2 GetHero(Player player)
    {
        CombatManager cm = CombatManager.Instance;
        if (player == cm.attackerPlayer) return attackerHero;
        if (player == cm.defenderPlayer) return defenderHero;
        return null;
    }

    public bool GetPieceList(Player owner, bool enemyPieces, out List<AbstractCombatPiece2> list)
    {
        list = null;
        CombatManager cm = CombatManager.Instance;
        if (owner == cm.attackerPlayer)
        {
            if (enemyPieces) list = defenderPieces;
            else list = attackerPieces;
        }
        else if (owner == cm.defenderPlayer)
        {
            if (enemyPieces) list = attackerPieces;
            else list = defenderPieces;
        }
        return list != null;
    }

    public List<AbstractCombatPiece2> GetIdlePieces(List<AbstractCombatPiece2> pieces)
    {
        List<AbstractCombatPiece2> result = new List<AbstractCombatPiece2>();
        foreach (var item in pieces)
        {
            if (item.ICP_IsIdle()) result.Add(item);
        }
        return result;
    }

    public List<AbstractCombatPiece2> GetActivePieces(List<AbstractCombatPiece2> pieces)
    {
        List<AbstractCombatPiece2> result = new List<AbstractCombatPiece2>();
        foreach (var item in pieces)
        {
            if (!item.stateDead) result.Add(item);
        }
        return result;
    }
}
