using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PartyInfo : AUI_PanelDragAndDrop
{
    [Header("Party Slots and Settings")]
    public TownUI_PartySlot heroSlot;
    public TownUI_PartySlot[] unitSlots;
    public bool canDragHero = true;
    public bool canDragUnits = true;

    [Header("Dynamic references")]
    public AbstractFieldPiece2 partySource;
    public TownPiece2 partySourceAsTown;
    public PartyPiece2 partySourceAsParty;
    public ITownPieceRefresh townPieceRefresh;
    public IPartyPieceRefresh partyPieceRefresh;

    public void Start()
    {
        townPieceRefresh = GetComponentInParent<ITownPieceRefresh>();
        partyPieceRefresh = GetComponentInParent<IPartyPieceRefresh>();
    }

    public void RefreshInfo(AbstractFieldPiece2 partySource)
    {
        this.partySource = partySource;
        partySourceAsTown = partySource as TownPiece2;
        partySourceAsParty = partySource as PartyPiece2;

        Party party = null;
        if (partySourceAsTown) party = partySourceAsTown.town.garrison;
        if (partySourceAsParty) party = partySourceAsParty.party;

        PartySlot hero = null;
        List<PartySlot> units = null;
        if (party)
        {
            hero = party.GetHeroSlot();
            units = party.GetUnitSlots();
        }

        heroSlot.UpdateSlot(this, hero);
        for (int i = 0; i < PartyConstants.MAX_UNITS; i++)
        {
            unitSlots[i].UpdateSlot(this, units?[i]);
        }
    }

    public override bool DNDCanDragThis(AUI_DNDSlot_Front slotFront)
    {
        TownUI_PartySlot slotBack = slotFront.slotBack as TownUI_PartySlot;

        bool result = true;
        //if (!slotBack || !slotBack.partySlot)     REVERT THIS IF NO PROGRESS OCCURS
        if (!slotBack)
        {
            result = false;
        }
        //return result;    not yet...

        if (result && slotBack.partySlot)
        {
            PartyElementType slotType = slotBack.partySlot.slotType;
            if (!canDragHero && slotType == PartyElementType.HERO) result = false;
            else if (!canDragUnits && slotType == PartyElementType.CREATURE) result = false;
        }
        return result;
    }

    public override void DNDBeginDrag(AUI_DNDSlot_Front slotFront)
    {
        TownUI_PartySlot slotBack = slotFront.slotBack as TownUI_PartySlot;

        PartySlot partySlot = slotBack.partySlot;
        partySlot.isBeingDragged = true;
        base.DNDBeginDrag(slotFront);

        //UI_PartyInfo sourcePartyDND = slotFront.slotBack.panelDND as UI_PartyInfo;
        //if (sourcePartyDND) UIRefresh(sourcePartyDND);
        ////if (partySourceAsTown) townPieceRefresh.TownPieceRefresh(partySourceAsTown);
        ////if (partySourceAsParty) partyPieceRefresh.PartyPieceRefresh(partySourceAsParty);
        UIRefresh();
    }

    public override void DNDDrop(AUI_DNDSlot_Front slotFrontDragged, AUI_DNDSlot targetSlot)
    {
        bool differentSources = false;
        UI_PartyInfo sourcePartyDND = null;

        Party sourceParty = null;
        Party targetParty = null;

        this.slotFrontDragged = slotFrontDragged;
        if (slotFrontDragged)
        {
            sourcePartyDND = slotFrontDragged.slotBack.panelDND as UI_PartyInfo;
            PreventNoTargetParty(sourcePartyDND);

            TownUI_PartySlot sourceUISlot = slotFrontDragged.slotBack as TownUI_PartySlot;
            PartySlot sourcePartySlot = sourceUISlot.partySlot;
            sourceParty = sourcePartySlot.party;

            TownUI_PartySlot targetUISlot = targetSlot as TownUI_PartySlot;
            PartySlot targetPartySlot;

            if (targetUISlot)
            {
                targetPartySlot = targetUISlot.partySlot;
                targetParty = targetPartySlot.party;
                targetParty.Swap(sourcePartySlot, targetPartySlot);
            }

            sourcePartySlot.isBeingDragged = false;
            //sourceParty.RecalculateStats();

            differentSources = targetParty && sourceParty != targetParty;
            //if (differentSources) targetParty.RecalculateStats();
            PreventEmptySourceParty(sourcePartyDND);
        }
        base.DNDDrop(slotFrontDragged, targetSlot);

        UIRefresh();
        if (differentSources) sourcePartyDND.DNDForceDrop();
    }

    private void PreventNoTargetParty(UI_PartyInfo sourcePartyDND)
    {
        if (!partySource)
        {
            FieldPieceHandler fph = FieldManager.Instance.pieceHandler;

            TownPiece2 sourcePartySourceAsTown = sourcePartyDND.partySourceAsTown;
            PartyPiece2 sourcePartySourceAsParty = sourcePartyDND.partySourceAsParty;

            PartyPiece2 newParty = null;
            if (sourcePartySourceAsTown)
            {
                Vector2Int mapPosition = sourcePartySourceAsTown.currentTile.posId;
                mapPosition.y++;
                newParty = fph.CreateParty(mapPosition, new PartyData(), sourcePartySourceAsTown.pieceOwner.GetOwner());
                //partySourceAsTown.visitorPiece = newParty;
                sourcePartySourceAsTown.visitorPiece = newParty;
            }
            else if (sourcePartySourceAsParty)
            {
                Vector2Int mapPosition = sourcePartySourceAsTown.currentTile.posId; //TODO use partySourceAsTown last tile or get any empty tile around it, insead of partySourceAsTown.currentTile
                newParty = fph.CreateParty(mapPosition, new PartyData(), sourcePartySourceAsParty.pieceOwner.GetOwner());
            }

            fph.partyPieces.Add(newParty);
            RefreshInfo(newParty);
        }
    }

    private void PreventEmptySourceParty(UI_PartyInfo sourcePartyDND)
    {
        TownPiece2 sourcePartySourceAsTown = sourcePartyDND.partySourceAsTown;
        PartyPiece2 sourcePartySourceAsParty = sourcePartyDND.partySourceAsParty;

        if (sourcePartySourceAsTown)
        {
            if (!sourcePartySourceAsTown.town.garrison.GetMostRelevant())
            {
                //Town garrison parties can be left empty inside their towns.
            }
        }
        else if (sourcePartySourceAsParty)
        {
            if (!sourcePartySourceAsParty.party.GetMostRelevant())
            {
                FieldManager.Instance.RemoveParty(sourcePartySourceAsParty);
                //partySourceAsTown.visitorPiece = null;
                sourcePartyDND.RefreshInfo(null);
            }
        }
    }

    //private void UIRefresh(UI_PartyInfo sourcePartyDND)
    private void UIRefresh()
    {
        if (partySourceAsTown)
        {
            if (townPieceRefresh != null) townPieceRefresh.TownPieceRefresh(partySourceAsTown);
        }
        if (partySourceAsParty)
        {
            if (partyPieceRefresh != null) partyPieceRefresh.PartyPieceRefresh(partySourceAsParty);
            //else if (sourcePartyDND.partySourceAsTown) sourcePartyDND.townPieceRefresh.TownPieceRefresh(sourcePartyDND.partySourceAsTown);
        }
    }
}
