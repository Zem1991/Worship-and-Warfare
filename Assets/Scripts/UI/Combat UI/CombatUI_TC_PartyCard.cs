using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI_TC_PartyCard : AUIPanel
{
    public AnyUI_HeroInfo heroInfo;

    public Image commandBar;
    public Image manaBar;

    public void UpdatePanel(CombatHeroPiece hc)
    {
        if (!hc)
        {
            heroInfo.Hide();
            return;
        }

        Hero hero = hc.hero;
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
