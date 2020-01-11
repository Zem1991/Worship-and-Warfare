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

    public static List<Highlight> MoveAreaHighlights(AbstractPiece2 piece, Transform transform, Sprite sprite,
        int movementPointsCurrent, Func<PathNode, PathNode, float> heuristic, bool needGroundAccess, bool needWaterAccess, bool needLavaAccess)
    {
        Highlight prefabHighlight = AllPrefabs.Instance.highlight;

        List<Highlight> result = new List<Highlight>();

        List<AbstractTile> movementArea = Pathfinder.GetMovementArea(piece.currentTile, movementPointsCurrent,
            heuristic, needGroundAccess, needWaterAccess, needLavaAccess);
        foreach (var item in movementArea)
        {
            Vector3 pos = item.transform.position;
            Quaternion rot = Quaternion.identity;

            Highlight highlight = UnityEngine.Object.Instantiate(prefabHighlight, pos, rot, transform);
            result.Add(highlight);
            highlight.name = "Move area: " + item.posId;
            highlight.ChangeSprite(sprite, HighlightManager.Instance.highlightDefault, SpriteOrderConstants.HIGHLIGHT_AREA);
        }

        return result;
    }

    public static List<Highlight> MovePathHighlights(AbstractPiece2 piece, Transform transform, Sprite[] movementArrowSprites, Sprite[] movementMarkerSprites,
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

                AbstractTile currentTile = (i == -1 ? piece.currentTile : path[i].tile) as AbstractTile;
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
                step.name = "Step #" + nextI;
                step.ChangeSprite(movementArrowSprites[(int)dir], moveColor, SpriteOrderConstants.HIGHLIGHT_PATH);

                Highlight marker = UnityEngine.Object.Instantiate(prefabHighlight, toPos, rot, transform);
                result.Add(marker);
                if (nextI == totalNodes)
                {
                    marker.name = "Final Marker";
                    marker.ChangeSprite(movementMarkerSprites[1], moveColor, SpriteOrderConstants.HIGHLIGHT_PATH);
                }
                else
                {
                    marker.name = "Marker #" + nextI;
                    marker.ChangeSprite(movementMarkerSprites[0], moveColor, SpriteOrderConstants.HIGHLIGHT_PATH);
                }
            }
        }

        return result;
    }
}
