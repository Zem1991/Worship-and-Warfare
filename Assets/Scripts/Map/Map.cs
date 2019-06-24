using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [Header("Data")]
    public List<Tile> tiles;

    public void Remove()
    {
        foreach (var item in tiles)
        {
            Destroy(item);
        }
        tiles.Clear();
    }

    public void Create(Vector2Int size, MapData mapData)
    {
        Remove();
        tiles = new List<Tile>();

        int current = 0;
        for (int row = 0; row < size.y; row++)
        {
            for (int col = 0; col < size.x; col++)
            {
                Tile prefab = MapManager.Singleton.prefabTile;
                Vector3 pos = new Vector3(col, 0, row);
                Quaternion rot = Quaternion.identity;

                Tile newTile = Instantiate(prefab, pos, rot, transform);
                tiles.Add(newTile);

                TileData tileData = mapData.tiles[current];
                newTile.id = size;
                newTile.lowerLand = tileData.lowerLand;
                newTile.upperLand = tileData.upperLand;
                newTile.water = tileData.water;
                newTile.feature = tileData.feature;
                newTile.road = tileData.road;

                newTile.name = size.ToString();
                current++;
            }
        }

        TileNeighbours();

        TileSprites();
    }

    private void TileNeighbours()
    {
        Debug.Log("Pretend tile neighbours were discovered.");
    }

    private void TileSprites()
    {
        List<DBContent> tilesets = DatabaseManager.Singleton.tilesets.content;
        foreach (var item in tiles)
        {
            DB_Tileset lowerLand = tilesets[item.lowerLand] as DB_Tileset;
            Sprite s = lowerLand.image;
            item.ChangeSprite(s);
        }
    }
}
