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

    public void UpdatePanel(AbstractCombatPiece2 unit)
    {
        if (unit == null) return;

        CombatantUnitPiece2 cup = unit as CombatantUnitPiece2;
        if (cup)
        {
            unitPortrait.sprite = cup.profilePicture;
            txtUnitName.text = cup.unit.GetName();

            txtUnitHitPoints.text = cup.hitPointsCurrent + "/" + cup.hitPointsMax;
            txtUnitStackCount.text = cup.stackSizeCurrent + "/" + cup.stackSizeStart;

            txtDamage.text = cup.unit.damageMin.ToString() + " - " + cup.unit.damageMax.ToString();
            txtResistance.text = cup.unit.resistance.ToString();
            txtSpeed.text = cup.unit.movementRange.ToString();
            txtInitiative.text = cup.unit.initiative.ToString();
        }

    }
}
