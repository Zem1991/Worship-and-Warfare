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

    public void RefreshInfo(CombatantPiece3 acap)
    {
        HeroUnitPiece3 hero = acap as HeroUnitPiece3;
        CombatUnitPiece3 unit = acap as CombatUnitPiece3;

        if (hero)
        {
            txtManaPoints.gameObject.SetActive(true);
            txtStackSize.gameObject.SetActive(false);

            imgPortrait.sprite = hero.GetHeroUnit().dbHeroPerson.profilePicture;
            txtHealthPoints.text = hero.healthStats.hitPoints_current + "/" + hero.healthStats.hitPoints_maximum;
            txtManaPoints.text = "missing mana points";
        }
        else if (unit)
        {
            txtManaPoints.gameObject.SetActive(false);
            txtStackSize.gameObject.SetActive(true);

            imgPortrait.sprite = unit.GetCombatUnit().GetDBCombatUnit().profilePicture;
            txtHealthPoints.text = unit.healthStats.hitPoints_current + "/" + unit.healthStats.hitPoints_maximum;
            //txtStackSize.text = unit.stackStats.stack + "/" + unit.stackStats.stack_maximum;
            txtStackSize.text = unit.GetStackHealthStats().GetStackSize().ToString();
        }
    }
}
