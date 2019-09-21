using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Pathfinder
{
    public const float DIAGONAL_MODIFIER_OCTO = 0.71F;
    public const float DIAGONAL_MODIFIER_HEX = 0.87F;

    public static void FindPath(AbstractTile startTile, AbstractTile targetTile, Func<PathNode, PathNode, float> heuristic,
        bool needGroundAccess, bool needWaterAccess, bool needLavaAccess,
        out List<PathNode> result, out float pathCost)
    {
        result = null;
        pathCost = 0;
        float operations = 0;
        if (!targetTile.occupantPiece && !targetTile.IsAcessible(needGroundAccess, needWaterAccess, needLavaAccess)) return;

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
            closedList.Add(currentPN.tile.posId);

            // Make path if the target node was found
            if (currentPN.tile.id == targetTile.id)
            {
                MakePath(startPN, currentPN, heuristic, out result, out pathCost);
                return;
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

                float moveCost = currentPN.gCost_DistFromStart + heuristic(currentPN, neighbourPN);
                if (!neighbourOnOpenList || moveCost < neighbourPN.gCost_DistFromStart)
                {
                    // New operation: create/update an PathNode
                    operations++;

                    neighbourPN.gCost_DistFromStart = moveCost;
                    neighbourPN.hCost_DistFromTarget = heuristic(neighbourPN, targetPN);
                    neighbourPN.previous = currentPN;
                    if (!neighbourOnOpenList) openList.Add(neighbourPN);
                }
            }
        }
    }

    public static float OctoHeuristic(PathNode from, PathNode to)
    {
        Vector2Int fromId = from.tile.posId;
        Vector2Int toId = to.tile.posId;
        int distX = Mathf.Abs(fromId.x - toId.x);
        int distY = Mathf.Abs(fromId.y - toId.y);
        bool isDiagonal = (distX != 0 && distY != 0);

        int fromCost = from.tile.groundMovementCost;
        int toCost = to.tile.groundMovementCost;
        float combinedCosts = (fromCost + toCost) / 2F;

        float result = distX + distY;
        result *= combinedCosts;
        if (isDiagonal) result *= DIAGONAL_MODIFIER_OCTO;
        return result;
    }

    public static float HexHeuristic(PathNode from, PathNode to)
    {
        Vector2Int fromId = from.tile.posId;
        Vector2Int toId = to.tile.posId;
        int distX = Mathf.Abs(fromId.x - toId.x);
        int distY = Mathf.Abs(fromId.y - toId.y);
        bool isDiagonal = (distX != 0 && distY != 0);

        int fromCost = from.tile.groundMovementCost;
        int toCost = to.tile.groundMovementCost;
        float combinedCosts = (fromCost + toCost) / 2F;

        float result = distX + distY;
        result *= combinedCosts;
        if (isDiagonal) result *= DIAGONAL_MODIFIER_HEX;
        return result;

        //int fromCost = from.tile.groundMovementCost;
        //int toCost = to.tile.groundMovementCost;
        //float result = (fromCost + toCost) / 2F;
        //return result;
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

    private static float DistanceCalcs(PathNode from, PathNode to, Func<PathNode, PathNode, float> heuristic)
    {
        return heuristic(from, to);
    }

    private static void MakePath(PathNode startNode, PathNode targetNode, Func<PathNode, PathNode, float> heuristic,
        out List<PathNode> result, out float pathCost)
    {
        result = new List<PathNode>();
        pathCost = 0;
        PathNode currentNode = targetNode;
        while (currentNode != startNode)
        {
            result.Add(currentNode);
            pathCost += heuristic(currentNode, currentNode.previous);
            currentNode = currentNode.previous;
        }
        result.Reverse();
    }
}
