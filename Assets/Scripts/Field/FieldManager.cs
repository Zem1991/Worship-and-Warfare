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

    public void BootField(Vector2Int scenarioSize, MapData map, PieceData[] pieces)
    {
        mapHandler.BuildMap(scenarioSize, map);
        pieceHandler.CreatePieces(pieces);
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
}
