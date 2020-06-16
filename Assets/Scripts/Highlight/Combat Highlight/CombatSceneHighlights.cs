using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSceneHighlights : AbstractSingleton<CombatSceneHighlights>, IShowableHideable
{
    [Header("Highlight holders (editor set)")]
    public Transform cursorAndSelectionHolder;
    public Transform moveAreaHolder;
    public Transform movePathHolder;
    public Transform targetAreaHolder;

    [Header("Sprites (editor set)")]
    public Sprite cursorSprite;
    public Sprite selectionSprite;
    public Sprite moveAreaSprite;
    public Sprite[] movePathArrowSprites = new Sprite[8];
    public Sprite[] movePathMarkerSprites = new Sprite[2];
    public Sprite targetAreaSprite;

    [Header("Highlighted pieces")]
    [SerializeField] private CombatTile cursorTile;
    [SerializeField] private AbstractCombatPiece3 selectionPiece;
    [SerializeField] private CombatantPiece3 movePiece;
    [SerializeField] private CombatantPiece3 targetPiece;
    [SerializeField] private bool movePiecePathChange;
    [SerializeField] private bool movePieceMoving;

    [Header("Highlights")]
    public Highlight cursorHighlight;
    public Highlight selectionHighlight;
    public List<Highlight> moveAreaHighlights = new List<Highlight>();
    public List<Highlight> movePathHighlights = new List<Highlight>();
    public List<Highlight> targetAreaHighlights = new List<Highlight>();

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
        ActionHighlights();
    }

    private void CursorHighlight()
    {
        CombatTile tile = CombatSceneInputs.Instance.executor.cursorTile;
        GameObject gObject = tile ? tile.gameObject : null;
        cursorTile = tile;
        cursorHighlight = SceneHighlightHelper.ObjectHighlight(gObject, cursorHighlight, cursorAndSelectionHolder, "Cursor", cursorSprite);
    }

    private void SelectionHighlight()
    {
        AbstractCombatPiece3 piece = CombatSceneInputs.Instance.executor.selectionPiece;
        GameObject gObject = piece ? piece.gameObject : null;
        selectionPiece = piece;
        selectionHighlight = SceneHighlightHelper.ObjectHighlight(gObject, selectionHighlight, cursorAndSelectionHolder, "Selection", selectionSprite);
    }

    private void MoveHighlights()
    {
        CombatInputExecutor cie = CombatSceneInputs.Instance.executor;
        UnitPiece3 actp = cie.selectionPiece as UnitPiece3;

        bool currentMoving = movePiece && !movePiece.ICP_IsIdle();
        bool currentNotSelected = movePiece != actp;
        bool selectedIdle = actp && actp.ICP_IsIdle();
        bool selectedCanCommand = cie.canCommandSelectedPiece;

        bool conditionsToRemove = (movePieceMoving && !currentMoving) || (!movePieceMoving && currentMoving) || currentNotSelected;
        bool conditionsToCreate = selectedIdle && selectedCanCommand;
        bool conditionsToRepath = movePiecePathChange && selectedIdle;

        movePiece = actp;
        if (conditionsToRemove)
        {
            RemoveMoveAreaHighlights();
            RemoveMovePathHighlights();

            if (conditionsToCreate)
            {
                CreateMoveAreaHighlights(actp);
                CreateMovePathHighlights(actp);
            }
        }
        else if (conditionsToRepath)
        {
            RemoveMovePathHighlights();
            CreateMovePathHighlights(actp);
        }
        movePiecePathChange = false;
        movePieceMoving = movePiece && !movePiece.ICP_IsIdle();
    }

    private void RemoveMoveAreaHighlights()
    {
        //Debug.Log("CombatSceneHighlights - RemoveMoveAreaHighlights()");
        foreach (var item in moveAreaHighlights) Destroy(item.gameObject);
        moveAreaHighlights.Clear();
    }

    private void RemoveMovePathHighlights()
    {
        //Debug.Log("CombatSceneHighlights - RemoveMovePathHighlights()");
        foreach (var item in movePathHighlights) Destroy(item.gameObject);
        movePathHighlights.Clear();
    }

    private void CreateMoveAreaHighlights(UnitPiece3 actp)
    {
        //Debug.Log("CombatSceneHighlights - CreateMoveAreaHighlights()");
        PieceMovement3 pm2 = actp.pieceMovement;
        moveAreaHighlights = SceneHighlightHelper.MoveAreaHighlights(actp.currentTile, moveAreaHolder, moveAreaSprite,
            pm2.movementPointsCurrent, Pathfinder.OctoHeuristic, true, false, false);
    }

    private void CreateMovePathHighlights(UnitPiece3 actp)
    {
        //Debug.Log("CombatSceneHighlights - CreateMovePathHighlights()");
        PieceMovement3 pm2 = actp.pieceMovement;
        movePathHighlights = SceneHighlightHelper.MovePathHighlights(actp.currentTile, movePathHolder, movePathArrowSprites, movePathMarkerSprites,
            pm2.movementPointsCurrent, pm2.GetPath());
    }

    private void ActionHighlights()
    {
        CombatInputExecutor cie = CombatSceneInputs.Instance.executor;
        UnitPiece3 actp = cie.selectionPiece as UnitPiece3;
        if (!actp) return;

        UnitPiece3 target = actp.targetPiece as UnitPiece3;

        bool targetChanged = target != targetPiece;

        bool conditionsToRemove = targetChanged || !target;
        bool conditionsToCreate = targetChanged && target;
        targetPiece = target;

        if (conditionsToRemove)
        {
            foreach (var item in targetAreaHighlights) Destroy(item.gameObject);
            targetAreaHighlights.Clear();

            if (conditionsToCreate)
            {
                CombatMap map = CombatManager.Instance.mapHandler.map;
                List<CombatTile> combatTiles = map.AreaLine(actp.currentTile as CombatTile, target.currentTile as CombatTile);
                //List<CombatTile> combatTiles = map.AreaLine(actp.currentTile.transform.position, target.currentTile.transform.position);

                List<AbstractTile> abstractTiles = new List<AbstractTile>();
                foreach (var item in combatTiles)
                {
                    abstractTiles.Add(item as AbstractTile);
                }
                targetAreaHighlights = SceneHighlightHelper.TargetAreaHighlights(abstractTiles, targetAreaHolder, targetAreaSprite);
            }
        }
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
        CombatInputExecutor cie = CombatSceneInputs.Instance.executor;
        if (cie.selectionPiece && cie.selectionPiece == movePiece)
        {
            movePiecePathChange = true;
        }
    }
}
