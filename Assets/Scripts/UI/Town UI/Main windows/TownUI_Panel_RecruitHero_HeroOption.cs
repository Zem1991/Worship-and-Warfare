﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TownUI_Panel_RecruitHero_HeroOption : MonoBehaviour, IPointerClickHandler
{
    [Header("Object components")]
    public Text txtHeroName;
    public Image heroImage;

    [Header("Other references")]
    public TownUI_Panel_RecruitHero parentPanel;
    public DB_HeroPerson dbHero;

    public void OnPointerClick(PointerEventData eventData)
    {
        parentPanel.SelectOption(this);
    }
}
