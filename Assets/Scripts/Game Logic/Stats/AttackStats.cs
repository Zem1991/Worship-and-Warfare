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

    public void Initialize(AttackStats attackStats)
    {
        damage_minimum = attackStats.damage_minimum;
        damage_maximum = attackStats.damage_maximum;
        isRanged = attackStats.isRanged;
        range_effective = attackStats.range_effective;
        range_maximum = attackStats.range_maximum;
    }
}
