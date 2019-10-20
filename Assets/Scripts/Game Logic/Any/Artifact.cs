using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : MonoBehaviour
{
    [Header("Artifact identification")]
    public DB_Artifact dbData;

    public void Initialize(DB_Artifact dbData)
    {
        this.dbData = dbData;
        //artifactName = dbArtifact.artifactName;
        //type = dbArtifact.type;
        //rarity = dbArtifact.rarity;
        //image = dbArtifact.image;

        //atrCommand = dbArtifact.atrCommand;
        //atrOffense = dbArtifact.atrOffense;
        //atrDefense = dbArtifact.atrDefense;
        //atrPower = dbArtifact.atrPower;
        //atrFocus = dbArtifact.atrFocus;
    }
}
