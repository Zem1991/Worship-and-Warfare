using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [Header("Data")]
    public Tile[,] tiles;

    public void Remove()
    {
        if (tiles != null)
        {
            foreach (var item in tiles)
            {
                Destroy(item);
            }
            tiles = null;
        }
    }

    public void Create(Vector2Int size, MapData mapData)
    {
        Remove();
        tiles = new Tile[size.x, size.y];

        int current = 0;
        for (int row = 0; row < size.y; row++)
        {
            for (int col = 0; col < size.x; col++)
            {
                Tile prefab = MapManager.Singleton.prefabTile;
                Vector3 pos = new Vector3(col, 0, row);
                Quaternion rot = Quaternion.identity;

                Tile newTile = Instantiate(prefab, pos, rot, transform);
                tiles[col, row] = newTile;

                TileData tileData = mapData.tiles[current];
                newTile.id = new Vector2Int(col, row);
                newTile.lowerLandId = tileData.lowerLandId;
                newTile.upperLandId = tileData.upperLandId;
                newTile.waterId = tileData.waterId;
                newTile.featureId = tileData.featureId;
                newTile.roadId = tileData.roadId;

                newTile.name = newTile.id.ToString();
                current++;

                if (col > 0)
                {
                    newTile.l = tiles[col - 1, row];
                    tiles[col - 1, row].r = newTile;
                }
                if (row > 0)
                {
                    newTile.b = tiles[col, row - 1];
                    tiles[col, row - 1].f = newTile;
                }
                if (col > 0 && row > 0)
                {
                    newTile.bl = tiles[col - 1, row - 1];
                    tiles[col - 1, row - 1].fr = newTile;
                }
                if (col < size.x - 1 && row > 0)
                {
                    newTile.br = tiles[col + 1, row - 1];
                    tiles[col + 1, row - 1].fl = newTile;
                }
            }
        }

        TileSprites();
    }

    private void TileSprites()
    {
        DatabaseManager db = DatabaseManager.Singleton;

        DBHandler_Tileset tilesets = db.tilesets;
        foreach (var item in tiles)
        {
            DB_Tileset lowerLand = tilesets.Select(item.lowerLandId) as DB_Tileset;
            Sprite s = lowerLand.image;
            item.ChangeLandSprite(s);

            item.groundMovementCost = lowerLand.groundMovementCost;
            item.allowGroundMovement = lowerLand.allowGroundMovement;
            item.allowWaterMovement = lowerLand.allowWaterMovement;
            item.allowLavaMovement = lowerLand.allowLavaMovement;

            DB_Tileset feature = tilesets.Select(item.featureId, false) as DB_Tileset;
            if (feature)
            {
                s = feature.image;
                item.ChangeFeatureSprite(s);

                item.groundMovementCost += feature.groundMovementCost;
                item.allowGroundMovement &= feature.allowGroundMovement;
                item.allowWaterMovement &= feature.allowWaterMovement;
                item.allowLavaMovement &= feature.allowLavaMovement;
            }
        }
    }
}
