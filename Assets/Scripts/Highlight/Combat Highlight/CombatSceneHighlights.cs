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

    [Header("Object highlights")]
    public Highlight cursorHighlight;
    public Highlight selectionHighlight;

    [Header("Movement highlights")]
    public List<Highlight> moveAreaHighlights = new List<Highlight>();
    public List<Highlight> movePathHighlights = new List<Highlight>();

    [Header("Specific update calls")]
    public bool showMoveArea;
    public bool hideMoveArea;
    public bool showMovePath;
    public bool hideMovePath;

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
        MoveAreaHighlight();
        MovePathHighlight();
    }

    private void CursorHighlight()
    {
        CombatTile tile = CombatSceneInputs.Instance.executor.cursorTile;
        GameObject gObject = tile ? tile.gameObject : null;
        cursorHighlight = SceneHighlightHelper.ObjectHighlight(gObject, cursorHighlight, transform, "Cursor", cursorSprite);
    }

    private void SelectionHighlight()
    {
        AbstractCombatPiece2 piece = CombatSceneInputs.Instance.executor.selectionPiece;
        GameObject gObject = piece ? piece.gameObject : null;
        selectionHighlight = SceneHighlightHelper.ObjectHighlight(gObject, selectionHighlight, transform, "Selection", selectionSprite);
    }

    private void MoveAreaHighlight()
    {
        if (!showMoveArea && !hideMoveArea) return;

        if (moveAreaHighlights != null)
        {
            foreach (var item in moveAreaHighlights) Destroy(item.gameObject);
            moveAreaHighlights = null;
        }

        if (showMoveArea)
        {
            FieldInputExecutor fie = FieldSceneInputs.Instance.executor;
            PartyPiece2 pp = fie.selectionPiece as PartyPiece2;

            bool dontCreateHighlights = false;
            if (!fie.canCommandSelectedPiece) dontCreateHighlights = true;
            if (!pp || !pp.ICP_IsIdle()) dontCreateHighlights = true;

            if (dontCreateHighlights) return;

            moveAreaHighlights = SceneHighlightHelper.MakeMoveAreaHighlights(pp, pp.pieceMovement,
                Pathfinder.OctoHeuristic, true, false, false,
                transform, moveAreaSprite);
        }

        showMoveArea = false;
        hideMoveArea = false;
    }

    private void MovePathHighlight()
    {
        if (!showMovePath && !hideMovePath) return;

        if (movePathHighlights != null)
        {
            foreach (var item in movePathHighlights) Destroy(item.gameObject);
            movePathHighlights.Clear();
        }

        if (showMovePath)
        {
            FieldInputExecutor fie = FieldSceneInputs.Instance.executor;
            PartyPiece2 pp = fie.selectionPiece as PartyPiece2;

            bool dontCreateHighlights = false;
            if (!fie.canCommandSelectedPiece) dontCreateHighlights = true;
            if (!pp || !pp.ICP_IsIdle()) dontCreateHighlights = true;

            if (dontCreateHighlights) return;

            movePathHighlights = SceneHighlightHelper.MakeMovePathHighlights(pp, pp.pieceMovement, transform, movePathArrowSprites, movePathMarkerSprites);
        }

        showMovePath = false;
        hideMovePath = false;
    }
}
