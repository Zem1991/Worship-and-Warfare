using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapData
{
    public int width;
    public int height;
    public int[] tiles;

    public MapData(int width, int height, int[] tiles)
    {
        this.width = width;
        this.height = height;
        this.tiles = tiles;
    }
}
