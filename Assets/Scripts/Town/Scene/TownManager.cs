﻿using System;
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

    public void BootTown(TownPiece2 townPiece)
    {
        this.townPiece = townPiece;
        if (townPiece.visitorPiece)
        {
            townPiece.town.visitor = townPiece.visitorPiece.party;  //AQUI CARAI
        }
        TownUI.Instance.CreateTown();
    }

    public void BuildStructurePanel()
    {
        bool closeThenReturn = TownUI.Instance.currentWindow == TownUI.Instance.buildStructure;
        TownUI.Instance.CloseCurrentWindow();
        if (closeThenReturn) return;
        TownUI.Instance.CW_BuildStructureShow();
    }

    public void RecruitHeroPanel()
    {
        bool closeThenReturn = TownUI.Instance.currentWindow == TownUI.Instance.recruitHero;
        TownUI.Instance.CloseCurrentWindow();
        if (closeThenReturn) return;
        TownUI.Instance.CW_RecruitHeroShow();
    }

    public void RecruitCreaturePanel()
    {
        bool closeThenReturn = TownUI.Instance.currentWindow == TownUI.Instance.recruitCreature;
        TownUI.Instance.CloseCurrentWindow();
        if (closeThenReturn) return;
        TownUI.Instance.CW_RecruitCreatureShow();
    }

    public void ExitTown()
    {
        if (TownUI.Instance.currentWindow != null) TownUI.Instance.CloseCurrentWindow();

        TownSC.Instance.HideScene();

        //TODO For some reason I can't change this line to another position down below (the #HERE marker). Investigate?
        GameManager.Instance.ChangeSchemes(GameScheme.FIELD);

        PartyPiece2 visitorPiece = townPiece.visitorPiece;
        Party visitor = townPiece.town.visitor;

        if (visitorPiece)
        {
            if (!visitorPiece.party.GetMostRelevant()) FieldManager.Instance.RemoveParty(visitorPiece);
        }
        else if (visitor.GetMostRelevant())
        {
            FieldManager.Instance.pieceHandler.SpawnTownVisitor(townPiece);
        }

        townPiece.visitorPiece = null;
        visitor.ClearParty();
        //#HERE

        FieldSC.Instance.ShowScene();
    }
}
