using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : AbstractSingleton<FieldManager>, IShowableHideable
{
    [Header("Auxiliary Objects")]
    public FieldMapHandler mapHandler;
    public FieldPieceHandler pieceHandler;

    public void TerminateField()
    {
        mapHandler.ClearMap();
        pieceHandler.Remove();
    }

    public void BootField(Vector2Int scenarioSize, MapData map, PieceData[] pieces)
    {
        mapHandler.BuildMap(scenarioSize, map);
        pieceHandler.Create(pieces);
    }

    public void RemovePiece(FieldPiece piece)
    {
        pieceHandler.RemovePiece(piece);
    }

    public void NextTurnForAll()
    {
        foreach (var item in pieceHandler.pieces)
        {
            item.StartTurn();
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        mapHandler.gameObject.SetActive(false);
        pieceHandler.gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        mapHandler.gameObject.SetActive(true);
        pieceHandler.gameObject.SetActive(true);
    }

    /*
     * Begin: UI Top Left buttons
     */
    public void EscapeMenu()
    {
        bool isPaused = GameManager.Instance.isPaused;
        AUIPanel currentWindow = FieldUI.Instance.currentWindow;
        if (!isPaused && currentWindow)
        {
            FieldUI.Instance.CloseCurrentWindow();
            return;
        }

        bool pauseStatus = GameManager.Instance.PauseUnpause();
        if (pauseStatus) FieldUI.Instance.EscapeMenuShow();
        else FieldUI.Instance.EscapeMenuHide();
    }

    public void Restart()
    {
        GameManager.Instance.PauseUnpause(false);
        FieldUI.Instance.EscapeMenuHide();

        GameManager.Instance.Restart();
    }
    /*
     * End: UI Top Left buttons
     */

    /*
     * Begin: UI Top Right buttons
     */
    public void EndTurnForCurrentPlayer()
    {
        GameManager.Instance.EndTurnForCurrentPlayer();
    }
    /*
     * End: UI Top Right buttons
     */

    /*
     * Begin: UI Bottom Center buttons
     */
    public void MakeSelectedPieceMove()
    {
        FieldInputs.Instance.MakeSelectedPieceInteract(false);
    }

    public void Inventory()
    {
        FieldPiece selectionPiece = FieldInputs.Instance.selectionPiece;
        bool canCommandSelectedPiece = FieldInputs.Instance.canCommandSelectedPiece;

        if (selectionPiece && canCommandSelectedPiece)
        {
            if (FieldUI.Instance.currentWindow == FieldUI.Instance.inventory) FieldUI.Instance.InventoryHide();
            else if (FieldUI.Instance.currentWindow == null) FieldUI.Instance.InventoryShow(selectionPiece);
        }
    }
    /*
    * End: UI Bottom Center buttons
    */
}
