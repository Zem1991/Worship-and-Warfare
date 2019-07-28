using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : Singleton<PieceManager>
{
    public readonly int MAX_UNITS = 5;

    [Header("Prefabs")]
    public Piece prefabPiece;

    [Header("Pieces")]
    public List<Piece> pieces;

    public void DeletePieces()
    {
        foreach (var item in pieces)
        {
            Destroy(item);
        }
        pieces.Clear();
    }

    public void CreatePieces(PieceData[] data)
    {
        DeletePieces();

        DatabaseManager db = DatabaseManager.Instance;
        DBHandler_Hero dbHeroes = db.heroes;
        DBHandler_Unit dbUnits = db.units;
        MapManager mm = MapManager.Instance;
        PlayerManager pm = PlayerManager.Instance;

        pieces = new List<Piece>();
        foreach (var item in data)
        {
            int posX = item.mapPosition[0];
            int posY = item.mapPosition[1];

            Vector3 pos = new Vector3(posX, 0, posY);
            Quaternion rot = Quaternion.identity;

            Piece newPiece = Instantiate(prefabPiece, pos, rot, transform);
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
                if (item.units.Length > MAX_UNITS)
                {
                    Debug.LogWarning("There are more units than the piece can store!");
                }

                newPiece.units = new Unit[MAX_UNITS];
                for (int i = 0; i < MAX_UNITS; i++)
                {
                    UnitData unit = item.units[i];
                    int dbId = unit.unitId;
                    DB_Unit dbData = dbUnits.Select(dbId);
                    int stackSize = unit.stackSize;
                    newPiece.units[i] = new Unit(dbId, dbData, stackSize);
                }
            }

            newPiece.currentTile = mm.map.tiles[posX, posY];
            newPiece.currentTile.piece = newPiece;

            if (hero != null)
            {
                newPiece.ChangeSprite(newPiece.hero.imgField);
                newPiece.name = newPiece.hero.heroName + "´s army";
            }
            else
            {
                Unit relevantUnit = newPiece.units[0];
                newPiece.ChangeSprite(relevantUnit.imgField);
                newPiece.name = "Army of " + relevantUnit.unitName;
            }
        }
    }

    public void Pathfind(Piece piece, FieldTile targetTile,
        bool needGroundAccess = true, bool needWaterAccess = false, bool needLavaAccess = false)
    {
        Pathfinder.FindPath(piece.currentTile, targetTile, out List<PathNode> result, out float pathCost,
            needGroundAccess, needWaterAccess, needLavaAccess);
        piece.SetPath(result, Mathf.CeilToInt(pathCost), targetTile);
    }

    public void PiecesAreInteracting(Piece sender, Piece receiver)
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
