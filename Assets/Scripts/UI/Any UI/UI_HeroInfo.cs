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

    public void ClearInfo()
    {
        if (heroPortrait) heroPortrait.sprite = null;
        if (txtHeroName) txtHeroName.text = "--";
        if (txtLevelAndClass) txtLevelAndClass.text = "--";
    }

    public void RefreshInfo(PartySlot slot)
    {
        Hero hero = slot?.slotObj as Hero;
        RefreshInfo(hero);
    }

    public void RefreshInfo(Hero hero)
    {
        ClearInfo();
        if (hero == null) return;

        if (heroPortrait) heroPortrait.sprite = hero.dbData.profilePicture;
        if (txtHeroName) txtHeroName.text = hero.dbData.heroName;
        if (txtLevelAndClass) txtLevelAndClass.text = "Level " + hero.experienceStats.level + " " + hero.dbData.classs.className;
    }
}
