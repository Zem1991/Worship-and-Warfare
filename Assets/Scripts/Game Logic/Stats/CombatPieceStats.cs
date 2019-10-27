using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPieceStats : MonoBehaviour
{
    [Header("Cost")]
    public CostStats costStats;

    [Header("Offense")]
    public AttackStats attack_primary;

    [Header("Defense")]
    public int armor_physical;
    public int armor_magical;

    [Header("Health")]
    public int hitPoints_current;
    public int hitPoints_maximum;

    [Header("Mobility")]
    public int initiative;
    public int movementRange;
    public CombatMovementType movementType;

    public void Initialize(CombatPieceStats combatPieceStats)
    {
        CostStats prefabCS = AllPrefabs.Instance.costStats;
        AttackStats prefabAS = AllPrefabs.Instance.attackStats;

        costStats = Instantiate(prefabCS, transform);
        costStats.Initialize(); //TODO add data

        attack_primary = Instantiate(prefabAS, transform);
        attack_primary.Initialize(combatPieceStats.attack_primary);
    }

    public bool TakeDamage(int amount, StackStats stackStats, out int stackLost)
    {
        stackLost = amount / hitPoints_maximum;
        int hpLost = amount % hitPoints_maximum;
        hitPoints_current -= hpLost;
        if (hitPoints_current <= 0)
        {
            stackLost++;
            hitPoints_current += hitPoints_maximum;
        }
        stackStats.stack_current -= stackLost;
        return stackStats.stack_current <= 0;
    }
}
