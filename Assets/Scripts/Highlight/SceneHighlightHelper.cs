using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZemDirections;

public static class SceneHighlightHelper
{
    public static Highlight ObjectHighlight(GameObject gObject, Highlight highlight, Transform transform, string highlightName, Sprite sprite)
    {
        if (!gObject)
        {
            if (highlight) UnityEngine.Object.Destroy(highlight.gameObject);
            return null;
        }
        else
        {
            Vector3 pos = gObject.transform.position;
            Quaternion rot = Quaternion.identity;

            if (!highlight)
            {
                highlight = UnityEngine.Object.Instantiate(AllPrefabs.Instance.highlight, pos, rot, transform);
                highlight.name = highlightName;
                highlight.ChangeSprite(sprite, HighlightManager.Instance.highlightDefault, SpriteOrderConstants.CURSOR);
            }
            else
            {
                highlight.transform.position = pos;
            }
            return highlight;
        }
    }

    public static List<Highlight> MoveAreaHighlight(GameObject gObject, PieceMovement2 pieceMovement, Transform transform, string highlightName, Sprite sprite,
        Func<PathNode, PathNode, float> heuristic, bool needGroundAccess, bool needWaterAccess, bool needLavaAccess)
    {

    }

    //  //  //  //  //

    public static List<Highlight> MakeMoveAreaHighlights(AbstractPiece2 piece, PieceMovement2 pieceMovement, 
        Func<PathNode, PathNode, float> heuristic, bool needGroundAccess, bool needWaterAccess, bool needLavaAccess,
        Transform transform, Sprite moveAreaSprite)
    {
        List<Highlight> movementHighlights = new List<Highlight>();

        Highlight prefabHighlight = AllPrefabs.Instance.highlight;

        List<AbstractTile> movementArea = Pathfinder.GetMovementArea(piece.currentTile, pieceMovement.movementPointsCurrent,
            heuristic, needGroundAccess, needWaterAccess, needLavaAccess);

        foreach (var item in movementArea)
        {
            Vector3 pos = item.transform.position;
            Quaternion rot = Quaternion.identity;

            Highlight highlight = UnityEngine.Object.Instantiate(prefabHighlight, pos, rot, transform);
            movementHighlights.Add(highlight);
            highlight.name = "Move area: " + item.posId;
            highlight.ChangeSprite(moveAreaSprite, HighlightManager.Instance.highlightDefault, SpriteOrderConstants.HIGHLIGHT_AREA);
        }

        return movementHighlights;
    }

    public static List<Highlight> MakeMovePathHighlights(AbstractPiece2 piece, PieceMovement2 pieceMovement, Transform transform, Sprite[] movementArrowSprites, Sprite[] movementMarkerSprites)
    {
        List<Highlight> movementHighlights = new List<Highlight>();

        List<PathNode> path = pieceMovement.GetPath();
        if (path != null)
        {
            HighlightManager hm = HighlightManager.Instance;
            Highlight prefabHighlight = AllPrefabs.Instance.highlight;

            movementHighlights = new List<Highlight>();
            int movePoints = pieceMovement.movementPointsCurrent;
            int moveCost = 0;
            Color moveColor;

            int totalNodes = path.Count;
            for (int i = -1; i < totalNodes - 1; i++)
            {
                int nextI = i + 1;

                AbstractTile currentTile = (i == -1 ? piece.currentTile : path[i].tile) as AbstractTile;
                AbstractTile nextTile = path[nextI].tile as AbstractTile;

                moveCost += path[nextI].moveCost;
                moveColor = moveCost > movePoints ? hm.highlightDenied : hm.highlightAllowed;

                Vector3 fromPos = currentTile.transform.position;
                Vector3 toPos = nextTile.transform.position;

                Vector3 pos = Vector3.Lerp(fromPos, toPos, 0.5F);
                Quaternion rot = Quaternion.identity;
                OctoDirXZ dir = currentTile.GetNeighbourDirection(nextTile);

                nextI++;

                Highlight step = UnityEngine.Object.Instantiate(prefabHighlight, pos, rot, transform);
                movementHighlights.Add(step);
                step.name = "Step #" + nextI;
                step.ChangeSprite(movementArrowSprites[(int)dir], moveColor, SpriteOrderConstants.HIGHLIGHT_PATH);

                Highlight marker = UnityEngine.Object.Instantiate(prefabHighlight, toPos, rot, transform);
                movementHighlights.Add(marker);
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

        return movementHighlights;
    }
}
