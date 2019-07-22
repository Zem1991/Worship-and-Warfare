using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Pathfinder
{
    public const float DIAGONAL_MODIFIER = 0.7F;

    public static void FindPath(FieldTile startTile, FieldTile targetTile, out List<PathNode> result, out float pathCost,
        bool needGroundAccess, bool needWaterAccess, bool needLavaAccess)
    {
        result = null;
        pathCost = 0;
        float operations = 0;
        if (!targetTile.piece && !targetTile.IsAcessible(needGroundAccess, needWaterAccess, needLavaAccess)) return;

        PathNode startPN = new PathNode(startTile);
        PathNode targetPN = new PathNode(targetTile);

        HashSet<Vector2Int> closedList = new HashSet<Vector2Int>();
        List<PathNode> openList = new List<PathNode>();
        openList.Add(startPN);
        while (openList.Count > 0)
        {
            // New operation: process an PathNode
            operations++;

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
            closedList.Add(currentPN.tile.id);

            // Make path if the target node was found
            if (currentPN.tile.id == targetTile.id)
            {
                MakePath(startPN, currentPN, out result, out pathCost);
                return;
            }

            // Identify and process accessible neighbouring nodes
            foreach (FieldTile neighbour in currentPN.tile.GetAccessibleNeighbours(needGroundAccess, needWaterAccess, needLavaAccess))
            {
                // Neighbour node cannot have a piece over it, UNLESS it's the target node.
                if (neighbour.piece)
                {
                    if (neighbour != targetTile) continue;
                }

                // Neighbour node cannot be on the closed set
                PathNode neighbourPN = new PathNode(neighbour);
                if (closedList.Contains(neighbour.id)) continue;

                // Switch to existing node if possible
                PathNode existingPN = ListContainsNodeId(openList, neighbour.id);
                bool neighbourOnOpenList = (existingPN != null);
                if (neighbourOnOpenList) neighbourPN = existingPN;

                float moveCost = currentPN.gCost_DistFromStart + DistanceFromHeuristic(currentPN, neighbourPN);
                if (!neighbourOnOpenList || moveCost < neighbourPN.gCost_DistFromStart)
                {
                    // New operation: create/update an PathNode
                    operations++;

                    neighbourPN.gCost_DistFromStart = moveCost;
                    neighbourPN.hCost_DistFromTarget = DistanceFromHeuristic(neighbourPN, targetPN);
                    neighbourPN.previous = currentPN;
                    if (!neighbourOnOpenList) openList.Add(neighbourPN);
                }
            }
        }
    }

    private static void MakePath(PathNode startNode, PathNode targetNode, out List<PathNode> result, out float pathCost)
    {
        result = new List<PathNode>();
        pathCost = 0;
        PathNode currentNode = targetNode;
        while (currentNode != startNode)
        {
            result.Add(currentNode);
            pathCost += DistanceFromHeuristic(currentNode, currentNode.previous);
            currentNode = currentNode.previous;
        }
        result.Reverse();
    }

    private static PathNode ListContainsNodeId(IList<PathNode> list, Vector2Int id)
    {
        foreach (PathNode item in list)
        {
            if (item.tile.id == id)
                return item;
        }
        return null;
    }

    private static float DistanceFromHeuristic(PathNode from, PathNode to)
    {
        Vector2Int fromId = from.tile.id;
        Vector2Int toId = to.tile.id;
        int distX = Mathf.Abs(fromId.x - toId.x);
        int distY = Mathf.Abs(fromId.y - toId.y);
        bool isDiagonal = (distX != 0 && distY != 0);

        int fromCost = from.tile.groundMovementCost;
        int toCost = to.tile.groundMovementCost;
        float combinedCosts = (fromCost + toCost) / 2F;

        float result = distX + distY;
        result *= combinedCosts;
        if (isDiagonal) result *= DIAGONAL_MODIFIER;
        return result;
    }

    //private static float DistanceFromHeuristic(PathNode from, PathNode to)
    //{
    //    int distX = Mathf.Abs(from.tile.id.x - to.tile.id.x);
    //    int distY = Mathf.Abs(from.tile.id.y - to.tile.id.y);
    //    float result = distX + distY;
    //    if (distX != 0 && distY != 0) result *= 0.7F;
    //    return result;
    //}
}
