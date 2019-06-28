using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Pathfinder
{
    public static void FindPath(Tile startTile, Tile targetTile, out List<PathNode> result, out float size)
    {
        result = null;
        size = 0;
        float operations = 0;

        PathNode startPN = new PathNode(startTile);
        PathNode targetPN = new PathNode(targetTile);

        HashSet<Vector2Int> closedList = new HashSet<Vector2Int>();
        List<PathNode> openList = new List<PathNode>();
        openList.Add(startPN);
        while (openList.Count > 0)
        {
            //New operation: process an PathNode
            operations++;

            //Get the PathNode with the lowest totalDistance/fCost
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

            //Make path if the target node was found
            if (currentPN.tile.id == targetTile.id)
            {
                MakePath(startPN, currentPN, out result, out size);
                break;
            }

            //Identify and process accessible neighbouring nodes
            foreach (Tile neighbour in currentPN.tile.GetNeighbours())
            {
                //Neighbour node cannot be on the closed set
                PathNode neighbourPN = new PathNode(neighbour);
                if (closedList.Contains(neighbour.id)) continue;

                //Switch to existing node if possible
                PathNode existingPN = ListContainsNodeId(openList, neighbour.id);
                bool neighbourOnOpenList = (existingPN != null);
                if (neighbourOnOpenList) neighbourPN = existingPN;

                float moveCost = currentPN.gCost_DistFromStart + DistanceFromHeuristic(currentPN, neighbourPN);
                if (!neighbourOnOpenList || moveCost < neighbourPN.gCost_DistFromStart)
                {
                    //New operation: create/update an PathNode
                    operations++;

                    neighbourPN.gCost_DistFromStart = moveCost;
                    neighbourPN.hCost_DistFromTarget = DistanceFromHeuristic(neighbourPN, targetPN);
                    neighbourPN.previous = currentPN;
                    if (!neighbourOnOpenList) openList.Add(neighbourPN);
                }
            }
        }
    }

    private static void MakePath(PathNode startNode, PathNode targetNode, out List<PathNode> result, out float size)
    {
        result = new List<PathNode>();
        size = 0;
        PathNode currentNode = targetNode;
        while (currentNode != startNode)
        {
            result.Add(currentNode);
            size += DistanceFromHeuristic(currentNode, currentNode.previous);
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
        int distX = Mathf.Abs(from.tile.id.x - to.tile.id.x);
        int distY = Mathf.Abs(from.tile.id.y - to.tile.id.y);
        float result = distX + distY;
        if (distX != 0 && distY != 0) result *= 0.7F;
        return result;
    }
}
