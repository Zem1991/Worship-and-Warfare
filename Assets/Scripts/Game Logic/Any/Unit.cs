using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public DB_Unit dbData;

    public int hitPointsMax;
    public int stackSizeCurrent;

    public int damageMin;
    public int damageMax;
    public bool hasRangedAttack;
    public int resistance;
    public int movementRange;
    public int initiative;

    public void Initialize(UnitData unitData, DB_Unit dbData)
    {
        this.dbData = dbData;

        hitPointsMax = dbData.hitPoints;
        stackSizeCurrent = unitData.stackSize;

        damageMin = dbData.damageMin;
        damageMax = dbData.damageMax;
        hasRangedAttack = dbData.hasRangedAttack;
        resistance = dbData.resistance;
        movementRange = dbData.movementRange;
        initiative = dbData.initiative;

        name = GetName();
    }

    public string GetName()
    {
        if (stackSizeCurrent <= 1) return dbData.nameSingular;
        return dbData.namePlural;
    }
}
