using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_Artifact : DBContent
{
    [Header("Artifact identification")]
    public new string name;
    public ArtifactType type;
    public ArtifactRarity rarity;
    public Sprite image;

    [Header("Artifact parameters")]
    public int atrCommand;
    public int atrOffense;
    public int atrDefense;
    public int atrPower;
    public int atrFocus;
}
