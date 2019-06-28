using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    // This node on the "visible" grid
    public Tile tile;

    // Where this node came from during the pathfinding process
    public PathNode previous;

    public float gCost_DistFromStart;
    public float hCost_DistFromTarget;
    public float fCost_totalDistance { get { return gCost_DistFromStart + hCost_DistFromTarget; } }

    public PathNode(Tile tile)
    {
        this.tile = tile;
    }
}
