using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_CombatUnit : DB_Unit
{
    [Header("Combat unit identification")]
    public string unitNameSingular;
    public string unitNamePlural;

    [Header("Combat unit settings")]
    public int tier;
    public int experienceValue;
}
