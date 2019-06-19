using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PieceData
{
    public string name;
    public int[] mapPosition;

    public PieceData(string name, int[] mapPosition)
    {
        this.name = name;
        this.mapPosition = mapPosition;
    }
}
