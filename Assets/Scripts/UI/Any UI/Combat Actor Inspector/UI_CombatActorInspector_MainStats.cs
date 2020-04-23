using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CombatActorInspector_MainStats : MonoBehaviour
{
    public Image imgPortrait;
    public Text txtHealthPoints;
    public Text txtManaPoints;
    public Text txtStackSize;

    public void RefreshInfo(AbstractCombatActorPiece2 acap)
    {
        CombatantHeroPiece2 hero = acap as CombatantHeroPiece2;
        CombatantUnitPiece2 unit = acap as CombatantUnitPiece2;

        if (hero)
        {
            txtManaPoints.gameObject.SetActive(true);
            txtStackSize.gameObject.SetActive(false);

            imgPortrait.sprite = hero.hero.dbData.profilePicture;
            txtHealthPoints.text = hero.combatPieceStats.hitPoints_current + "/" + hero.combatPieceStats.hitPoints_maximum;
            txtManaPoints.text = "missing mana points";
        }
        else if (unit)
        {
            txtManaPoints.gameObject.SetActive(false);
            txtStackSize.gameObject.SetActive(true);

            imgPortrait.sprite = unit.unit.dbData.profilePicture;
            txtHealthPoints.text = unit.combatPieceStats.hitPoints_current + "/" + unit.combatPieceStats.hitPoints_maximum;
            //txtStackSize.text = unit.stackStats.stack + "/" + unit.stackStats.stack_maximum;
            txtStackSize.text = unit.stackStats.Get().ToString();
        }
    }
}
