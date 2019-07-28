using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZemDirections;

public class CombatTile : MonoBehaviour
{
    [Header("Identification")]
    public int id;
    public int rowId;
    public int colId;

    [Header("Neighbours")]
    public CombatTile bl;
    public CombatTile br;
    public CombatTile l;
    public CombatTile r;
    public CombatTile fl;
    public CombatTile fr;

    [Header("Objects")]
    public UnitCombat unit;

    public bool IsOccupied()
    {
        return unit;
    }

    public List<CombatTile> GetNeighbours()
    {
        List<CombatTile> result = new List<CombatTile>();
        if (bl) result.Add(bl);
        if (br) result.Add(br);
        if (l) result.Add(l);
        if (r) result.Add(r);
        if (fl) result.Add(fl);
        if (fr) result.Add(fr);
        return result;
    }

    public List<CombatTile> GetAccessibleNeighbours()
    {
        List<CombatTile> result = new List<CombatTile>();
        foreach (var item in GetNeighbours())
        {
            if (!item.IsOccupied()) continue;
            result.Add(item);
        }
        return result;
    }

    public OctoDirXZ GetNeighbourDirection(CombatTile t)
    {
        if (t == bl) return OctoDirXZ.BACK_LEFT;
        if (t == br) return OctoDirXZ.BACK_RIGHT;
        if (t == l) return OctoDirXZ.LEFT;
        if (t == r) return OctoDirXZ.RIGHT;
        if (t == fl) return OctoDirXZ.FRONT_LEFT;
        if (t == fr) return OctoDirXZ.FRONT_RIGHT;
        return OctoDirXZ.NONE;
    }
}
