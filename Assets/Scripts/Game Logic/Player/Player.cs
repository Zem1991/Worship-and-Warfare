using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType
{
    HUMAN,
    COMPUTER,
    REMOTE,
    INACTIVE
}

public class Player : MonoBehaviour
{
    [Header("Identification")]
    public PlayerType type;
    public Color color;
    public string playerName;

    [Header("Game Data")]
    public DBHandler_Faction faction;
    public long gold;

    [Header("Game Flow")]
    public int currentTurn;
    public bool currentTurnAvailable;

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
