﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AbstractMap<T> : MonoBehaviour where T : AbstractTile
{
    [Header("Map data")]
    [SerializeField] protected Vector2Int size;
    [SerializeField] private Dictionary<Vector2Int, T> mapIdToTiles;
    [SerializeField] private Dictionary<int, List<T>> mapYcoordsToTiles;

    public void Remove()
    {
        if (mapIdToTiles != null)
        {
            foreach (var item in mapIdToTiles.Values)
            {
                Destroy(item.gameObject);
            }
        }
        mapIdToTiles = new Dictionary<Vector2Int, T>();
        mapYcoordsToTiles = new Dictionary<int, List<T>>();
    }

    public abstract void Create(Vector2Int size);

    public Vector2Int GetSize()
    {
        return size;
    }

    public void AddTile(Vector2Int id, T tile)
    {
        mapIdToTiles.Add(id, tile);

        mapYcoordsToTiles.TryGetValue(id.y, out List<T> list);
        if (list == null)
        {
            list = new List<T>();
            mapYcoordsToTiles.Add(id.y, list);
        }
        list.Add(tile);
    }

    public List<T> GetAllTiles()
    {
        return Enumerable.ToList(mapIdToTiles.Values);
    }

    public T GetTile(Vector2Int id)
    {
        return mapIdToTiles[id];
    }

    public List<T> GetTiles(List<Vector2Int> ids)
    {
        List<T> result = new List<T>();
        foreach (Vector2Int id in ids)
        {
            result.Add(mapIdToTiles[id]);
        }
        return result;
    }

    public List<T> GetTiles(int yCoord)
    {
        return mapYcoordsToTiles[yCoord];
    }

    public List<T> AreaLine(T startTile, T endTile)
    {
        //Debug.Log("AreaLine start");
        Vector3 startPos = startTile.transform.position;
        Vector3 endPos = endTile.transform.position;

        float difX = endPos.x - startPos.x;
        float difZ = endPos.z - startPos.z;
        difX *= Mathf.Sign(difX);
        difZ *= Mathf.Sign(difZ);
        //float totalDif = difZ + difX;
        float totalDif = difZ + difX + 1;
        float t = 1F / totalDif;

        //Debug.Log("totalDif = " + totalDif);
        //Debug.Log("t = " + t);

        List<Vector2Int> coordinates = new List<Vector2Int>();
        for (int i = 0; i < totalDif; i++)
        {
            Vector3 vector = Vector3.Lerp(startPos, endPos, t * i);
            Vector2Int vectorInt = new Vector2Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.z));
            coordinates.Add(vectorInt);
        }
        //Debug.Log("AreaLine finish");
        return GetTiles(coordinates);
    }
}
