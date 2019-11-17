using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPieceStats : MonoBehaviour
{
    [Header("Cost")]
    public CostStats costStats;

    [Header("Offense")]
    public AttackStats attack_melee;
    public AttackStats attack_ranged;

    [Header("Defense")]
    public int armor_physical;
    public int armor_magical;

    [Header("Combat actions settings")]
    public bool canWait;
    public bool canDefend;
    public bool canRetaliate;
    public bool canCounter;
    public int retaliationsMax;

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

        armor_physical = combatPieceStats.armor_physical;
        armor_magical = combatPieceStats.armor_magical;

        hitPoints_current = combatPieceStats.hitPoints_current;
        hitPoints_maximum = combatPieceStats.hitPoints_maximum;

        initiative = combatPieceStats.initiative;
        movementRange = combatPieceStats.movementRange;
        movementType = combatPieceStats.movementType;

        costStats = Instantiate(prefabCS, transform);
        costStats.Initialize(); //TODO add data

        attack_melee = Instantiate(prefabAS, transform);
        attack_melee.Initialize(combatPieceStats.attack_melee);
        attack_melee.isRanged = false;
        attack_melee.name = "Melee attack stats";

        if (combatPieceStats.attack_ranged)
        {
            attack_ranged = Instantiate(prefabAS, transform);
            attack_ranged.Initialize(combatPieceStats.attack_ranged);
            attack_ranged.isRanged = true;
            attack_melee.name = "Ranged attack stats";
        }
    }

    public bool TakeDamage(int amount)
    {
        hitPoints_current -= amount;
        return hitPoints_current <= 0;
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
