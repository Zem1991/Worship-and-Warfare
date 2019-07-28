﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZemDirections;

public class FieldInputs : Singleton<FieldInputs>, IInputScheme
{
    [Header("Prefabs and Sprites")]
    public InputHighlight prefabHighlight;
    public Sprite cursorSprite;
    public Sprite selectionSprite;
    public Sprite[] movementArrowSprites = new Sprite[8];
    public Sprite[] movementMarkerSprites = new Sprite[2];

    [Header("Cursor Data")]
    public InputHighlight cursorHighlight;
    public Vector2Int cursorPos;
    public FieldTile cursorTile;
    public Piece cursorPiece;

    [Header("Selection Data")]
    public InputHighlight selectionHighlight;
    public Vector2Int selectionPos;
    public FieldTile selectionTile;
    public Piece selectionPiece;
    public bool canCommandSelectedPiece;

    [Header("Movement Highlights")]
    public bool movementHighlightsUpdateFromCommand;
    public bool movementHighlightsUpdateOnPieceStop;
    public List<InputHighlight> movementHighlights = new List<InputHighlight>();

    [Header("Required Objects")]
    public InputManager im;
    public FieldInputRecorder recorder;
    public CameraController cameraController;

    public override void Awake()
    {
        cursorHighlight = Instantiate(prefabHighlight, transform);
        cursorHighlight.name = "Cursor Highlight";
        cursorHighlight.ChangeSprite(cursorSprite);

        selectionHighlight = Instantiate(prefabHighlight, transform);
        selectionHighlight.name = "Selection Highlight";
        selectionHighlight.ChangeSprite(selectionSprite);

        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        im = InputManager.Instance;
        recorder = GetComponent<FieldInputRecorder>();
        cameraController = GetComponentInChildren<CameraController>();
    }

    public CameraController CameraController()
    {
        return cameraController;
    }

    public void UpdateInputs()
    {
        CameraControls();
        CursorHighlight();

        SelectionHighlight();
        SelectionCommand();

        MovementHighlights();
    }

    private void CameraControls()
    {
        float x = im.mouseAfterBorders.x + recorder.cameraAxes.x;
        float z = im.mouseAfterBorders.y + recorder.cameraAxes.z;
        Vector3 direction = new Vector3(x, 0, z);
        cameraController.MoveCamera(direction);
    }

    private void CursorHighlight()
    {
        cursorPos = Vector2Int.one * -1;
        cursorTile = null;
        cursorPiece = null;

        if (im.cursorOnPlayArea)
        {
            Vector3 pos = im.mouseWorldPos;
            pos.x = Mathf.Floor(pos.x);
            pos.z = Mathf.Floor(pos.z);

            cursorPos = new Vector2Int((int)pos.x, (int)pos.z);

            Camera cam = GameManager.Instance.mainCamera;
            Ray ray = cam.ScreenPointToRay(im.mouseScreenPos);
            RaycastHit[] hits = new RaycastHit[2];
            Physics.RaycastNonAlloc(ray, hits);

            foreach (var item in hits)
            {
                if (item.collider == null) continue;

                FieldTile t = item.collider.GetComponentInParent<FieldTile>();
                Piece p = item.collider.GetComponentInParent<Piece>();
                if (cursorTile == null && t) cursorTile = t;
                if (cursorPiece == null && p) cursorPiece = p;
            }

            cursorHighlight.transform.position = pos;
            cursorHighlight.gameObject.SetActive(true);
        }
        else
        {
            cursorHighlight.gameObject.SetActive(false);
        }
    }

    private void SelectionHighlight()
    {
        if (recorder.selectionDown && 
            !UIManager.Instance.focusedPanel)
        {
            movementHighlightsUpdateFromCommand = true;

            if (im.cursorOnPlayArea)
            {
                selectionPos = cursorPos;
                selectionTile = cursorTile;
                selectionPiece = cursorPiece;

                selectionHighlight.transform.position = cursorHighlight.transform.position;
            }
            else
            {
                selectionPos = Vector2Int.one * -1;
                selectionTile = null;
                selectionPiece = null;
            }
        }

        if (selectionPiece)
        {
            if (selectionPiece.inMovement)
            {
                selectionTile = selectionPiece.currentTile;
                selectionPos = selectionTile.id;

                selectionHighlight.transform.position = selectionPiece.transform.position;
            }

            selectionHighlight.gameObject.SetActive(true);
            canCommandSelectedPiece = selectionPiece.owner == PlayerManager.Instance.localPlayer;
        }
        else
        {
            selectionHighlight.gameObject.SetActive(false);
            canCommandSelectedPiece = false;
        }
    }

    private void SelectionCommand()
    {
        if (recorder.commandDown &&
            !UIManager.Instance.focusedPanel)
        {
            if (im.cursorOnPlayArea && selectionPiece)
            {
                if (canCommandSelectedPiece)
                {
                    movementHighlightsUpdateFromCommand = true;

                    if (selectionPiece.inMovement)
                    {
                        selectionPiece.Stop();
                        movementHighlightsUpdateOnPieceStop = true;
                    }
                    else
                    {
                        if (selectionPiece.HasPath(cursorTile))
                            selectionPiece.Move();
                        else
                            PieceManager.Instance.Pathfind(selectionPiece, cursorTile);
                    }
                }
            }
            else
            {
                canCommandSelectedPiece = false;
            }
        }
    }

    private void MovementHighlights()
    {
        if (!selectionPiece) return;

        List<PathNode> path = selectionPiece.path;
        if (path == null) return;

        bool condition1 =
            movementHighlightsUpdateFromCommand;
        bool condition2 =
            movementHighlightsUpdateOnPieceStop &&
            !selectionPiece.inMovement;
        if (!condition1 && !condition2) return;

        foreach (var item in movementHighlights)
        {
            Destroy(item.gameObject);
        }
        movementHighlights.Clear();

        if (canCommandSelectedPiece &&
            !selectionPiece.inMovement)
        {
            movementHighlights = new List<InputHighlight>();
            int totalNodes = path.Count;
            for (int i = -1; i < totalNodes - 1; i++)
            {
                int nextI = i + 1;

                FieldTile currentTile = (i == -1 ? selectionPiece.currentTile : path[i].tile);
                FieldTile nextTile = path[nextI].tile;

                Vector3 fromPos = currentTile.transform.position;
                Vector3 toPos = nextTile.transform.position;

                Vector3 pos = Vector3.Lerp(fromPos, toPos, 0.5F);
                Quaternion rot = Quaternion.identity;
                OctoDirXZ dir = currentTile.GetNeighbourDirection(nextTile);

                nextI++;

                InputHighlight step = Instantiate(prefabHighlight, pos, rot, transform);
                movementHighlights.Add(step);
                step.name = "Step #" + nextI;
                step.ChangeSprite(movementArrowSprites[(int)dir]);

                InputHighlight marker = Instantiate(prefabHighlight, toPos, rot, transform);
                movementHighlights.Add(marker);
                if (nextI == totalNodes)
                {
                    marker.name = "Final Marker";
                    marker.ChangeSprite(movementMarkerSprites[1]);
                }
                else
                {
                    marker.name = "Marker #" + nextI;
                    marker.ChangeSprite(movementMarkerSprites[0]);
                }
            }
        }

        movementHighlightsUpdateFromCommand = false;
    }
}