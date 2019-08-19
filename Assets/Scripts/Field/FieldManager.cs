using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : AbstractSingleton<FieldManager>, IShowableHideable
{
    [Header("Prefabs")]
    public FieldTile prefabTile;
    public FieldPiece prefabPiece;

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
        FieldUI.Instance.EscapeMenuHide();

        mapHandler.BuildMap(scenarioSize, map);
        pieceHandler.Create(pieces);
    }

    public void RemovePiece(FieldPiece piece)
    {
        pieceHandler.RemovePiece(piece);
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

    public void EscapeMenu()
    {
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
}
