using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUnit : AbstractUnit
{
    public void Initialize(DB_CombatUnit dbCombatUnit, int stackSize)
    {
        Initialize(dbCombatUnit);
        name = dbCombatUnit.unitNameSingular;

        StackHealthStats2 shs = GetStackHealthStats();
        shs.CopyFrom(dbCombatUnit.healthStats);
        shs.SetStackSize(stackSize);
    }

    public DB_CombatUnit GetDBCombatUnit()
    {
        return dbUnit as DB_CombatUnit;
    }

    public override string AU_GetUnitName()
    {
        if (GetStackHealthStats().GetStackSize() <= 1) return GetDBCombatUnit().unitNameSingular;
        return GetDBCombatUnit().unitNamePlural;
    }

    public override Sprite AU_GetProfileImage()
    {
        return GetDBCombatUnit().profilePicture;
    }

    public StackHealthStats2 GetStackHealthStats()
    {
        return healthStats as StackHealthStats2;
    }
}
