using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CombatActorInspector_OtherStats : MonoBehaviour
{
    public Text txtMorale;
    public Text txtFaction;
    public Text txtTier;

    public void RefreshInfo(CombatantPiece3 acap)
    {
        if (acap)
        {
            txtMorale.text = "morale missing";

            HeroUnitPiece3 hero = acap as HeroUnitPiece3;
            CombatUnitPiece3 unit = acap as CombatUnitPiece3;

            if (hero)
            {
                txtFaction.text = hero.GetHeroUnit().dbHeroPerson.heroClass.faction.factionName;
                txtTier.text = "Hero";
            }
            else if (unit)
            {
                txtFaction.text = unit.GetCombatUnit().GetDBCombatUnit().faction.factionName;
                txtTier.text = "Tier " + unit.GetCombatUnit().GetDBCombatUnit().tier;
            }
        }
    }
}
