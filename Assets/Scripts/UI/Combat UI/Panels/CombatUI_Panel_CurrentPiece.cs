using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI_Panel_CurrentPiece : AUIPanel
{
    public Image unitPortrait;
    public Text txtUnitName;

    public Text txtUnitHitPoints;
    public Text txtUnitStackCount;

    public Text txtDamage;
    public Text txtResistance;
    public Text txtSpeed;
    public Text txtInitiative;

    public void UpdatePanel(AbstractCombatantPiece2 actp)
    {
        if (actp == null) return;

        CombatantHeroPiece2 asHero = actp as CombatantHeroPiece2;
        CombatantUnitPiece2 asUnit = actp as CombatantUnitPiece2;

        Sprite portrait = null;
        if (asHero)
        {
            portrait = asHero.hero.dbData.profilePicture;
            txtUnitName.text = asHero.hero.dbData.heroName;
            txtUnitStackCount.text = "--";
        }
        else if (asUnit)
        {
            portrait = asUnit.unit.dbData.profilePicture;
            txtUnitName.text = asUnit.unit.GetName();
            txtUnitStackCount.text = asUnit.stackStats.stack_current + "/" + asUnit.stackStats.stack_maximum;
        }

        unitPortrait.sprite = portrait;
        txtUnitHitPoints.text = actp.combatPieceStats.hitPoints_current + "/" + actp.combatPieceStats.hitPoints_maximum;

        txtDamage.text = actp.combatPieceStats.attack_primary.damage_minimum.ToString() + " ~ " + actp.combatPieceStats.attack_primary.damage_maximum.ToString();
        txtResistance.text = actp.combatPieceStats.armor_physical.ToString() + "/" + actp.combatPieceStats.armor_magical.ToString();
        txtSpeed.text = actp.combatPieceStats.movementRange.ToString();
        txtInitiative.text = actp.combatPieceStats.initiative.ToString();
    }
}
