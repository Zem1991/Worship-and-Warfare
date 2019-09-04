﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCombatPiece : AbstractCombatPiece
{
    //public bool didMove;
    //public bool didAttack;

    [Header("Unit combat stats")]
    public string unitName;
    public int hitPoints;
    public int hitPointsCurrent;
    public int stackSize;
    public int stackSizeCurrent;
    public int damageMin;
    public int damageMax;
    public int resistance;
    public int speed;
    public int initiative;

    public void Initialize(Unit unit)
    {
        unitName = unit.unitName;
        hitPoints = unit.hitPoints;
        hitPointsCurrent = hitPoints;
        stackSize = unit.stackSize;
        stackSizeCurrent = stackSize;

        damageMin = unit.damageMin;
        damageMax = unit.damageMax;
        resistance = unit.resistance;
        speed = unit.speed;
        initiative = unit.initiative;

        imgProfile = unit.imgProfile;
        SetAnimatorOverrideController(unit.animatorCombat);
    }

    protected override void InteractWithPiece(AbstractPiece target)
    {
        UnitCombatPiece targetUnit = target as UnitCombatPiece;
        if (targetUnit)
        {
            Debug.LogWarning("InteractWithPiece insta-killed the target!");
            targetUnit.hitPointsCurrent = 0;
        }
        else
        {
            Debug.LogWarning("InteractWithPiece IS DESTROYING PIECES!");
            Destroy(target.gameObject);
        }
    }
}
