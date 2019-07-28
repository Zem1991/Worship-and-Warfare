﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI_TC_PartyCard : MonoBehaviour
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
    public Image manaBar;

    public void UpdatePanel(HeroCombat hc)
    {
        heroPortrait.sprite = hc.imgProfile;
        txtHeroName.text = hc.heroName;
        txtLevelAndClass.text = "Level ?? " + hc.className;

        txtCommand.text = hc.atrCommand.ToString();
        txtOffense.text = hc.atrOffense.ToString();
        txtDefense.text = hc.atrDefense.ToString();
        txtPower.text = hc.atrPower.ToString();
        txtFocus.text = hc.atrFocus.ToString();
    }
}