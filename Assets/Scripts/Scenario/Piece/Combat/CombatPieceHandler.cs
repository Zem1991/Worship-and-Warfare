using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPieceHandler : MonoBehaviour
{
    [Header("Attacker")]
    public HeroUnitPiece3 attackerHero;
    public List<CombatUnitPiece3> attackerUnits;
    public List<CombatantPiece3> attackerPieces;

    [Header("Defender")]
    public HeroUnitPiece3 defenderHero;
    public List<CombatUnitPiece3> defenderUnits;
    public List<CombatantPiece3> defenderPieces;

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
        attackerUnits = new List<CombatUnitPiece3>();
        attackerPieces = new List<CombatantPiece3>();

        if (defenderUnits != null)
        {
            foreach (var item in defenderUnits)
            {
                Destroy(item.gameObject);
            }
        }
        defenderUnits = new List<CombatUnitPiece3>();
        defenderPieces = new List<CombatantPiece3>();
    }

    public void Create(PartyPiece3 attackerParty, PartyPiece3 defenderParty)
    {
        Remove();
        HeroUnitPiece3 prefabHero = AllPrefabs.Instance.combatHeroPiece;
        CombatUnitPiece3 prefabUnit = AllPrefabs.Instance.combatUnitPiece;

        int spawnId = 0;
        if (attackerParty.party.GetHeroSlot() != null)
        {
            HeroUnit hero = attackerParty.party.GetHeroSlot().Get() as HeroUnit;
            if (hero)
            {
                attackerHero = Instantiate(prefabHero, transform);
                attackerHero.Initialize(attackerParty.pieceOwner.GetOwner(), spawnId, false, hero);
                attackerPieces.Add(attackerHero);
            }
        }

        spawnId = 1;
        if (defenderParty.party.GetHeroSlot() != null)
        {
            HeroUnit hero = defenderParty.party.GetHeroSlot().Get() as HeroUnit;
            if (hero)
            {
                defenderHero = Instantiate(prefabHero, transform);
                defenderHero.Initialize(defenderParty.pieceOwner.GetOwner(), spawnId, true, hero);
                defenderPieces.Add(defenderHero);
            }
        }

        spawnId = 2;
        foreach (var unitSlot in attackerParty.party.GetUnitSlots())
        {
            CombatUnit unit = unitSlot.Get() as CombatUnit;
            if (unit)
            {
                CombatUnitPiece3 uc = Instantiate(prefabUnit, transform);
                uc.Initialize(attackerParty.pieceOwner.GetOwner(), spawnId, false, unit);
                attackerUnits.Add(uc);
                attackerPieces.Add(uc);
            }
            spawnId += 2;
        }

        spawnId = 3;
        foreach (var unitSlot in defenderParty.party.GetUnitSlots())
        {
            CombatUnit unit = unitSlot.Get() as CombatUnit;
            if (unit)
            {
                CombatUnitPiece3 uc = Instantiate(prefabUnit, transform);
                uc.Initialize(defenderParty.pieceOwner.GetOwner(), spawnId, true, unit);
                defenderUnits.Add(uc);
                defenderPieces.Add(uc);
            }
            spawnId += 2;
        }
    }

    public void InitialPositions(CombatMap map)
    {
        InitialPosition(attackerPieces, map.attackerStartTiles);
        InitialPosition(defenderPieces, map.defenderStartTiles);
    }

    private void InitialPosition(List<CombatantPiece3> combatants, List<CombatTile> tiles)
    {
        int middle = tiles.Count / 2;
        for (int i = 0; i < combatants.Count; i++)
        {
            int modifier = (i + 1) / 2;
            if (i % 2 == 1) modifier *= -1;

            CombatantPiece3 combatPiece = combatants[i];
            CombatTile tile = tiles[middle + modifier];

            combatPiece.transform.position = tile.transform.position;
            combatPiece.currentTile = tile;
            tile.occupantPiece = combatPiece;
        }
    }

    public bool Pathfind(UnitPiece3 piece, CombatTile targetTile,
        bool needGroundAccess = true, bool needWaterAccess = false, bool needLavaAccess = false)
    {
        bool result = Pathfinder.FindPath(piece.currentTile, targetTile,
            Pathfinder.HexHeuristic, needGroundAccess, needWaterAccess, needLavaAccess,
            out PathfindResults pathfindResults);
        piece.pieceMovement.SetPath(pathfindResults, targetTile);
        return result;
    }

    public HeroUnitPiece3 GetHero(Player player)
    {
        CombatManager cm = CombatManager.Instance;
        if (player == cm.attackerPlayer) return attackerHero;
        if (player == cm.defenderPlayer) return defenderHero;
        return null;
    }

    public bool GetPieceList(Player owner, bool enemyPieces, out List<CombatantPiece3> list)
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

    public List<CombatantPiece3> GetIdlePieces(List<CombatantPiece3> pieces)
    {
        List<CombatantPiece3> result = new List<CombatantPiece3>();
        foreach (var item in pieces)
        {
            if (item.ICP_IsIdle()) result.Add(item);
        }
        return result;
    }

    public List<CombatantPiece3> GetActivePieces(List<CombatantPiece3> pieces)
    {
        List<CombatantPiece3> result = new List<CombatantPiece3>();
        foreach (var item in pieces)
        {
            if (!item.stateDead) result.Add(item);
        }
        return result;
    }

    public List<CombatantPiece3> GetActivePieces()
    {
        List<CombatantPiece3> result = new List<CombatantPiece3>();
        result.AddRange(attackerPieces);
        result.AddRange(defenderPieces);
        return result;
    }

    public void KillEmAll(List<CombatantPiece3> pieces)
    {
        foreach (var item in pieces)
        {
            item.Die();
        }
    }
}
