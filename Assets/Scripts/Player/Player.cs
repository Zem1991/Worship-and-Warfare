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

    [Header("Stats")]
    public ResourceStats resourceStats;

    [Header("Pieces")]
    public DBHandler_Faction faction;
    public List<TownPiece2> townPieces = new List<TownPiece2>();
    public List<PartyPiece2> partyPieces = new List<PartyPiece2>();

    [Header("Game Flow")]
    public int currentTurn;
    public bool currentTurnAvailable;

    public void Initialize(PlayerData data, int id, DB_Color dbColor)
    {
        playerName = data.name;
        type = data.playerType;

        this.id = id;
        this.dbColor = dbColor;

        resourceStats.Initialize(data.resourceData);

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
}
