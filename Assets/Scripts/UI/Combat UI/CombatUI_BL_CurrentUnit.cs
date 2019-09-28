using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI_BL_CurrentUnit : AUIPanel
{
    public Image unitPortrait;
    public Text txtUnitName;

    public Text txtUnitHitPoints;
    public Text txtUnitStackCount;

    public Text txtDamage;
    public Text txtResistance;
    public Text txtSpeed;
    public Text txtInitiative;

    public void UpdatePanel(Unit unit)
    {
        if (unit == null) return;

        unitPortrait.sprite = unit.imgProfile;
        txtUnitName.text = unit.GetName();

        txtUnitHitPoints.text = unit.hitPointsCurrent + "/" + unit.hitPointsMax;
        txtUnitStackCount.text = unit.stackSizeCurrent + "/" + unit.stackSizeStart;

        txtDamage.text = unit.damageMin.ToString() + " - " + unit.damageMax.ToString();
        txtResistance.text = unit.resistance.ToString();
        txtSpeed.text = unit.speed.ToString();
        txtInitiative.text = unit.initiative.ToString();
    }
}
