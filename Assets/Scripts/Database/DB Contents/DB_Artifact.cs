using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_Artifact : AbstractDBContent
{
    [Header("Identification")]
    public string artifactName;
    public ArtifactType artifactType;
    public ArtifactRarity artifactRarity;
    public Sprite image;
    public string artifactDescription;

    [Header("Stats")]
    public AttributeStats2 attributeStats;
}
