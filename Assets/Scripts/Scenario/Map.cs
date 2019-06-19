using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public Vector2 size;
    public List<Tile> tiles;

    public void Remove()
    {
        size = Vector2.zero;

        foreach (var item in tiles)
        {
            Destroy(item);
        }
        tiles.Clear();
    }

    public void Create(MapData data)
    {
        Remove();

        size = new Vector2(data.width, data.height);
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

                newTile.id = new Vector2(col, row);
                newTile.type = (TileType)data.tiles[current];
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
        foreach (var item in tiles)
        {
            int id = (int)item.type;
            Sprite s = MapManager.Singleton.terrainSprites[id];
            item.ChangeSprite(s);
        }
    }
}
