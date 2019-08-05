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
}
