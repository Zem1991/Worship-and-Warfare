using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldSceneHighlights : AbstractSingleton<FieldSceneHighlights>, IShowableHideable
{
    [Header("Sprites (editor set)")]
    public Sprite cursorSprite;
    public Sprite selectionSprite;
    public Sprite moveAreaSprite;
    public Sprite[] movePathArrowSprites = new Sprite[8];
    public Sprite[] movePathMarkerSprites = new Sprite[2];

    [Header("Highlighted pieces")]
    [SerializeField] private FieldTile cursorTile;
    [SerializeField] private AbstractFieldPiece2 selectionPiece;
    [SerializeField] private PartyPiece2 movePiece;
    [SerializeField] private bool movePiecePathChange;
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
        FieldTile tile = FieldSceneInputs.Instance.executor.cursorTile;
        GameObject gObject = tile ? tile.gameObject : null;
        cursorTile = tile;
        cursorHighlight = SceneHighlightHelper.ObjectHighlight(gObject, cursorHighlight, transform, "Cursor", cursorSprite);
    }

    private void SelectionHighlight()
    {
        AbstractFieldPiece2 piece = FieldSceneInputs.Instance.executor.selectionPiece;
        GameObject gObject = piece ? piece.gameObject : null;
        selectionPiece = piece;
        selectionHighlight = SceneHighlightHelper.ObjectHighlight(gObject, selectionHighlight, transform, "Selection", selectionSprite);
    }

    private void MoveHighlights()
    {
        FieldInputExecutor fie = FieldSceneInputs.Instance.executor;
        PartyPiece2 pp = fie.selectionPiece as PartyPiece2;

        bool currentMoving = movePiece && !movePiece.ICP_IsIdle();
        bool currentNotSelected = movePiece != pp;
        bool selectedIdle = pp && pp.ICP_IsIdle();
        bool selectedCanCommand = fie.canCommandSelectedPiece;

        bool conditionsToRemove = (movePieceMoving && !currentMoving) || (!movePieceMoving && currentMoving) || currentNotSelected;
        bool conditionsToCreate = selectedIdle && selectedCanCommand;

        movePiece = pp;
        if (conditionsToRemove)
        {
            RemoveMoveAreaighlights();
            RemoveMovePathHighlights();

            if (conditionsToCreate)
            {
                CreateMoveAreaHighlights(pp);
                CreateMovePathHighlights(pp);
            }
        }
        else if (movePiecePathChange)
        {
            RemoveMovePathHighlights();
            CreateMovePathHighlights(pp);
            movePiecePathChange = false;
        }
        movePieceMoving = movePiece && !movePiece.ICP_IsIdle();
    }

    private void RemoveMoveAreaighlights()
    {
        Debug.Log("FieldSceneHighlights - RemoveMoveAreaighlights()");
        foreach (var item in moveAreaHighlights) Destroy(item.gameObject);
        moveAreaHighlights.Clear();
    }

    private void RemoveMovePathHighlights()
    {
        Debug.Log("FieldSceneHighlights - RemoveMovePathHighlights()");
        foreach (var item in movePathHighlights) Destroy(item.gameObject);
        movePathHighlights.Clear();
    }

    private void CreateMoveAreaHighlights(PartyPiece2 pp)
    {
        Debug.Log("FieldSceneHighlights - CreateMoveAreaHighlights()");
        PieceMovement2 pm2 = pp.pieceMovement;
        moveAreaHighlights = SceneHighlightHelper.MoveAreaHighlights(pp, transform, moveAreaSprite,
            pm2.movementPointsCurrent, Pathfinder.OctoHeuristic, true, false, false);
    }

    private void CreateMovePathHighlights(PartyPiece2 pp)
    {
        Debug.Log("FieldSceneHighlights - CreateMovePathHighlights()");
        PieceMovement2 pm2 = pp.pieceMovement;
        movePathHighlights = SceneHighlightHelper.MovePathHighlights(pp, transform, movePathArrowSprites, movePathMarkerSprites,
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

    public void PathChange()
    {
        FieldInputExecutor fie = FieldSceneInputs.Instance.executor;
        if (fie.selectionPiece && fie.selectionPiece == movePiece)
        {
            movePiecePathChange = true;
        }
    }
}
