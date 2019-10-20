﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public DB_Unit dbData;

    public int hitPointsMax;
    public int stackSize;

    public int damageMin;
    public int damageMax;
    public bool hasRangedAttack;
    public int resistance;
    public int movementRange;
    public int initiative;

    public void Initialize(DB_Unit dbData, int stackSize)
    {
        this.dbData = dbData;

        hitPointsMax = dbData.hitPoints;
        this.stackSize = stackSize;

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
        if (stackSize <= 1) return dbData.nameSingular;
        return dbData.namePlural;
    }
}
