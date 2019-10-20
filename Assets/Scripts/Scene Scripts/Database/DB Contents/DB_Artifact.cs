using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_Artifact : DBContent
{
    [Header("Artifact identification")]
    public string artifactName;
    public ArtifactType artifactType;
    public ArtifactRarity artifactRarity;
    public Sprite image;
    public string artifactDescription;

    [Header("Artifact parameters")]
    public int atrCommand;
    public int atrOffense;
    public int atrDefense;
    public int atrPower;
    public int atrFocus;
}
