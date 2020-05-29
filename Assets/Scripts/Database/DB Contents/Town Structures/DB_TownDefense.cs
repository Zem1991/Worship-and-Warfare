using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_TownDefense : DB_TownStructure
{
    [Header("Defense identification")]
    public TownDefenseType defenseType;

    [Header("Defense stats")]
    public SettingsStats2 settingsStats;
    public HealthStats2 healthStats;
    public OffenseStats2 offenseStats;
    public DefenseStats2 defenseStats;

    [Header("Defense animations")]
    public AnimatorOverrideController animatorCombat;
}
