using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffenseStats2 : MonoBehaviour
{
    //public AttackStats2 attack_melee;
    //public AttackStats2 attack_ranged;

    [Header("Attacks")]
    [SerializeField] private AttackStats2 meleeAttack;
    [SerializeField] private AttackStats2 rangedAttack;

    [Header("Settings")]
    public bool useRangedAtMeleeRange = false;
    //public bool canUseRanged = true;        //TODO: remove this later?

    public void CopyFrom(OffenseStats2 offenseStats)
    {
        AttackStats2[] listAttack = offenseStats.GetComponentsInChildren<AttackStats2>();
        if (listAttack.Length > 2) Debug.LogWarning("More than 2 attacks were found.");

        foreach (AttackStats2 attack in listAttack)
        {
            AttackStats2 copy = Instantiate(attack, transform);
            //listAttack.Add(copy);

            if (!meleeAttack && copy.attackType == AttackType.MELEE) meleeAttack = copy;
            if (!rangedAttack && copy.attackType == AttackType.RANGED) rangedAttack = copy;
        }
    }

    public AttackStats2 GetMeleeAttack()
    {
        return meleeAttack;
    }

    public AttackStats2 GetRangedAttack()
    {
        return rangedAttack;
    }
}
