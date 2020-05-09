using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPieceStats : MonoBehaviour
{
    [Header("Health")]
    public int hitPoints_current;
    public int hitPoints_maximum;

    [Header("Mobility")]
    public int initiative;
    public int movementRange;
    public MovementType movementType;

    [Header("Offense")]
    public AttackStats attack_melee;
    public AttackStats attack_ranged;

    [Header("Defense")]
    public int armor_physical;
    public int armor_magical;

    [Header("Abilities")]
    public DB_Ability ability1;
    public DB_Ability ability2;
    public DB_Ability ability3;

    [Header("Combat actions settings")]
    public bool canWait;
    public bool canDefend;
    public bool canRetaliate;
    public bool canCounter;
    public int retaliationsMax;

    public void Clone(CombatPieceStats combatPieceStats)
    {
        hitPoints_current = combatPieceStats.hitPoints_current;
        hitPoints_maximum = combatPieceStats.hitPoints_maximum;

        initiative = combatPieceStats.initiative;
        movementRange = combatPieceStats.movementRange;
        movementType = combatPieceStats.movementType;

        attack_melee.Clone(combatPieceStats.attack_melee);
        attack_melee.isRanged = false;
        attack_melee.name = "Melee attack";
        if (combatPieceStats.attack_ranged) //TODO remove this later if everything get 2 attacks
        {
            attack_ranged.Clone(combatPieceStats.attack_ranged);
            attack_ranged.isRanged = true;
            attack_ranged.name = "Ranged attack";
        }

        armor_physical = combatPieceStats.armor_physical;
        armor_magical = combatPieceStats.armor_magical;

        ability1 = combatPieceStats.ability1;
        ability2 = combatPieceStats.ability2;
        ability3 = combatPieceStats.ability3;
    }

    public bool ReceiveHealing(int amount)
    {
        hitPoints_current += amount;
        hitPoints_current = Mathf.Clamp(hitPoints_current, 0, hitPoints_maximum);
        return hitPoints_current >= hitPoints_maximum;
    }

    public bool ReceiveDamage(int amount)
    {
        hitPoints_current -= amount;
        hitPoints_current = Mathf.Clamp(hitPoints_current, 0, hitPoints_maximum);
        return hitPoints_current <= 0;
    }

    public bool ReceiveDamage(int amount, StackStats stackStats, out int stackLost)
    {
        stackLost = amount / hitPoints_maximum;
        int hpLost = amount % hitPoints_maximum;
        hitPoints_current -= hpLost;
        if (hitPoints_current <= 0)
        {
            stackLost++;
            hitPoints_current += hitPoints_maximum;
        }
        if (stackLost > stackStats.Get())
        {
            stackLost = stackStats.Get();
        }
        return stackStats.Subtract(stackLost);
    }
}
