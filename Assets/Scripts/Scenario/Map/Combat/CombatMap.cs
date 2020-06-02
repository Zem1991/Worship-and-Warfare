using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatMap : AbstractMap<CombatTile>
{
    public override void Create(Vector2Int size)
    {
        Remove();

        base.size = size;
        CombatTile prefabTile = AllPrefabs.Instance.combatTile;

        prefabTile.groundMovementCost = 100;    //TODO GET MOVEMENT COSTS LATER
        prefabTile.allowGroundMovement = true;  //TODO GET MOVEMENT TYPES LATER

        int width = base.size.x;
        //int height = mapSize.y;
        //int maxX = mapSize.x - 1;
        int maxY = base.size.y - 1;

        Vector3 startPos = new Vector3();
        startPos.x = 0; // (width - 1) / -2F;
        startPos.z = 0; // (height - 1) / -2F;

        int current = 0;
        CombatTile previousRowReference = null;
        CombatTile previousRowReferenceLeft = null;
        for (int row = maxY; row >= 0; row--)
        {
            CombatTile firstOfCurrentRow = null;
            CombatTile previousInCurrentRow = null;

            int maxCol = width;
            float colPosAdjust = 0;
            bool oddCol = row % 2 == 1;
            if (oddCol)
            {
                maxCol++;
                colPosAdjust = -0.5F;
            }

            for (int col = 0; col < maxCol; col++)
            {
                Vector3 pos = startPos;
                pos.x += col + colPosAdjust;
                pos.z += row;
                Quaternion rot = Quaternion.identity;

                Vector2Int tileId = new Vector2Int(col, row);
                CombatTile newTile = Instantiate(prefabTile, pos, rot, transform);
                AddTile(tileId, newTile);

                newTile.id = current;
                newTile.posId = tileId;
                newTile.name = "Tile #" + current + " " + tileId.ToString();
                current++;

                if (!firstOfCurrentRow)
                {
                    firstOfCurrentRow = newTile;
                }

                if (previousInCurrentRow) previousInCurrentRow.r = newTile;
                newTile.l = previousInCurrentRow;

                if (previousRowReference)
                {
                    if (oddCol)
                    {
                        newTile.fr = previousRowReference;
                        newTile.fl = previousRowReference.l;
                        newTile.fr.bl = newTile;
                        if (newTile.fl) newTile.fl.br = newTile;
                    }
                    else
                    {
                        newTile.fl = previousRowReference;
                        newTile.fr = previousRowReference.r;
                        newTile.fl.br = newTile;
                        newTile.fr.bl = newTile;
                    }
                    previousRowReferenceLeft = previousRowReference;
                    previousRowReference = previousRowReference.r as CombatTile;
                }
                else if (previousRowReferenceLeft)
                {
                    newTile.fl = previousRowReferenceLeft;
                    newTile.fl.br = newTile;
                }

                previousInCurrentRow = newTile;

                int lastCol = maxCol - 1;
                if (col == lastCol)
                {
                    previousRowReference = firstOfCurrentRow;
                    previousRowReferenceLeft = firstOfCurrentRow.l as CombatTile;
                }
            }
        }
    }

    public void ApplyTileset(DB_Tileset tileset)
    {
        Sprite s = tileset.image;
        foreach (var tile in GetAllTiles())
        {
            tile.db_tileset_battleground = tileset;
            tile.ChangeLandSprite(s);
        }
    }

    public void AddRandomObstacles(DB_Tileset tileset)
    {
        if (tileset.combatObstacles.Count <= 0) return;

        DoodadPiece3 prefab = AllPrefabs.Instance.doodadPiece;
        System.Random rand = new System.Random();
        List<CombatTile> values = GetAllTiles();

        int obstaclesToMake = UnityEngine.Random.Range(0, 20);
        for (int i = 0; i < obstaclesToMake; i++)
        {
            CombatTile cTile = values[rand.Next(values.Count)];
            int maxTilesX = size.x - 1;
            if (cTile.posId.y % 2 == 1) maxTilesX++;

            if (cTile.posId.x <= 2) continue;
            if (cTile.posId.x >= maxTilesX - 2) continue;
            if (cTile.GetBlockedNeighbours().Count > 0) continue;

            int aux = rand.Next(tileset.combatObstacles.Count);
            DB_CombatObstacle dbObstacle = tileset.combatObstacles[aux];

            DoodadPiece3 obstacle = Instantiate(prefab, cTile.transform);
            obstacle.SetMainSprite(dbObstacle.imgObstacle, SpriteOrderConstants.OBSTACLE);
            obstacle.currentTile = cTile;
            cTile.obstaclePiece = obstacle;
        }
    }

    public List<CombatTile> GetAttackerSpawnTiles()
    {
        List<CombatTile> result = new List<CombatTile>();
        for (int y = 0; y < size.y; y++)
        {
            List<CombatTile> list = GetTiles(y);
            CombatTile tile = list[0];
            result.Add(tile);
        }
        return result;
    }

    public List<CombatTile> GetDefenderSpawnTiles()
    {
        List<CombatTile> result = new List<CombatTile>();
        for (int y = 0; y < size.y; y++)
        {
            List<CombatTile> list = GetTiles(y);
            CombatTile tile = list[list.Count - 1];
            result.Add(tile);
        }
        return result;
    }

    public List<CombatTile> GetWallTiles()
    {
        List<CombatTile> result = new List<CombatTile>();
        int total = size.y;
        for (int y = 0; y < total; y++)
        {
            if (y == total / 2) continue;

            List<CombatTile> list = GetTiles(y);
            CombatTile tile = list[list.Count - 4];
            result.Add(tile);
        }
        return result;
    }
}
