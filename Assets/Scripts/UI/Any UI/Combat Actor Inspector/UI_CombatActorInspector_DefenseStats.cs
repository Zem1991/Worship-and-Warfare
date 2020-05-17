using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CombatActorInspector_DefenseStats : MonoBehaviour
{
    public Text txtPhysArmor;
    public Text txtMagicArmor;

    public void RefreshInfo(CombatantPiece3 acap)
    {
        if (acap)
        {
            txtPhysArmor.text = "" + acap.defenseStats.armor_physical;
            txtMagicArmor.text = "" + acap.defenseStats.armor_magical;
        }
    }
}
