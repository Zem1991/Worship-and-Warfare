using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackData
{
    [Header("Damage")]
    public int damage_minimum;
    public int damage_maximum;

    [Header("Range")]
    public bool isRanged;
    public int range_effective;
    public int range_maximum;
}
