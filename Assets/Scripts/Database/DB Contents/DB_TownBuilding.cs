using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_TownBuilding : AbstractDBContent
{
    public string townBuildingName;
    public TownBuildingType townBuildingType;
    public Sprite buildingImage;
    public string buildingDescription;
    public Rect rect;

    //[Header("Animations")]
    //public AnimatorOverrideController animator;

    [Header("References")]
    public DB_Faction faction;
}
