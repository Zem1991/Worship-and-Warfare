using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZemDirections;

public static class InputHelper
{
    public static List<InputHighlight> MakeMoveAreaHighlights(AbstractPiece2 piece, PieceMovement2 pieceMovement, 
        Func<PathNode, PathNode, float> heuristic, bool needGroundAccess, bool needWaterAccess, bool needLavaAccess,
        Transform transform, Sprite moveAreaSprite)
    {
        List<InputHighlight> movementHighlights = new List<InputHighlight>();

        InputManager im = InputManager.Instance;
        InputHighlight prefabHighlight = AllPrefabs.Instance.inputHighlight;

        List<AbstractTile> movementArea = Pathfinder.GetMovementArea(piece.currentTile, pieceMovement.movementPointsCurrent,
            heuristic, needGroundAccess, needWaterAccess, needLavaAccess);

        foreach (var item in movementArea)
        {
            Vector3 pos = item.transform.position;
            Quaternion rot = Quaternion.identity;

            InputHighlight highlight = UnityEngine.Object.Instantiate(prefabHighlight, pos, rot, transform);
            movementHighlights.Add(highlight);
            highlight.name = "Move area: " + item.posId;
            highlight.ChangeSprite(moveAreaSprite, im.highlightDefault, SpriteOrderConstants.HIGHLIGHT_AREA);
        }

        return movementHighlights;
    }

    public static List<InputHighlight> MakeMovePathHighlights(AbstractPiece2 piece, PieceMovement2 pieceMovement, Transform transform, Sprite[] movementArrowSprites, Sprite[] movementMarkerSprites)
    {
        List<InputHighlight> movementHighlights = new List<InputHighlight>();

        List<PathNode> path = pieceMovement.GetPath();
        if (path != null)
        {
            InputManager im = InputManager.Instance;
            InputHighlight prefabHighlight = AllPrefabs.Instance.inputHighlight;

            movementHighlights = new List<InputHighlight>();
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
                moveColor = moveCost > movePoints ? im.highlightDenied : im.highlightAllowed;

                Vector3 fromPos = currentTile.transform.position;
                Vector3 toPos = nextTile.transform.position;

                Vector3 pos = Vector3.Lerp(fromPos, toPos, 0.5F);
                Quaternion rot = Quaternion.identity;
                OctoDirXZ dir = currentTile.GetNeighbourDirection(nextTile);

                nextI++;

                InputHighlight step = UnityEngine.Object.Instantiate(prefabHighlight, pos, rot, transform);
                movementHighlights.Add(step);
                step.name = "Step #" + nextI;
                step.ChangeSprite(movementArrowSprites[(int)dir], moveColor, SpriteOrderConstants.HIGHLIGHT_PATH);

                InputHighlight marker = UnityEngine.Object.Instantiate(prefabHighlight, toPos, rot, transform);
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
