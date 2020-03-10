using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_HeroClass : AbstractDBContent
{
    public string className;
    public string classDescription;
    public HeroClassType classType;

    [Header("Stats")]
    public ResourceStats resourceStats;
    public CombatPieceStats combatPieceStats;
    public AttributeStats attributeStats;

    [Header("Animations")]
    public AnimatorOverrideController animatorField;
    public AnimatorOverrideController animatorCombat;

    [Header("References")]
    public DB_Faction faction;

    public string GetDescriptionWithCosts()
    {
        Dictionary<ResourceStats, int> costs = new Dictionary<ResourceStats, int> { [resourceStats] = 1 };
        return classDescription + "\n" + "Costs: " + resourceStats.WrittenForm(costs);
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
