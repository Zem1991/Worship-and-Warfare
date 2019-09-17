﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitMovementType
{
    GROUND,
    FLY,
    TELEPORT
}

public class DB_Unit : DBContent
{
    public string nameSingular;
    public string namePlural;

    [Header("Stats")]
    public int goldCost;
    public int leadershipCost;
    public int hitPoints;
    public int damageMin;
    public int damageMax;
    public int resistance;
    public int movementRange;
    public int initiative;
    public UnitMovementType movementType;

    [Header("Graphics")]
    public Sprite profilePicture;

    [Header("Animations")]
    public AnimatorOverrideController animatorField;
    public AnimatorOverrideController animatorCombat;
}
