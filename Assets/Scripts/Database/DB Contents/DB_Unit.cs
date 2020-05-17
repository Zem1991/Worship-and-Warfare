using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_Unit : AbstractDBContent
{
    [Header("Unit identification")]
    public Sprite profilePicture;
    public UnitType unitType;

    [Header("Unit stats")]
    public ResourceStats resourceStats;
    public SettingsStats2 settingsStats;
    public HealthStats2 healthStats;
    public MovementStats2 movementStats;
    public AttributeStats2 attributeStats;
    public OffenseStats2 offenseStats;
    public DefenseStats2 defenseStats;
    public AbilityStats2 abilityStats;

    [Header("Unit animations")]
    public AnimatorOverrideController animatorField;
    public AnimatorOverrideController animatorCombat;

    [Header("Unit references")]
    public DB_Faction faction;
}
