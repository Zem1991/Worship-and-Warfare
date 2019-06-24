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

    [Header("Instances")]
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
        PlayerManager pm = PlayerManager.Singleton;

        pieces = new List<Piece>();
        foreach (var item in data)
        {
            int posX = item.mapPosition[0];
            int posZ = item.mapPosition[1];

            Vector3 pos = new Vector3(posX, 0, posZ);
            Quaternion rot = Quaternion.identity;

            Piece newPiece = Instantiate(prefabPiece, pos, rot, transform);
            pieces.Add(newPiece);

            newPiece.owner = pm.allPlayers[item.ownerId];

            HeroData hero = item.hero;
            if (hero != null)
            {
                List<DBContent> heroes = db.heroes.content;
                newPiece.hero = heroes[hero.heroId] as DB_Hero;
                newPiece.heroExperience = hero.experience;
            }

            if (item.units != null)
            {
                if (item.units.Length > MAX_UNITS)
                {
                    Debug.LogWarning("There are more units than the piece can store!");
                }

                List<DBContent> units = db.units.content;
                newPiece.units = new DB_Unit[MAX_UNITS];
                newPiece.stackSizes = new int[MAX_UNITS];

                for (int i = 0; i < MAX_UNITS; i++)
                {
                    UnitData unit = item.units[i];
                    newPiece.units[i] = units[unit.unitId] as DB_Unit;
                    newPiece.stackSizes[i] = unit.stackSize;
                }
            }
        }
    }
}
