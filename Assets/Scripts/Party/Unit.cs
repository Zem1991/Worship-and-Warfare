using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : AbstractPartyElement
{
    [Header("Prefab references")]
    public CombatPieceStats combatPieceStats;
    public StackStats stackStats;

    [Header("Database reference")]
    public DB_CombatUnit dbData;

    public void Initialize(DB_CombatUnit dbData, int stack_maximum)
    {
        partyElementType = UnitCategory.CREATURE;

        this.dbData = dbData;
        name = dbData.unitNameSingular;

        combatPieceStats.Clone(dbData.combatPieceStats);
        stackStats.Initialize(stack_maximum);
    }

    public override Sprite GetProfileImage()
    {
        return dbData.profilePicture;
    }

    public string GetName()
    {
        if (stackStats.Get() <= 1) return dbData.unitNameSingular;
        return dbData.unitNamePlural;
    }
}
