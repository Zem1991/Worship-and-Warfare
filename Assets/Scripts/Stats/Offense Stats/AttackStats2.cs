using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStats2 : MonoBehaviour
{
    public AttackType attackType;

    [Header("Damage")]
    public int damage_minimum;
    public int damage_maximum;

    [Header("Range")]
    public int range_effective;
    public int range_maximum;

    [Header("References")]
    public DB_Animation animationProjectile;

    public bool IsRanged()
    {
        return attackType == AttackType.RANGED;
    }

    public string GetAnimatorStateName()
    {
        return attackType.GetAnimatorStateName();
    }
}
