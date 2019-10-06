using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public DB_Unit dbData;

    public int hitPointsMax;
    public int hitPointsCurrent;
    public int stackSizeStart;
    public int stackSizeCurrent;

    public int damageMin;
    public int damageMax;
    public bool hasRangedAttack;
    public int resistance;
    public int speed;
    public int initiative;

    public void Initialize(UnitData unitData, DB_Unit dbData)
    {
        this.dbData = dbData;

        hitPointsMax = dbData.hitPoints;
        hitPointsCurrent = hitPointsMax;
        stackSizeStart = unitData.stackSize;
        stackSizeCurrent = stackSizeStart;

        damageMin = dbData.damageMin;
        damageMax = dbData.damageMax;
        hasRangedAttack = dbData.hasRangedAttack;
        resistance = dbData.resistance;
        speed = dbData.movementRange;
        initiative = dbData.initiative;

        name = GetName();
    }

    public string GetName()
    {
        if (stackSizeCurrent <= 1) return dbData.nameSingular;
        return dbData.namePlural;
    }
}
