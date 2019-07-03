﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZemDirections;

public class InputManager : MonoBehaviour
{
    public static InputManager Singleton;
    private InputRecorder recorder;

    [Header("Prefabs and Sprites")]
    public InputHighlight prefabHighlight;
    public Sprite cursorSprite;
    public Sprite selectionSprite;
    public Sprite[] movementArrowSprites = new Sprite[8];
    public Sprite[] movementMarkerSprites = new Sprite[2];

    [Header("Mouse Position")]
    public Vector3 mouseScreenPos;
    public Vector3 mouseWorldPos;
    public Vector3 mouseAfterBorders;
    public bool cursorOnPlayArea;

    [Header("Cursor Data")]
    public InputHighlight cursorHighlight;
    public Vector2Int cursorPos;
    public Tile cursorTile;
    public Piece cursorPiece;

    [Header("Selection Data")]
    public InputHighlight selectionHighlight;
    public Vector2Int selectionPos;
    public Tile selectionTile;
    public Piece selectionPiece;
    public bool canCommandSelectedPiece;

    [Header("Movement Highlights")]
    public bool movementHighlightsUpdateFromCommand;
    public bool movementHighlightsUpdateOnPieceStop;
    public List<InputHighlight> movementHighlights = new List<InputHighlight>();

    void Awake()
    {
        if (Singleton != null)
        {
            Debug.LogWarning("Only one instance of InputManager may exist! Deleting this extra one.");
            Destroy(this);
        }
        else
        {
            Singleton = this;
        }

        cursorHighlight = Instantiate(prefabHighlight, transform);
        cursorHighlight.name = "Cursor Highlight";
        cursorHighlight.ChangeSprite(cursorSprite);

        selectionHighlight = Instantiate(prefabHighlight, transform);
        selectionHighlight.name = "Selection Highlight";
        selectionHighlight.ChangeSprite(selectionSprite);
    }

    // Start is called before the first frame update
    void Start()
    {
        recorder = GetComponent<InputRecorder>();
    }

    // Update is called once per frame
    void Update()
    {
        MousePosition();

        CameraControls();
        CursorHighlight();

        SelectionHighlight();
        SelectionCommand();

        MovementHighlights();
    }

    public Vector3 ScreenToWorld(Vector2 screenPos)
    {
        Vector3 result = Vector3.one * -1;
        Camera cam = GameManager.Singleton.cameraController.camera;
        Ray ray = cam.ScreenPointToRay(screenPos);
        Plane xzPlane = new Plane(Vector3.up, Vector3.zero);
        if (xzPlane.Raycast(ray, out float distance))
        {
            result = ray.GetPoint(distance);
        }
        return result;
    }

    private void MousePosition()
    {
        float width = Screen.width - 1;
        float height = Screen.height - 1;

        mouseScreenPos = Input.mousePosition;
        mouseWorldPos = ScreenToWorld(mouseScreenPos);
        mouseAfterBorders = Vector3.zero;
        if (mouseScreenPos.y < 0)
            mouseAfterBorders.y--;
        if (mouseScreenPos.x < 0)
            mouseAfterBorders.x--;
        if (mouseScreenPos.y > height)
            mouseAfterBorders.y++;
        if (mouseScreenPos.x > width)
            mouseAfterBorders.x++;

        ScenarioManager sm = ScenarioManager.Singleton;
        cursorOnPlayArea = sm.IsWithinBounds(mouseWorldPos);
    }

    private void CameraControls()
    {
        float x = mouseAfterBorders.x + recorder.cameraAxes.x;
        float z = mouseAfterBorders.y + recorder.cameraAxes.z;
        Vector3 direction = new Vector3(x, 0, z);
        GameManager.Singleton.cameraController.MoveCamera(direction);
    }

    private void CursorHighlight()
    {
        cursorPos = Vector2Int.one * -1;
        cursorTile = null;
        cursorPiece = null;

        if (cursorOnPlayArea)
        {
            Vector3 pos = mouseWorldPos;
            pos.x = Mathf.Floor(pos.x);
            pos.z = Mathf.Floor(pos.z);

            cursorPos = new Vector2Int((int)pos.x, (int)pos.z);

            Camera cam = GameManager.Singleton.cameraController.camera;
            Ray ray = cam.ScreenPointToRay(mouseScreenPos);
            RaycastHit[] hits = new RaycastHit[2];
            Physics.RaycastNonAlloc(ray, hits);

            foreach (var item in hits)
            {
                if (item.collider == null) continue;

                Tile t = item.collider.GetComponentInParent<Tile>();
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
        if (recorder.selectionDown)
        {
            movementHighlightsUpdateFromCommand = true;

            if (cursorOnPlayArea)
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
            canCommandSelectedPiece = selectionPiece.owner == PlayerManager.Singleton.localPlayer;
        }
        else
        {
            selectionHighlight.gameObject.SetActive(false);
            canCommandSelectedPiece = false;
        }
    }

    private void SelectionCommand()
    {
        if (recorder.commandDown)
        {
            if (cursorOnPlayArea && selectionPiece)
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
                            PieceManager.Singleton.Pathfind(selectionPiece, cursorTile);
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
        bool condition1 = 
            movementHighlightsUpdateFromCommand;
        bool condition2 = 
            movementHighlightsUpdateOnPieceStop && 
            selectionPiece && 
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
            List<PathNode> path = selectionPiece.path;
            int totalNodes = path.Count;

            movementHighlights = new List<InputHighlight>();

            for (int i = -1; i < totalNodes - 1; i++)
            {
                int nextI = i + 1;

                Tile currentTile = (i == -1 ? selectionPiece.currentTile : path[i].tile);
                Tile nextTile = path[nextI].tile;

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