using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldUI_BR_PartyCard : AUIPanel
{
    public Image heroPortrait;
    public Text txtHeroName;
    public Text txtLevelAndClass;

    public Text txtCommand;
    public Text txtOffense;
    public Text txtDefense;
    public Text txtPower;
    public Text txtFocus;

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
        heroPortrait.sprite = p.hero.imgProfile;
        txtHeroName.text = p.hero.heroName;
        txtLevelAndClass.text = "Level ?? " + p.hero.className;

        txtCommand.text = p.hero.atrCommand.ToString();
        txtOffense.text = p.hero.atrOffense.ToString();
        txtDefense.text = p.hero.atrDefense.ToString();
        txtPower.text = p.hero.atrPower.ToString();
        txtFocus.text = p.hero.atrFocus.ToString();
    }
}
