using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZemDirections;

public static class InputHelper
{
    public static List<InputHighlight> MakeMovementHighlights(AbstractPiece2 piece, PieceMovement2 pieceMovement, Transform transform, Sprite[] movementArrowSprites, Sprite[] movementMarkerSprites)
    {
        List<InputHighlight> movementHighlights = null;

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

                InputHighlight step = Object.Instantiate(prefabHighlight, pos, rot, transform);
                movementHighlights.Add(step);
                step.name = "Step #" + nextI;
                step.ChangeSprite(movementArrowSprites[(int)dir], moveColor);

                InputHighlight marker = Object.Instantiate(prefabHighlight, toPos, rot, transform);
                movementHighlights.Add(marker);
                if (nextI == totalNodes)
                {
                    marker.name = "Final Marker";
                    marker.ChangeSprite(movementMarkerSprites[1], moveColor);
                }
                else
                {
                    marker.name = "Marker #" + nextI;
                    marker.ChangeSprite(movementMarkerSprites[0], moveColor);
                }
            }
        }

        return movementHighlights;
    }
}
