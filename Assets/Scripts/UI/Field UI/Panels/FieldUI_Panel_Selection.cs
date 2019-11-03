using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldUI_Panel_Selection : AUIPanel
{
    public Text txtSelectionTitle;

    public UI_HeroInfo heroInfo;
    public UI_UnitsInfo unitsInfo;
    public UI_PickupInfo pickupInfo;

    public void HideInformations()
    {
        txtSelectionTitle.text = "--";

        heroInfo.Hide();
        unitsInfo.Hide();
        pickupInfo.Hide();
    }

    public void UpdatePanel(PartyPiece2 party)
    {
        HideInformations();
        if (!party) return;

        Hero hero = party.partyHero;
        if (hero)
        {
            heroInfo.RefreshInfo(hero);
            heroInfo.Show();

            txtSelectionTitle.text = hero.dbData.heroName + "'s party";
        }

        List<Unit> units = party.partyUnits;
        if (units != null)
        {
            unitsInfo.RefreshInfo(units);
            unitsInfo.Show();

            if (hero == null) txtSelectionTitle.text = "Non-commissioned party";
        }
    }

    public void UpdatePanel(PickupPiece2 pickup)
    {
        HideInformations();
        if (!pickup) return;

        pickupInfo.RefreshInfo(pickup, txtSelectionTitle);
        pickupInfo.Show();
    }
}
