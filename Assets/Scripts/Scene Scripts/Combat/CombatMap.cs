﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMap : AbstractMap<CombatTile>
{
    [Header("Common Tiles")]
    public List<CombatTile> attackerStartTiles;
    public List<CombatTile> defenderStartTiles;

    [Header("Special Tiles")]
    public CombatTile attackerHeroTile;
    public CombatTile defenderHeroTile;

    public override void Remove()
    {
        base.Remove();

        if (attackerStartTiles != null)
        {
            foreach (var item in attackerStartTiles)
            {
                Destroy(item.gameObject);
            }
        }
        attackerStartTiles = new List<CombatTile>();

        if (defenderStartTiles != null)
        {
            foreach (var item in defenderStartTiles)
            {
                Destroy(item.gameObject);
            }
        }
        defenderStartTiles = new List<CombatTile>();

        if (attackerHeroTile) Destroy(attackerHeroTile.gameObject);
        if (defenderHeroTile) Destroy(defenderHeroTile.gameObject);
    }

    public override void Create(Vector2Int size)
    {
        Remove();
        mapSize = size;

        CombatTile prefabTile = CombatManager.Instance.prefabTile;
        prefabTile.groundMovementCost = 100;    //TODO GET MOVEMENT COSTS LATER
        prefabTile.allowGroundMovement = true;  //TODO GET MOVEMENT TYPES LATER

        int width = mapSize.x;
        int height = mapSize.y;

        Vector3 startPos = new Vector3();
        startPos.x = 0; // (width - 1) / -2F;
        startPos.z = 0; // (height - 1) / -2F;

        int current = 0;
        CombatTile previousRowReference = null;
        CombatTile previousRowReferenceLeft = null;
        for (int row = 0; row < height; row++)
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
                tiles.Add(tileId, newTile);

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
                        newTile.br = previousRowReference;
                        newTile.bl = previousRowReference.l;
                        newTile.br.fl = newTile;
                        if (newTile.bl) newTile.bl.fr = newTile;
                    }
                    else
                    {
                        newTile.bl = previousRowReference;
                        newTile.br = previousRowReference.r;
                        newTile.bl.fr = newTile;
                        newTile.br.fl = newTile;
                    }
                    previousRowReferenceLeft = previousRowReference;
                    previousRowReference = previousRowReference.r as CombatTile;
                }
                else if (previousRowReferenceLeft)
                {
                    newTile.bl = previousRowReferenceLeft;
                    newTile.bl.fr = newTile;
                }

                previousInCurrentRow = newTile;

                int lastCol = maxCol - 1;
                if (col == lastCol)
                {
                    previousRowReference = firstOfCurrentRow;
                    previousRowReferenceLeft = firstOfCurrentRow.l as CombatTile;
                }

                //Set attacker's start tiles
                if (col == 0) attackerStartTiles.Add(newTile);
                //Set defender's start tiles
                if (col == lastCol) defenderStartTiles.Add(newTile);
            }
        }

        Vector3 heroTilePos = new Vector3();
        heroTilePos.x += -1.5F;
        heroTilePos.z += size.y / 2;
        Quaternion heroTileRot = Quaternion.identity;
        attackerHeroTile = Instantiate(prefabTile, heroTilePos, heroTileRot, transform);

        heroTilePos.x *= -1;
        heroTilePos.x += size.x - 1;
        defenderHeroTile = Instantiate(prefabTile, heroTilePos, heroTileRot, transform);
    }

    public void ApplyTileset(DB_Tileset tileset)
    {
        Sprite s = tileset.image;
        foreach (var tile in tiles.Values)
        {
            tile.db_tileset_battleground = tileset;
            tile.ChangeLandSprite(s);
        }
        attackerHeroTile.db_tileset_battleground = tileset;
        attackerHeroTile.ChangeLandSprite(s);
        defenderHeroTile.db_tileset_battleground = tileset;
        defenderHeroTile.ChangeLandSprite(s);
    }
}