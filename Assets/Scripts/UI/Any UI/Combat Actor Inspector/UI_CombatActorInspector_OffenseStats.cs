using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CombatActorInspector_OffenseStats : MonoBehaviour
{
    public RectTransform boxMelee;
    public Text txtMeleeDamage;
    public RectTransform boxRanged;
    public Text txtRangedDamage;
    public Text txtRangedRange;

    public void RefreshInfo(AbstractCombatActorPiece2 acap)
    {
        AbstractCombatantPiece2 combatant = acap as AbstractCombatantPiece2;

        if (combatant)
        {
            AttackStats melee = combatant.combatPieceStats.attack_melee;
            AttackStats ranged = combatant.combatPieceStats.attack_ranged;

            if (melee)
            {
                boxMelee.gameObject.SetActive(true);
                txtMeleeDamage.text = melee.damage_minimum + " ~ " + melee.damage_maximum;
            }
            else
            {
                boxMelee.gameObject.SetActive(false);
            }

            if (ranged)
            {
                boxRanged.gameObject.SetActive(true);
                txtRangedDamage.text = ranged.damage_minimum + " ~ " + ranged.damage_maximum;
                txtRangedRange.text = "" + ranged.range_maximum;
            }
            else
            {
                boxRanged.gameObject.SetActive(false);
            }
        }
        else
        {
            boxMelee.gameObject.SetActive(false);
            boxRanged.gameObject.SetActive(false);
        }
    }
}
