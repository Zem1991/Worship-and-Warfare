﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnyUI_HeroInfo : AUIPanel
{
    public Image heroPortrait;
    public Text txtHeroName;
    public Text txtLevelAndClass;

    public Text txtCommand;
    public Text txtOffense;
    public Text txtDefense;
    public Text txtPower;
    public Text txtFocus;

    public void UpdatePanel(Hero hero)
    {
        if (hero != null)
        {
            heroPortrait.sprite = hero.imgProfile;
            txtHeroName.text = hero.heroName;
            txtLevelAndClass.text = "Level ?? " + hero.className;

            txtCommand.text = hero.atrCommand.ToString();
            txtOffense.text = hero.atrOffense.ToString();
            txtDefense.text = hero.atrDefense.ToString();
            txtPower.text = hero.atrPower.ToString();
            txtFocus.text = hero.atrFocus.ToString();
        }
        else
        {
            heroPortrait.sprite = null;
            txtHeroName.text = "--";
            txtLevelAndClass.text = "--";

            txtCommand.text = "--";
            txtOffense.text = "--";
            txtDefense.text = "--";
            txtPower.text = "--";
            txtFocus.text = "--";
        }
    }
}