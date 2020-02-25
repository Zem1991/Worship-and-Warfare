using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PartyInfo : MonoBehaviour, IShowableHideable
{
    public UI_HeroInfo hero;
    public UI_UnitsInfo units;

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void RefreshInfo(PartyPiece2 partyPiece)
    {
        RefreshInfo(partyPiece.party);
    }

    public void RefreshInfo(Party party)
    {
        hero.RefreshInfo(party.hero);
        units.RefreshInfo(party.units);
    }
}
