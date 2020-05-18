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

    [Header("Stats")]
    public ResourceStats2 resourceStats;

    //[Header("Animations")]
    //public AnimatorOverrideController animator;

    [Header("References")]
    public DB_Faction faction;

    public string GetDescriptionWithCosts()
    {
        Dictionary<ResourceStats2, int> costs = new Dictionary<ResourceStats2, int> { [resourceStats] = 1 };
        return buildingDescription + "\n" + "Costs: " + resourceStats.WrittenForm(costs);
    }
}
