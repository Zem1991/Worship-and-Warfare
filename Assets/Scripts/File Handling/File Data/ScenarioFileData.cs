using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScenarioFileData
{
    public string name;
    public string author;
    public int width;
    public int height;

    public PlayerData[] players;
    public MapData map;
    //public MapData extraMap;
    public PieceData[] pieces;
}
