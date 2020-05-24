using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Identification")]
    public string playerName;
    public PlayerType type;
    public int id;
    public DB_Color dbColor;
    public AIPersonality aiPersonality;

    [Header("Object components")]
    public ResourceStats2 currentResources;

    [Header("Pieces")]
    public DBHandler_Faction faction;
    public List<TownPiece3> townPieces = new List<TownPiece3>();
    public List<PartyPiece3> partyPieces = new List<PartyPiece3>();

    [Header("Game Flow")]
    public int currentTurn;
    public bool currentTurnAvailable;

    //private Vector3 startingPosition;

    public void Initialize(PlayerData data, int id, DB_Color dbColor)
    {
        playerName = data.name;
        type = data.playerType;

        this.id = id;
        this.dbColor = dbColor;

        currentResources.Initialize(data.resourceData);

        if (type == PlayerType.COMPUTER)
        {
            AIPersonality ai = Instantiate(AllPrefabs.Instance.aiPersonality, transform);
            ai.Initialize(this);
            aiPersonality = ai;
        }

        name = "P" + id + " - " + playerName;
    }

    public bool HasTurn()
    {
        return currentTurnAvailable;
    }

    public void EndTurn()
    {
        currentTurnAvailable = false;
    }

    public void RefreshTurn(int currentTurn)
    {
        this.currentTurn = currentTurn;
        currentTurnAvailable = true;
    }

    public void ApplyDailyIncome()
    {
        Dictionary<ResourceStats2, int> mapTownIncome = new Dictionary<ResourceStats2, int>();
        foreach (TownPiece3 townPiece in townPieces)
        {
            mapTownIncome.Add(townPiece.town.dailyIncome, 1);
        }
        currentResources.Add(mapTownIncome);
    }

    public Vector3 GetStartingPosition()
    {
        AbstractPiece3 piece = null;
        if (partyPieces.Count > 0) piece = partyPieces[0];
        else if (townPieces.Count > 0) piece = townPieces[0];

        Vector3 result = Vector3.zero;
        if (piece) result = piece.currentTile.transform.position;
        return result;      //TODO: Use the startingPosition vector3
    }
}
