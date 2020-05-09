using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStats : MonoBehaviour
{
    [Header("Damage")]
    public int damage_minimum;
    public int damage_maximum;

    [Header("Range")]
    public bool isRanged;
    public int range_effective;
    public int range_maximum;

    [Header("References")]
    public DB_Animation animationProjectile;

    public void Clone(AttackStats attackStats)
    {
        damage_minimum = attackStats.damage_minimum;
        damage_maximum = attackStats.damage_maximum;

        isRanged = attackStats.isRanged;
        range_effective = attackStats.range_effective;
        range_maximum = attackStats.range_maximum;

        animationProjectile = attackStats.animationProjectile;
    }

    public string AttackType()
    {
        string attackType = isRanged ? "Ranged attack" : "Melee attack";
        return attackType;
    }
}
