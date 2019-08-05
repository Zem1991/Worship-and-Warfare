using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldMapHandler : MonoBehaviour
{
    [Header("Maps")]
    public FieldMap map;
    //public FieldMap extraMap;

    public void BuildMap(Vector2Int size, MapData surfaceMap)
    {
        map.Create(size);
        map.ApplyMapData(surfaceMap);
        //if (subterraneanMap)
    }
}
