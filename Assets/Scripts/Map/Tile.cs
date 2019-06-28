using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZemDirections;

public class Tile : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public Vector2Int id;

    [Header("Tilesets")]
    public int lowerLand;
    public int upperLand;
    public int water;
    public int feature;
    public int road;

    [Header("Neighbours")]
    public Tile bl;
    public Tile b;
    public Tile br;
    public Tile l;
    public Tile r;
    public Tile fl;
    public Tile f;
    public Tile fr;

    [Header("Other Objects")]
    public Piece piece;

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public List<Tile> GetNeighbours()
    {
        List<Tile> result = new List<Tile>();
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

    public OctoDirXZ GetNeighbourDirection(Tile t)
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

    public void ChangeSprite(Sprite s)
    {
        spriteRenderer.sprite = s;
    }
}
