using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_HeroUnit : DB_Unit
{
    [Header("Hero unit identification")]
    public string className;
    //public string classDescription;
    public HeroClassType classType;

    [Header("Hero stats")]
    public AttributeStats attributeStats;

    public string GetDescriptionWithCosts()
    {
        Dictionary<ResourceStats, int> costs = new Dictionary<ResourceStats, int> { [resourceStats] = 1 };
        return "TODO: classDescription" + "\n" + "Costs: " + resourceStats.WrittenForm(costs);
    }

    public AttributeType GetPrimaryAttribute()
    {
        AttributeType result = AttributeType.COMMAND;
        switch (classType)
        {
            case HeroClassType.COMMAND:
                result = AttributeType.COMMAND;
                break;
            case HeroClassType.MAGIC:
                result = AttributeType.MAGIC;
                break;
            case HeroClassType.TECH:
                result = AttributeType.TECH;
                break;
            default:
                break;
        }
        return result;
    }
}
