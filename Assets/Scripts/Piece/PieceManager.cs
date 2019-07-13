using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    public static PieceManager Singleton;

    public readonly int MAX_UNITS = 5;

    [Header("Prefabs")]
    public Piece prefabPiece;

    [Header("Pieces")]
    public List<Piece> pieces;

    void Awake()
    {
        if (Singleton != null)
        {
            Debug.LogWarning("Only one instance of PieceManager may exist! Deleting this extra one.");
            Destroy(this);
        }
        else
        {
            Singleton = this;
        }
    }

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

        DatabaseManager db = DatabaseManager.Singleton;
        DBHandler_Hero dbHeroes = db.heroes;
        DBHandler_Unit dbUnits = db.units;
        MapManager mm = MapManager.Singleton;
        PlayerManager pm = PlayerManager.Singleton;

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
                newPiece.hero = dbHeroes.Select(hero.heroId) as DB_Hero;
                newPiece.heroExperience = hero.experience;
            }

            if (item.units != null)
            {
                if (item.units.Length > MAX_UNITS)
                {
                    Debug.LogWarning("There are more units than the piece can store!");
                }

                newPiece.units = new DB_Unit[MAX_UNITS];
                newPiece.stackSizes = new int[MAX_UNITS];
                for (int i = 0; i < MAX_UNITS; i++)
                {
                    UnitData unit = item.units[i];
                    newPiece.units[i] = dbUnits.Select(unit.unitId) as DB_Unit;
                    newPiece.stackSizes[i] = unit.stackSize;
                }
            }

            newPiece.currentTile = mm.map.tiles[posX, posY];
            newPiece.currentTile.piece = newPiece;

            if (hero != null)
            {
                newPiece.ChangeSprite(newPiece.hero.classs.fieldPicture);
                newPiece.name = newPiece.hero.heroName + "´s army";
            }
            else
            {
                DB_Unit relevantUnit = dbUnits.defaultContent as DB_Unit;
                if (item.units != null)
                {
                    relevantUnit = newPiece.units[0];
                }
                newPiece.ChangeSprite(relevantUnit.fieldPicture);
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
            GameManager.Singleton.PerformExchange(sender, receiver);
        }
        GameManager.Singleton.GoToCombat(sender, receiver);
    }
}
