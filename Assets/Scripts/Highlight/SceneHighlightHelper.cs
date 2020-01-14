using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZemDirections;

public static class SceneHighlightHelper
{
    public static Highlight ObjectHighlight(GameObject gObject, Highlight current, Transform transform, string highlightName, Sprite sprite)
    {
        if (!gObject)
        {
            if (current) UnityEngine.Object.Destroy(current.gameObject);
            return null;
        }

        Vector3 pos = gObject.transform.position;
        Quaternion rot = Quaternion.identity;

        if (!current)
        {
            current = UnityEngine.Object.Instantiate(AllPrefabs.Instance.highlight, pos, rot, transform);
            current.name = highlightName;
            current.ChangeSprite(sprite, HighlightManager.Instance.highlightDefault, SpriteOrderConstants.CURSOR);
        }
        else
        {
            current.transform.position = pos;
        }
        return current;
    }

    public static List<Highlight> MoveAreaHighlights(AbstractTile startTile, Transform transform, Sprite moveAreaSprite,
        int movementPointsCurrent, Func<PathNode, PathNode, float> heuristic, bool needGroundAccess, bool needWaterAccess, bool needLavaAccess)
    {
        Highlight prefabHighlight = AllPrefabs.Instance.highlight;

        List<Highlight> result = new List<Highlight>();
        List<AbstractTile> movementArea = Pathfinder.GetMovementArea(startTile, movementPointsCurrent,
            heuristic, needGroundAccess, needWaterAccess, needLavaAccess);
        foreach (AbstractTile tile in movementArea)
        {
            Vector3 pos = tile.transform.position;
            Quaternion rot = Quaternion.identity;

            Highlight highlight = UnityEngine.Object.Instantiate(prefabHighlight, pos, rot, transform);
            result.Add(highlight);
            highlight.name = "Move area: " + tile.posId;
            highlight.ChangeSprite(moveAreaSprite, HighlightManager.Instance.highlightDefault, SpriteOrderConstants.HIGHLIGHT_MOVE_AREA);
        }
        return result;
    }

    public static List<Highlight> MovePathHighlights(AbstractTile startTile, Transform transform, Sprite[] movementArrowSprites, Sprite[] movementMarkerSprites,
        int movementPointsCurrent, List<PathNode> path)
    {
        HighlightManager hm = HighlightManager.Instance;
        Highlight prefabHighlight = AllPrefabs.Instance.highlight;

        List<Highlight> result = new List<Highlight>();
        if (path != null && path.Count > 0)
        {
            int moveCost = 0;
            Color moveColor;

            int totalNodes = path.Count;
            for (int i = -1; i < totalNodes - 1; i++)
            {
                int nextI = i + 1;

                AbstractTile currentTile = (i == -1 ? startTile : path[i].tile) as AbstractTile;
                AbstractTile nextTile = path[nextI].tile as AbstractTile;

                moveCost += path[nextI].moveCost;
                moveColor = moveCost > movementPointsCurrent ? hm.highlightDenied : hm.highlightAllowed;

                Vector3 fromPos = currentTile.transform.position;
                Vector3 toPos = nextTile.transform.position;

                Vector3 pos = Vector3.Lerp(fromPos, toPos, 0.5F);
                Quaternion rot = Quaternion.identity;
                OctoDirXZ dir = currentTile.GetNeighbourDirection(nextTile);

                nextI++;

                Highlight step = UnityEngine.Object.Instantiate(prefabHighlight, pos, rot, transform);
                result.Add(step);
                step.name = "Move path - Step #" + nextI;
                step.ChangeSprite(movementArrowSprites[(int)dir], moveColor, SpriteOrderConstants.HIGHLIGHT_MOVE_PATH);

                Highlight marker = UnityEngine.Object.Instantiate(prefabHighlight, toPos, rot, transform);
                result.Add(marker);
                if (nextI == totalNodes)
                {
                    marker.name = "Move path - Final Marker";
                    marker.ChangeSprite(movementMarkerSprites[1], moveColor, SpriteOrderConstants.HIGHLIGHT_MOVE_PATH);
                }
                else
                {
                    marker.name = "Move path - Marker #" + nextI;
                    marker.ChangeSprite(movementMarkerSprites[0], moveColor, SpriteOrderConstants.HIGHLIGHT_MOVE_PATH);
                }
            }
        }
        return result;
    }

    public static List<Highlight> TargetAreaHighlights(List<AbstractTile> tiles, Transform transform, Sprite targetAreaSprite)
    {
        Highlight prefabHighlight = AllPrefabs.Instance.highlight;

        List<Highlight> result = new List<Highlight>();
        foreach (AbstractTile tile in tiles)
        {
            Vector3 pos = tile.transform.position;
            Quaternion rot = Quaternion.identity;

            Highlight highlight = UnityEngine.Object.Instantiate(prefabHighlight, pos, rot, transform);
            result.Add(highlight);
            highlight.name = "Target area: " + tile.posId;
            highlight.ChangeSprite(targetAreaSprite, HighlightManager.Instance.highlightDenied, SpriteOrderConstants.HIGHLIGHT_TARGET_AREA);
        }
        return result;
    }
}
