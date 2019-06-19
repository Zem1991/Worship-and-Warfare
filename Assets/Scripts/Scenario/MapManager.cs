using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    WATER,
    GRASSLAND
}

public class MapManager : MonoBehaviour
{
    public static MapManager Singleton;

    [Header("Prefabs")]
    public Tile prefabTile;

    [Header("Maps")]
    public Map mapSurface;

    [Header("Terrain Sprites")]
    public List<Sprite> terrainSprites;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateMap(MapData data)
    {
        mapSurface.Create(data);
    }
}
