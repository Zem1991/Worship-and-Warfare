using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPieceHandler : MonoBehaviour
{
    [Header("Attacker")]
    public HeroUnitPiece3 attackerHeroUnit;
    public List<CombatUnitPiece3> attackerCombatUnits;
    public List<CombatantPiece3> attackerPieces;

    [Header("Defender")]
    public HeroUnitPiece3 defenderHeroUnit;
    public List<CombatUnitPiece3> defenderCombatUnits;
    public List<CombatantPiece3> defenderPieces;
    public List<WallPiece3> defenderWalls;

    public void Remove()
    {
        if (attackerHeroUnit) Destroy(attackerHeroUnit.gameObject);

        if (attackerCombatUnits != null)
        {
            foreach (var item in attackerCombatUnits) Destroy(item.gameObject);
        }
        attackerCombatUnits = new List<CombatUnitPiece3>();

        attackerPieces = new List<CombatantPiece3>();

        if (defenderHeroUnit) Destroy(defenderHeroUnit.gameObject);

        if (defenderCombatUnits != null)
        {
            foreach (var item in defenderCombatUnits) Destroy(item.gameObject);
        }
        defenderCombatUnits = new List<CombatUnitPiece3>();

        defenderPieces = new List<CombatantPiece3>();

        if (defenderWalls != null)
        {
            foreach (var item in defenderWalls) Destroy(item.gameObject);
        }
        defenderWalls = new List<WallPiece3>();
    }

    public void Create(Player attackerPlayer, Party attackerParty, Player defenderPlayer, Party defenderParty, Town defenderTown)
    {
        Remove();

        CombatMap map = CombatManager.Instance.mapHandler.map;

        HeroUnitPiece3 prefabHero = AllPrefabs.Instance.heroUnitPiece;
        CombatUnitPiece3 prefabUnit = AllPrefabs.Instance.combatUnitPiece;
        WallPiece3 prefabWall = AllPrefabs.Instance.wallPiece;

        //Attacker hero unit
        int spawnId = 0;
        if (attackerParty.GetHeroSlot() != null)
        {
            HeroUnit hero = attackerParty.GetHeroSlot().Get() as HeroUnit;
            if (hero)
            {
                attackerHeroUnit = Instantiate(prefabHero, transform);
                attackerHeroUnit.Initialize(attackerPlayer, spawnId, false, hero);
                attackerPieces.Add(attackerHeroUnit);
            }
        }

        //Defender hero unit
        spawnId = 1;
        if (defenderParty.GetHeroSlot() != null)
        {
            HeroUnit hero = defenderParty.GetHeroSlot().Get() as HeroUnit;
            if (hero)
            {
                defenderHeroUnit = Instantiate(prefabHero, transform);
                defenderHeroUnit.Initialize(defenderPlayer, spawnId, true, hero);
                defenderPieces.Add(defenderHeroUnit);
            }
        }

        //Attacker combat units
        spawnId = 10;
        foreach (var unitSlot in attackerParty.GetUnitSlots())
        {
            CombatUnit unit = unitSlot.Get() as CombatUnit;
            if (unit)
            {
                CombatUnitPiece3 uc = Instantiate(prefabUnit, transform);
                uc.Initialize(attackerPlayer, spawnId, false, unit);
                attackerCombatUnits.Add(uc);
                attackerPieces.Add(uc);
            }
            spawnId += 2;
        }

        //Defender combat units
        spawnId = 11;
        foreach (var unitSlot in defenderParty.GetUnitSlots())
        {
            CombatUnit unit = unitSlot.Get() as CombatUnit;
            if (unit)
            {
                CombatUnitPiece3 uc = Instantiate(prefabUnit, transform);
                uc.Initialize(defenderPlayer, spawnId, true, unit);
                defenderCombatUnits.Add(uc);
                defenderPieces.Add(uc);
            }
            spawnId += 2;
        }

        ////Attacker support units ?
        //spawnId = 100;

        ////Defender support units ?
        //spawnId = 101;

        //Defender town defenses
        if (defenderTown)
        {
            TownDefense wall = defenderTown.wall;
            if (defenderTown.wall)
            {
                spawnId = 1000;
                List<CombatTile> wallTiles = map.GetWallTiles();

                foreach (CombatTile wallTile in wallTiles)
                {
                    WallPiece3 newWall = Instantiate(prefabWall, transform);
                    newWall.Initialize(defenderPlayer, spawnId, wall.GetDBTownDefense());
                    spawnId++;

                    defenderWalls.Add(newWall);
                }
            }

            //TODO: all other possible town defenses
        }
    }

    public void InitialUnitPositions()
    {
        CombatMap map = CombatManager.Instance.mapHandler.map;
        PartyInitialPositions(attackerPieces, map.GetAttackerSpawnTiles());
        PartyInitialPositions(defenderPieces, map.GetDefenderSpawnTiles());
    }

    public void InitialTownDefensePositions()
    {
        CombatMap map = CombatManager.Instance.mapHandler.map;
        WallInitialPositions(defenderWalls, map.GetWallTiles());
    }

    private void PartyInitialPositions(List<CombatantPiece3> combatants, List<CombatTile> tiles)
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

    private void WallInitialPositions(List<WallPiece3> walls, List<CombatTile> tiles)
    {
        if (walls.Count != tiles.Count)
            Debug.LogError("Number of Wall Pieces is different from the number of Tiles.");

        for (int i = 0; i < walls.Count; i++)
        {
            WallPiece3 wall = walls[i];
            CombatTile tile = tiles[i];
            wall.transform.position = tile.transform.position;
            wall.currentTile = tile;
            tile.occupantPiece = wall;
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
        if (player == cm.attackerPlayer) return attackerHeroUnit;
        if (player == cm.defenderPlayer) return defenderHeroUnit;
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
