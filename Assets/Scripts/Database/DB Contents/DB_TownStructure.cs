using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_TownStructure : AbstractDBContent
{
    [Header("Database references")]
    public DB_Faction faction;

    [Header("Structure identification")]
    public string structureName;
    public Sprite structureImage;
    public string structureDescription;
    public Rect rect;

    [Header("Structure stats")]
    public ResourceStats2 resourceStats;

    //[Header("Animations")]
    //public AnimatorOverrideController animator;

    public string GetDescriptionWithCosts()
    {
        Dictionary<ResourceStats2, int> costs = new Dictionary<ResourceStats2, int> { [resourceStats] = 1 };
        return structureDescription + "\n" + "Costs: " + resourceStats.WrittenForm(costs);
    }
}
