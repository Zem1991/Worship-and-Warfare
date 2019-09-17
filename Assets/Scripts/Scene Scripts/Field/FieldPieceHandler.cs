﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldPieceHandler : MonoBehaviour
{
    public readonly int MAX_UNITS = 5;

    [Header("Pieces")]
    public List<FieldPiece> pieces;

    public void Remove()
    {
        if (pieces != null)
        {
            foreach (var item in pieces)
            {
                Destroy(item.gameObject);
            }
        }
        pieces = new List<FieldPiece>();
    }

    public void Create(PieceData[] pieceData)
    {
        Remove();

        DatabaseManager db = DatabaseManager.Instance;
        DBHandler_Hero dbHeroes = db.heroes;
        DBHandler_Unit dbUnits = db.units;
        PlayerManager pm = PlayerManager.Instance;

        pieces = new List<FieldPiece>();
        FieldManager fm = FieldManager.Instance;

        foreach (var item in pieceData)
        {
            int posX = item.mapPosition[0];
            int posY = item.mapPosition[1];

            Vector3 pos = new Vector3(posX, 0, posY);
            Quaternion rot = Quaternion.identity;

            FieldPiece newPiece = Instantiate(fm.prefabPiece, pos, rot, transform);
            pieces.Add(newPiece);

            newPiece.owner = pm.allPlayers[item.ownerId];

            HeroData hero = item.hero;
            if (hero != null)
            {
                int dbId = hero.heroId;
                DB_Hero dbData = dbHeroes.Select(dbId);
                newPiece.hero = new Hero(dbId, dbData);
            }

            if (item.units != null)
            {
                if (item.units.Length > MAX_UNITS) Debug.LogWarning("There are more units than the piece can store!");
                int totalUnits = Mathf.Min(item.units.Length, MAX_UNITS);

                newPiece.units = new Unit[totalUnits];
                for (int i = 0; i < totalUnits; i++)
                {
                    UnitData unit = item.units[i];
                    int dbId = unit.unitId;
                    DB_Unit dbData = dbUnits.Select(dbId);
                    int stackSize = unit.stackSize;
                    newPiece.units[i] = new Unit(dbId, dbData, stackSize);
                }
            }

            FieldMap fieldMap = FieldManager.Instance.mapHandler.map;

            Vector2Int id = new Vector2Int(posX, posY);
            newPiece.currentTile = fieldMap.tiles[id];
            newPiece.currentTile.occupantPiece = newPiece;

            if (hero != null)
            {
                newPiece.SetAnimatorOverrideController(newPiece.hero.animatorField);
                newPiece.name = newPiece.hero.heroName + "´s army";
            }
            else
            {
                Unit relevantUnit = newPiece.units[0];
                newPiece.SetAnimatorOverrideController(relevantUnit.animatorField);
                newPiece.name = "Army of " + relevantUnit.namePlural;
            }
        }
    }

    public void RemovePiece(FieldPiece piece)
    {
        pieces.Remove(piece);
        piece.currentTile.occupantPiece = null;
        Destroy(piece.gameObject);
    }

    public void Pathfind(FieldPiece piece, FieldTile targetTile,
        bool needGroundAccess = true, bool needWaterAccess = false, bool needLavaAccess = false)
    {
        Pathfinder.FindPath(piece.currentTile, targetTile, Pathfinder.OctoHeuristic,
            needGroundAccess, needWaterAccess, needLavaAccess,
            out List<PathNode> result, out float pathCost);
        piece.SetPath(result, Mathf.CeilToInt(pathCost), targetTile);
    }

    public void PartiesAreInteracting(FieldPiece sender, FieldPiece receiver)
    {
        if (sender.owner == receiver.owner)
        {
            GameManager.Instance.PerformExchange(sender, receiver);
        }
        else
        {
            GameManager.Instance.GoToCombat(sender, receiver);
        }
    }
}
