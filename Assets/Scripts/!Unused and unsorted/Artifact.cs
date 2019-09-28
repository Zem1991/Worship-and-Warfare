using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArtifactType
{
    ANY,
    MAIN_HAND,
    OFF_HAND,
    HELMET,
    ARMOR,
    TRINKET
}

public enum ArtifactRarity
{
    COMMON,
    MINOR,
    MAJOR,
    RELIC
}

public class Artifact
{
    public ArtifactType type;
    public ArtifactRarity rarity;
    public string name;

    public int cmdBoost;
    public int atkBoost;
    public int defBoost;
    public int pwrBoost;
    public int fcsBoost;
}
