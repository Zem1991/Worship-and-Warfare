using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public System.TimeSpan timeElapsed;
    public int currentDay;
    public int currentPlayer;

    public GameData(System.TimeSpan timeElapsed, int currentDay, int currentPlayer)
    {
        this.timeElapsed = timeElapsed;
        this.currentDay = currentDay;
        this.currentPlayer = currentPlayer;
    }
}
