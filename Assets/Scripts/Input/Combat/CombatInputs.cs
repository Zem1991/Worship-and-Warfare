﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZemDirections;

public class CombatInputs : AbstractSingleton<CombatInputs>, IInputScheme
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
    public FieldPiece cursorPiece;

    [Header("Selection Data")]
    public InputHighlight selectionHighlight;
    public Vector2Int selectionPos;
    public FieldTile selectionTile;
    public FieldPiece selectionPiece;
    public bool canCommandSelectedPiece;

    [Header("Movement Highlights")]
    public bool movementHighlightsUpdateFromCommand;
    public bool movementHighlightsUpdateOnPieceStop;
    public List<InputHighlight> movementHighlights = new List<InputHighlight>();

    [Header("Required Objects")]
    public InputManager im;
    public CombatInputRecorder recorder;
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
        recorder = GetComponent<CombatInputRecorder>();
        cameraController = GetComponentInChildren<CameraController>();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public CameraController CameraController()
    {
        return cameraController;
    }

    public void UpdateInputs()
    {
        //CameraControls();
        //CursorHighlight();

        //SelectionHighlight();
        //SelectionCommand();
    }
}
