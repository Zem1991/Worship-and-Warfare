using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HeroInfo : MonoBehaviour, IShowableHideable
{
    public Image heroPortrait;
    public Text txtHeroName;
    public Text txtLevelAndClass;

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void RefreshInfo(Hero hero)
    {
        if (hero != null)
        {
            if (heroPortrait) heroPortrait.sprite = hero.dbData.profilePicture;
            if (txtHeroName) txtHeroName.text = hero.dbData.heroName;
            if (txtLevelAndClass) txtLevelAndClass.text = "Level " + hero.experienceStats.level + " " + hero.dbData.classs.className;
        }
        else
        {
            if (heroPortrait) heroPortrait.sprite = null;
            if (txtHeroName) txtHeroName.text = "--";
            if (txtLevelAndClass) txtLevelAndClass.text = "--";
        }
    }
}
