using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUnit : AbstractUnit
{
    public void Initialize(DB_CombatUnit dbCombatUnit, int stack_maximum)
    {
        Initialize(dbCombatUnit);
        name = dbCombatUnit.unitNameSingular;

        GetStackHealthStats().CopyFrom(dbCombatUnit.healthStats);
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
