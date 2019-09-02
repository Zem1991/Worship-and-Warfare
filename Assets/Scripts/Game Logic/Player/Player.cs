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
}
