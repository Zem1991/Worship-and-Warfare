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

    public void RefreshInfo(AbstractCombatActorPiece2 acap)
    {
        AbstractCombatantPiece2 combatant = acap as AbstractCombatantPiece2;

        if (combatant)
        {
            txtMorale.text = "morale missing";

            CombatantHeroPiece2 hero = acap as CombatantHeroPiece2;
            CombatantUnitPiece2 unit = acap as CombatantUnitPiece2;

            if (hero)
            {
                txtFaction.text = hero.GetHero().dbData.heroClass.faction.factionName;
                txtTier.text = "Hero";
            }
            else if (unit)
            {
                txtFaction.text = unit.GetUnit().dbData.faction.factionName;
                txtTier.text = "Tier " + unit.GetUnit().dbData.tier;
            }
        }
    }
}
