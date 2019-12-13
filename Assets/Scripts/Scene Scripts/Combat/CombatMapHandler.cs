using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMapHandler : MonoBehaviour
{
    [Header("Maps")]
    public CombatMap map;

    public void ClearMap()
    {
        map.Remove();
    }

    public void BuildMap(Vector2Int size, DB_Tileset tileset)
    {
        map.Create(size);
        map.ApplyTileset(tileset);
    }

    public void AddRandomObstacles(DB_Tileset tileset)
    {
        map.AddRandomObstacles(tileset);
    }
}
