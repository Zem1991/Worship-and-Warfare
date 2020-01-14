using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CombatActorInspector_DefenseStats : MonoBehaviour
{
    public Text txtPhysArmor;
    public Text txtMagicArmor;

    public void RefreshInfo(AbstractCombatActorPiece2 acap)
    {
        AbstractCombatantPiece2 combatant = acap as AbstractCombatantPiece2;

        if (combatant)
        {
            txtPhysArmor.text = "" + combatant.combatPieceStats.armor_physical;
            txtMagicArmor.text = "" + combatant.combatPieceStats.armor_magical;
        }
    }
}
