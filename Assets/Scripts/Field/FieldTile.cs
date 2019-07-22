using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZemDirections;

public class FieldTile : MonoBehaviour
{
    [Header("Renderers")]
    public SpriteRenderer landRenderer;
    public SpriteRenderer featureRenderer;

    [Header("Stats")]
    public Vector2Int id;
    public int groundMovementCost;
    public bool allowGroundMovement;
    public bool allowWaterMovement;
    public bool allowLavaMovement;

    [Header("Tilesets Used")]
    public string lowerLandId;
    public string upperLandId;
    public string waterId;
    public string featureId;
    public string roadId;

    [Header("Neighbours")]
    public FieldTile bl;
    public FieldTile b;
    public FieldTile br;
    public FieldTile l;
    public FieldTile r;
    public FieldTile fl;
    public FieldTile f;
    public FieldTile fr;

    [Header("Other Objects")]
    public Piece piece;

    //void Awake()
    //{
    //    landRenderer = GetComponentInChildren<SpriteRenderer>();
    //}

    public bool IsOccupied()
    {
        return piece;
    }

    public bool IsAcessible(bool needGroundAccess, bool needWaterAccess, bool needLavaAccess)
    {
        if (needGroundAccess && !allowGroundMovement) return false;
        if (needWaterAccess && !allowWaterMovement) return false;
        if (needLavaAccess && !allowLavaMovement) return false;
        return true;
    }

    public List<FieldTile> GetAccessibleNeighbours(bool needGroundAccess, bool needWaterAccess, bool needLavaAccess)
    {
        List<FieldTile> result = new List<FieldTile>();
        foreach (var item in GetNeighbours())
        {
            if (!item.IsAcessible(needGroundAccess, needWaterAccess, needLavaAccess)) continue;
            result.Add(item);
        }
        return result;
    }

    public OctoDirXZ GetNeighbourDirection(FieldTile t)
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

    public void ChangeLandSprite(Sprite s)
    {
        landRenderer.sprite = s;
    }

    public void ChangeFeatureSprite(Sprite s)
    {
        featureRenderer.sprite = s;
    }

    private List<FieldTile> GetNeighbours()
    {
        List<FieldTile> result = new List<FieldTile>();
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
}
