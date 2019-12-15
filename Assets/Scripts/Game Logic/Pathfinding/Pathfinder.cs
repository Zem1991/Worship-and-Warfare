using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PathfindResults
{
    public List<PathNode> path;
    public float pathTotalCost;
    public int operations;

    public PathfindResults(List<PathNode> path, float pathTotalCost, int operations)
    {
        this.path = path;
        this.pathTotalCost = pathTotalCost;
        this.operations = operations;
    }
}

public static class Pathfinder
{
    public const float DIAGONAL_MODIFIER_OCTO = 0.7F;
    //public const float DIAGONAL_MODIFIER_HEX = 0.87F;

    public static bool FindPath(AbstractTile startTile, AbstractTile targetTile, Func<PathNode, PathNode, float> heuristic,
        bool needGroundAccess, bool needWaterAccess, bool needLavaAccess,
        out PathfindResults pathfindResults)
    {
        pathfindResults = new PathfindResults(new List<PathNode>(), 0, 0);
        if (!targetTile.IsAcessible(needGroundAccess, needWaterAccess, needLavaAccess, true)) return false;
        //if (!targetTile.occupantPiece && !targetTile.IsAcessible(needGroundAccess, needWaterAccess, needLavaAccess)) return false;

        PathNode startPN = new PathNode(startTile);
        PathNode targetPN = new PathNode(targetTile);

        HashSet<Vector2Int> closedList = new HashSet<Vector2Int>();
        List<PathNode> openList = new List<PathNode>();
        openList.Add(startPN);
        while (openList.Count > 0)
        {
            // New operation: process an new PathNode
            pathfindResults.operations++;

            // Get the PathNode with the lowest totalDistance/fCost
            PathNode currentPN = openList[0];
            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].fCost_totalDistance < currentPN.fCost_totalDistance ||
                    (openList[i].fCost_totalDistance == currentPN.fCost_totalDistance && openList[i].hCost_DistFromTarget < currentPN.hCost_DistFromTarget))
                {
                    currentPN = openList[i];
                }
            }

            openList.Remove(currentPN);
            closedList.Add(currentPN.tile.posId);

            // Make path if the target node was found
            if (currentPN.tile.id == targetTile.id)
            {
                pathfindResults = MakePath(pathfindResults, startPN, currentPN);
                return true;
            }

            // Identify and process accessible neighbouring nodes
            foreach (AbstractTile neighbour in currentPN.tile.GetAccessibleNeighbours(needGroundAccess, needWaterAccess, needLavaAccess))
            {
                // Neighbour node cannot have a piece over it, UNLESS it's the target node.
                if (neighbour.occupantPiece)
                {
                    if (neighbour != targetTile) continue;
                }

                // Neighbour node cannot be on the closed set
                PathNode neighbourPN = new PathNode(neighbour);
                if (closedList.Contains(neighbour.posId)) continue;

                // Switch to existing node if possible
                PathNode existingPN = ListContainsNodeId(openList, neighbour.posId);
                bool neighbourOnOpenList = (existingPN != null);
                if (neighbourOnOpenList) neighbourPN = existingPN;

                int moveCost = Mathf.CeilToInt(heuristic(currentPN, neighbourPN));
                float gCost = currentPN.gCost_DistFromStart + moveCost;
                if (!neighbourOnOpenList || gCost < neighbourPN.gCost_DistFromStart)
                {
                    neighbourPN.moveCost = moveCost;
                    neighbourPN.gCost_DistFromStart = gCost;
                    neighbourPN.hCost_DistFromTarget = heuristic(neighbourPN, targetPN);
                    neighbourPN.previous = currentPN;
                    if (!neighbourOnOpenList) openList.Add(neighbourPN);
                }
            }
        }

        return false;
    }

    public static float OctoHeuristic(PathNode from, PathNode to)
    {
        //int fromCost = from.tile.groundMovementCost;
        //int toCost = to.tile.groundMovementCost;
        //float fromToCost = (fromCost + toCost) / 2F;
        float fromToCost = to.tile.groundMovementCost;

        Vector2Int fromId = from.tile.posId;
        Vector2Int toId = to.tile.posId;
        int distX = Mathf.Abs(fromId.x - toId.x);
        int distY = Mathf.Abs(fromId.y - toId.y);
        bool isDiagonal = (distX != 0 && distY != 0);

        float result = distX + distY;
        result *= fromToCost;
        if (isDiagonal) result *= DIAGONAL_MODIFIER_OCTO;
        return result;
    }

    public static float HexHeuristic(PathNode from, PathNode to)
    {
        //Vector2Int fromId = from.tile.posId;
        //Vector2Int toId = to.tile.posId;
        //int distX = Mathf.Abs(fromId.x - toId.x);
        //int distY = Mathf.Abs(fromId.y - toId.y);
        //bool isDiagonal = (distX != 0 && distY != 0);

        //int fromCost = from.tile.groundMovementCost;
        //int toCost = to.tile.groundMovementCost;
        //float combinedCosts = (fromCost + toCost) / 2F;
        //return combinedCosts;

        //float result = distX + distY;
        //result *= combinedCosts;
        //if (isDiagonal) result *= DIAGONAL_MODIFIER_HEX;      //TODO IDK
        //return result;

        //int fromCost = from.tile.groundMovementCost;
        //int toCost = to.tile.groundMovementCost;
        //float fromToCost = (fromCost + toCost) / 2F;
        float fromToCost = to.tile.groundMovementCost;
        float result = fromToCost;
        return result;
    }

    private static PathNode ListContainsNodeId(IList<PathNode> list, Vector2Int id)
    {
        foreach (PathNode item in list)
        {
            if (item.tile.posId == id)
                return item;
        }
        return null;
    }

    //private static float DistanceCalcs(PathNode from, PathNode to, Func<PathNode, PathNode, float> heuristic)
    //{
    //    return heuristic(from, to);
    //}

    private static PathfindResults MakePath(PathfindResults pathfindResults, PathNode startNode, PathNode targetNode)
    {
        PathNode currentNode = targetNode;
        while (currentNode != startNode)
        {
            pathfindResults.path.Add(currentNode);
            pathfindResults.pathTotalCost += currentNode.moveCost;
            //pathfindResults.pathCost += heuristic(currentNode, currentNode.previous);
            currentNode = currentNode.previous;
        }
        pathfindResults.path.Reverse();
        return pathfindResults;
    }
}
