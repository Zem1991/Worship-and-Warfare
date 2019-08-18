using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMapHandler : MonoBehaviour
{
    [Header("Maps")]
    public CombatMap map;

    public void BuildMap(Vector2Int size, DB_Tileset tileset)
    {
        map.Create(size);
        map.ApplyTileset(tileset);
    }
}
