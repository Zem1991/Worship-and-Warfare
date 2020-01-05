using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSceneHighlights : AbstractSingleton<CombatSceneHighlights>, IShowableHideable
{
    [Header("Sprites (editor set)")]
    public Sprite cursorSprite;
    public Sprite selectionSprite;
    public Sprite moveAreaSprite;
    public Sprite[] movePathArrowSprites = new Sprite[8];
    public Sprite[] movePathMarkerSprites = new Sprite[2];

    [Header("Highlighted pieces")]
    [SerializeField] private CombatTile cursorTile;
    [SerializeField] private AbstractCombatPiece2 selectionPiece;
    [SerializeField] private AbstractCombatantPiece2 movePiece;
    [SerializeField] private bool movePieceMoving;

    [Header("Highlights")]
    public Highlight cursorHighlight;
    public Highlight selectionHighlight;
    public List<Highlight> moveAreaHighlights = new List<Highlight>();
    public List<Highlight> movePathHighlights = new List<Highlight>();

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Update()
    {
        CursorHighlight();
        SelectionHighlight();
        MoveHighlights();
    }

    private void CursorHighlight()
    {
        CombatTile tile = CombatSceneInputs.Instance.executor.cursorTile;
        GameObject gObject = tile ? tile.gameObject : null;
        cursorTile = tile;
        cursorHighlight = SceneHighlightHelper.ObjectHighlight(gObject, cursorHighlight, transform, "Cursor", cursorSprite);
    }

    private void SelectionHighlight()
    {
        AbstractCombatPiece2 piece = CombatSceneInputs.Instance.executor.selectionPiece;
        GameObject gObject = piece ? piece.gameObject : null;
        selectionPiece = piece;
        selectionHighlight = SceneHighlightHelper.ObjectHighlight(gObject, selectionHighlight, transform, "Selection", selectionSprite);
    }

    private void MoveHighlights()
    {
        CombatInputExecutor cie = CombatSceneInputs.Instance.executor;
        AbstractCombatantPiece2 actp = cie.selectionPiece as AbstractCombatantPiece2;

        bool currentMoving = movePiece && !movePiece.ICP_IsIdle();
        bool currentNotSelected = movePiece != actp;
        bool selectedIdle = actp && actp.ICP_IsIdle();
        bool selectedCanCommand = cie.canCommandSelectedPiece;

        bool conditionsToRemove = (movePieceMoving && !currentMoving) || (!movePieceMoving && currentMoving) || currentNotSelected;
        bool conditionsToCreate = selectedIdle && selectedCanCommand;

        movePiece = actp;
        if (conditionsToRemove) RemoveMoveHighlights();
        if (conditionsToRemove && conditionsToCreate) CreateMoveHighlights(actp);
        movePieceMoving = movePiece && !movePiece.ICP_IsIdle();
    }

    private void RemoveMoveHighlights()
    {
        Debug.Log("CombatSceneHighlights - RemoveMoveHighlights()");
        foreach (var item in moveAreaHighlights) Destroy(item.gameObject);
        moveAreaHighlights.Clear();
        foreach (var item in movePathHighlights) Destroy(item.gameObject);
        movePathHighlights.Clear();
    }

    private void CreateMoveHighlights(AbstractCombatantPiece2 actp)
    {
        Debug.Log("CombatSceneHighlights - CreateMoveHighlights()");
        PieceMovement2 pm2 = actp.pieceMovement;
        moveAreaHighlights = SceneHighlightHelper.MoveAreaHighlights(actp, transform, moveAreaSprite,
            pm2.movementPointsCurrent, Pathfinder.OctoHeuristic, true, false, false);
        movePathHighlights = SceneHighlightHelper.MovePathHighlights(actp, transform, movePathArrowSprites, movePathMarkerSprites,
            pm2.movementPointsCurrent, pm2.GetPath());
    }

    public void Refresh()
    {
        cursorTile = null;
        selectionPiece = null;
        movePiece = null;
        movePieceMoving = false;
        Update();
    }
}
