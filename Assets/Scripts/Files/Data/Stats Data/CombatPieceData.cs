using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPieceData
{
    [Header("Cost")]
    public CostData cost;

    [Header("Offense")]
    public AttackData attack_primary;

    [Header("Defense")]
    public int armor_physical;
    public int armor_magical;

    [Header("Health")]
    //public int hitPoints_current;
    public int hitPoints_maximum;

    [Header("Mobility")]
    public int initiative;
    public int movementRange;
    public CombatMovementType movementType;
}
