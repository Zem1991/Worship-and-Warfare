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
    public AbstractPiece2 occupantPiece;

    public bool IsAcessible(bool needGroundAccess, bool needWaterAccess, bool needLavaAccess)
    {
        if (needGroundAccess && !allowGroundMovement) return false;
        if (needWaterAccess && !allowWaterMovement) return false;
        if (needLavaAccess && !allowLavaMovement) return false;
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
            if (!item.IsAcessible(needGroundAccess, needWaterAccess, needLavaAccess)) continue;
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
}
