using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldUI_BR_SelectionCard : AUIPanel
{
    public Image crest;

    public AnyUI_HeroInfo heroInfo;
    public AnyUI_UnitsInfo unitsInfo;
    public AnyUI_PickupInfo pickupInfo;

    //public Image commandBar;

    public void HidePanel()
    {
        heroInfo.Hide();
        unitsInfo.Hide();
        pickupInfo.Hide();
    }

    public void UpdatePanel(PartyPiece2 party)
    {
        HidePanel();
        if (!party) return;

        Hero hero = party.partyHero;
        if (hero)
        {
            heroInfo.RefreshInfo(hero);
            heroInfo.Show();
        }

        List<Unit> units = party.partyUnits;
        if (units != null)
        {
            unitsInfo.RefreshInfo(units);
            unitsInfo.Show();
        }
    }

    public void UpdatePanel(PickupPiece2 pickup)
    {
        HidePanel();
        if (!pickup) return;
        
        pickupInfo.RefreshInfo(pickup);
        pickupInfo.Show();
    }
}
