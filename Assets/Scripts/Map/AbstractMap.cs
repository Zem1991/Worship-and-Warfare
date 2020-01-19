using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractMap<T> : MonoBehaviour where T : AbstractTile
{
    [Header("Data")]
    public Vector2Int mapSize;
    public Dictionary<Vector2Int, T> tiles;

    public virtual void Remove()
    {
        if (tiles != null)
        {
            foreach (var item in tiles.Values)
            {
                Destroy(item.gameObject);
            }
        }
        tiles = new Dictionary<Vector2Int, T>();
    }

    public abstract void Create(Vector2Int size);

    public List<T> GetTiles(List<Vector2Int> ids)
    {
        List<T> result = new List<T>();
        foreach (Vector2Int id in ids)
        {
            result.Add(tiles[id]);
        }
        return result;
    }

    public List<T> AreaLine(T startTile, T endTile)
    {
        Debug.Log("AreaLine start");
        Vector3 startPos = startTile.transform.position;
        Vector3 endPos = endTile.transform.position;

        float difX = endPos.x - startPos.x;
        float difZ = endPos.z - startPos.z;
        difX *= Mathf.Sign(difX);
        difZ *= Mathf.Sign(difZ);
        //float totalDif = difZ + difX;
        float totalDif = difZ + difX + 1;
        float t = 1F / totalDif;

        Debug.Log("totalDif = " + totalDif);
        Debug.Log("t = " + t);

        List<Vector2Int> coordinates = new List<Vector2Int>();
        for (int i = 0; i < totalDif; i++)
        {
            Vector3 vector = Vector3.Lerp(startPos, endPos, t * i);
            Vector2Int vectorInt = new Vector2Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.z));
            coordinates.Add(vectorInt);
        }
        Debug.Log("AreaLine finish");
        return GetTiles(coordinates);
    }

    //public List<T> AreaLine(Vector3 startPos, Vector3 endPos)
    //{
    //    Debug.Log("AreaLine start");
    //    Vector3 centerFix = new Vector3(0.5F, 0, 0.5F);

    //    Vector3 direction = (endPos + centerFix) - (startPos + centerFix);
    //    float maxDistance = direction.magnitude;
    //    Debug.Log(startPos + centerFix);
    //    Debug.Log(endPos + centerFix);
    //    Debug.Log(direction);
    //    Debug.Log(maxDistance);
    //    RaycastHit[] hits = Physics.RaycastAll(startPos + centerFix, direction, maxDistance);

    //    List<Vector2Int> coordinates = new List<Vector2Int>();
    //    foreach (var item in hits)
    //    {
    //        GameObject gobj = item.collider.gameObject;
    //        AbstractTile tile = gobj.GetComponent<AbstractTile>();
    //        if (!tile) tile = gobj.GetComponentInParent<AbstractTile>();
    //        if (!tile) continue;

    //        Vector2Int pos = tile.posId;
    //        if (coordinates.Contains(pos)) continue;

    //        coordinates.Add(pos);
    //    }
    //    Debug.Log("AreaLine finish");
    //    return GetTiles(coordinates);
    //}
}
