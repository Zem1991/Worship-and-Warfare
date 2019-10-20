using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Identification")]
    public int id;
    public PlayerType type;
    public AIPersonality aiPersonality;
    //public Color color;
    public DB_Color dbColor;
    public string playerName;

    [Header("Game Data")]
    public DBHandler_Faction faction;
    public long gold;
    public List<PartyPiece2> partyPieces = new List<PartyPiece2>();

    [Header("Game Flow")]
    public int currentTurn;
    public bool currentTurnAvailable;

    public void Initialize(PlayerData data, int id, DB_Color dbColor)
    {
        type = data.playerType;
        //color = PlayerManager.PLAYER_COLORS[data.colorId];
        playerName = data.name;

        this.id = id;
        this.dbColor = dbColor;

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
