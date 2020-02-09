using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownManager : AbstractSingleton<TownManager>, IShowableHideable
{
    [Header("Teams")]
    public TownPiece2 townPiece;
    public PartyPiece2 visitor;
    public PartyPiece2 garrison;

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void BootTown(TownPiece2 town)
    {
        townPiece = town;
        visitor = town.visitor;
        garrison = town.garrison;

        TownUI.Instance.CreateTownBuildings(town.town.buildings);
    }
}
