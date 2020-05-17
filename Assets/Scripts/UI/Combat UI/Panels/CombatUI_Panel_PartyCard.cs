using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI_Panel_PartyCard : AbstractUIPanel
{
    public UI_HeroInfo heroInfo;

    public Image commandBar;
    public Image manaBar;

    public void UpdatePanel(HeroUnitPiece3 hc)
    {
        if (!hc)
        {
            heroInfo.Hide();
            return;
        }

        HeroUnit hero = hc.GetHeroUnit();
        if (hero)
        {
            heroInfo.RefreshInfo(hero);
            heroInfo.Show();
        }
        else
        {
            heroInfo.Hide();
        }
    }
}
