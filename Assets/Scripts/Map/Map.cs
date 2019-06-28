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
                newTile.lowerLand = tileData.lowerLand;
                newTile.upperLand = tileData.upperLand;
                newTile.water = tileData.water;
                newTile.feature = tileData.feature;
                newTile.road = tileData.road;

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
            DB_Tileset lowerLand = tilesets.Select(item.lowerLand) as DB_Tileset;
            Sprite s = lowerLand.image;
            item.ChangeSprite(s);
        }
    }
}
