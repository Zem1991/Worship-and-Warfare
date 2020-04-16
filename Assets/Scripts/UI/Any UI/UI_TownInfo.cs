using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TownInfo : MonoBehaviour, IShowableHideable
{
    public Image townPortrait;
    public Text txtTownName;
    public UI_PartyInfo garrisonInfo;

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
        if (townPortrait) townPortrait.sprite = null;
        if (txtTownName) txtTownName.text = "--";
    }

    public void RefreshInfo(TownPiece2 town)
    {
        ClearInfo();
        if (town == null) return;

        //if (townPortrait) townPortrait.sprite = town.dbData.profilePicture;   //TODO town portrait
        if (txtTownName) txtTownName.text = town.town.townName;
        if (garrisonInfo) garrisonInfo.RefreshInfo(town);
    }
}
