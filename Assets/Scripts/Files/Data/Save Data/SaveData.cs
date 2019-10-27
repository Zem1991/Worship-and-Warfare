using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public System.TimeSpan timeElapsed;
    public int currentDay;
    public int currentPlayer;

    public SaveData(System.TimeSpan timeElapsed, int currentDay, int currentPlayer)
    {
        this.timeElapsed = timeElapsed;
        this.currentDay = currentDay;
        this.currentPlayer = currentPlayer;
    }
}
