using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : MonoBehaviour
{
    [Header("Artifact identification")]
    public string artifactName;
    public ArtifactType type;
    public ArtifactRarity rarity;
    public Sprite image;

    [Header("Artifact parameters")]
    public int atrCommand;
    public int atrOffense;
    public int atrDefense;
    public int atrPower;
    public int atrFocus;

    public void Initialize(DB_Artifact dbArtifact)
    {
        artifactName = dbArtifact.name;
        type = dbArtifact.type;
        rarity = dbArtifact.rarity;
        image = dbArtifact.image;

        atrCommand = dbArtifact.atrCommand;
        atrOffense = dbArtifact.atrOffense;
        atrDefense = dbArtifact.atrDefense;
        atrPower = dbArtifact.atrPower;
        atrFocus = dbArtifact.atrFocus;
    }
}
