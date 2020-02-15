using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownManager : AbstractSingleton<TownManager>, IShowableHideable
{
    [Header("Highlight Colors")]
    public Color highlightBuilt = Color.green;
    public Color highlightAvailable = Color.yellow;
    public Color highlightDenied = Color.red;

    [Header("References")]
    public TownPiece2 townPiece;
    //public PartyPiece2 visitor;
    //public PartyPiece2 garrison;

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
        //visitor = town.visitor;
        //garrison = town.garrison;

        TownUI.Instance.CreateTown();
    }

    public void BuildStructurePanel()
    {
        if (TownUI.Instance.currentWindow == TownUI.Instance.buildStructure)
        {
            TownUI.Instance.BuildStructureHide();
            return;
        }
        TownUI.Instance.CloseCurrentWindow();
        TownUI.Instance.BuildStructureShow();
    }

    public void RecruitHeroPanel()
    {
        if (TownUI.Instance.currentWindow == TownUI.Instance.recruitHero)
        {
            TownUI.Instance.RecruitHeroHide();
            return;
        }
        TownUI.Instance.CloseCurrentWindow();
        TownUI.Instance.RecruitHeroShow();
    }

    public void RecruitCreaturePanel()
    {
        if (TownUI.Instance.currentWindow == TownUI.Instance.recruitCreature)
        {
            TownUI.Instance.RecruitCreatureHide();
            return;
        }
        TownUI.Instance.CloseCurrentWindow();
        TownUI.Instance.RecruitCreatureShow();
    }

    public void ExitTown()
    {
        if (TownUI.Instance.currentWindow != null) TownUI.Instance.CloseCurrentWindow();

        TownSC.Instance.HideScene();
        GameManager.Instance.ChangeSchemes(GameScheme.FIELD);
        FieldSC.Instance.ShowScene();
    }
}
