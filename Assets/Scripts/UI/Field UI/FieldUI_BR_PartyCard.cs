using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldUI_BR_PartyCard : AUIPanel
{
    public Image crest;

    public AnyUI_HeroInfo heroInfo;
    public AnyUI_UnitsInfo unitsInfo;

    //public Image commandBar;

    public void UpdatePanel(PartyPiece2 p)
    {
        if (!p)
        {
            heroInfo.Hide();
            unitsInfo.Hide();
            return;
        }

        Hero hero = p.partyHero;
        if (hero)
        {
            heroInfo.RefreshInfo(hero);
            heroInfo.Show();
        }
        else
        {
            heroInfo.Hide();
        }

        List<Unit> units = p.partyUnits;
        if (units != null)
        {
            unitsInfo.RefreshInfo(units);
            unitsInfo.Show();
        }
        else
        {
            unitsInfo.Hide();
        }
    }
}
