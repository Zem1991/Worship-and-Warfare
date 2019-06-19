using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScenarioFileData
{
    public string name;
    public string author;
    public MapData map;
    public PieceData[] pieces;

    public ScenarioFileData(MapData map, PieceData[] pieces)
    {
        this.map = map;
        this.pieces = pieces;
    }
}
