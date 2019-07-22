using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    [Header("Prefabs")]
    public FieldTile prefabTile;

    [Header("Maps")]
    public FieldMap map;
    //public FieldMap extraMap;

    [Header("Database status")]
    public bool isLoaded;

    public void BuildMap(Vector2Int size, MapData data)
    {
        map.Create(size, data);
    }
}
