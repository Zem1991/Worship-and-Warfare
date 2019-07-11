using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Singleton;

    [Header("Prefabs")]
    public Tile prefabTile;

    [Header("Maps")]
    public Map map;
    //public Map extraMap;

    [Header("Database status")]
    public bool isLoaded;

    void Awake()
    {
        if (Singleton != null)
        {
            Debug.LogWarning("Only one instance of MapManager may exist! Deleting this extra one.");
            Destroy(this);
        }
        else
        {
            Singleton = this;
        }

        isLoaded = true;
        Debug.Log("MapManager loaded!");
    }

    public void BuildMap(Vector2Int size, MapData data)
    {
        map.Create(size, data);
    }
}
