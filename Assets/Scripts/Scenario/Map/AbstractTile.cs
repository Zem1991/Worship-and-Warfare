using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZemDirections;

public abstract class AbstractTile : MonoBehaviour
{
    [Header("Identification")]
    public int id;
    public Vector2Int posId;

    [Header("Stats")]
    public int groundMovementCost;
    public bool allowGroundMovement;
    public bool allowWaterMovement;
    public bool allowLavaMovement;

    [Header("Known Stuff")]
    public AbstractTile bl;
    public AbstractTile b;
    public AbstractTile br;
    public AbstractTile l;
    public AbstractTile r;
    public AbstractTile fl;
    public AbstractTile f;
    public AbstractTile fr;
    public AbstractPiece3 occupantPiece;
    public AbstractPiece3 obstaclePiece;

    public bool IsAcessible(bool needGroundAccess, bool needWaterAccess, bool needLavaAccess, bool needNoObstacle)
    {
        if (needGroundAccess && !allowGroundMovement) return false;
        if (needWaterAccess && !allowWaterMovement) return false;
        if (needLavaAccess && !allowLavaMovement) return false;
        if (needNoObstacle && obstaclePiece) return false;
        return true;
    }

    public List<AbstractTile> GetNeighbours()
    {
        List<AbstractTile> result = new List<AbstractTile>();
        if (bl) result.Add(bl);
        if (b) result.Add(b);
        if (br) result.Add(br);
        if (l) result.Add(l);
        if (r) result.Add(r);
        if (fl) result.Add(fl);
        if (f) result.Add(f);
        if (fr) result.Add(fr);
        return result;
    }

    public List<AbstractTile> GetAccessibleNeighbours(bool needGroundAccess, bool needWaterAccess, bool needLavaAccess)
    {
        List<AbstractTile> result = new List<AbstractTile>();
        foreach (var item in GetNeighbours())
        {
            if (!item.IsAcessible(needGroundAccess, needWaterAccess, needLavaAccess, true)) continue;
            result.Add(item);
        }
        return result;
    }

    public List<AbstractTile> GetBlockedNeighbours()
    {
        List<AbstractTile> result = new List<AbstractTile>();
        foreach (var item in GetNeighbours())
        {
            if (!item.obstaclePiece) continue;
            result.Add(item);
        }
        return result;
    }

    public OctoDirXZ GetNeighbourDirection(AbstractTile t)
    {
        if (t == bl) return OctoDirXZ.BACK_LEFT;
        if (t == b) return OctoDirXZ.BACK;
        if (t == br) return OctoDirXZ.BACK_RIGHT;
        if (t == l) return OctoDirXZ.LEFT;
        if (t == r) return OctoDirXZ.RIGHT;
        if (t == fl) return OctoDirXZ.FRONT_LEFT;
        if (t == f) return OctoDirXZ.FRONT;
        if (t == fr) return OctoDirXZ.FRONT_RIGHT;
        return OctoDirXZ.NONE;
    }

    public bool IsNeighbour(AbstractTile t)
    {
        return GetNeighbourDirection(t) != OctoDirXZ.NONE;
    }

    //public List<AbstractTile> GetAccessibleTiles(bool needGroundAccess, bool needWaterAccess, bool needLavaAccess, float movementRange)
    //{
    //    List<AbstractTile> currentList = new List<AbstractTile>();

    //    List<AbstractTile> accessibleNeighbours = new List<AbstractTile>();
    //    foreach (var item in GetNeighbours())
    //    {
    //        if (!item.IsAcessible(needGroundAccess, needWaterAccess, needLavaAccess, true)) continue;
    //        accessibleNeighbours.Add(item);
    //    }

    //    foreach (var item in accessibleNeighbours)
    //    {
    //        if (currentList.Contains(item)) continue;
    //        if (movementRange < item.groundMovementCost) continue;

    //        currentList.Add(item);

    //        List<AbstractTile> moreTiles = item.GetAccessibleTiles(needGroundAccess, needWaterAccess, needLavaAccess, movementRange - item.groundMovementCost);
    //        currentList.AddRange(moreTiles);
    //    }

    //    return currentList;
    //}
}
