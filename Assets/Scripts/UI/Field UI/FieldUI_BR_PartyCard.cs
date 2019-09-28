using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldUI_BR_PartyCard : AUIPanel
{
    public Image crest;

    public AnyUI_HeroInfo heroInfo;
    public AnyUI_UnitsInfo unitsInfo;

    public Image commandBar;
    public Image u1Portrait;
    public Text u1StackSize;
    public Image u2Portrait;
    public Text u2StackSize;
    public Image u3Portrait;
    public Text u3StackSize;
    public Image u4Portrait;
    public Text u4StackSize;
    public Image u5Portrait;
    public Text u5StackSize;

    public void UpdatePanel(FieldPiece p)
    {
        if (!p)
        {
            heroInfo.Hide();
            unitsInfo.Hide();
            return;
        }

        unitsInfo.Show();

        Hero hero = p.hero;
        if (hero)
        {
            heroInfo.UpdatePanel(p.hero);
            heroInfo.Show();
        }
        else
        {
            heroInfo.Hide();
        }
    }
}
