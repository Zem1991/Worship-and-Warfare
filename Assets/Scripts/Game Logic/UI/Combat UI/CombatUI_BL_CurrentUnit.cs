using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI_BL_CurrentUnit : AUIPanel
{
    public Image unitPortrait;
    public Text txtUnitName;

    public Text txtDamage;
    public Text txtResistance;
    public Text txtSpeed;
    public Text txtInitiative;

    public void UpdatePanel(UnitCombatPiece uc)
    {
        if (!uc) return;

        unitPortrait.sprite = uc.imgProfile;
        txtUnitName.text = uc.GetName();

        txtDamage.text = uc.damageMin.ToString() + " - " + uc.damageMax.ToString();
        txtResistance.text = uc.resistance.ToString();
        txtSpeed.text = uc.speed.ToString();
        txtInitiative.text = uc.initiative.ToString();
    }
}
