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
            txtUnitStackCount.text = cup.stackStats.stack_current + "/" + cup.stackStats.stack_maximum;

            txtDamage.text = cup.unit.combatPieceStats.attack_primary.damage_minimum.ToString() + " ~ " + cup.unit.combatPieceStats.attack_primary.damage_maximum.ToString();
            txtResistance.text = cup.unit.combatPieceStats.armor_physical.ToString() + "/" + cup.unit.combatPieceStats.armor_magical.ToString();
            txtSpeed.text = cup.unit.combatPieceStats.movementRange.ToString();
            txtInitiative.text = cup.unit.combatPieceStats.initiative.ToString();
        }

    }
}
