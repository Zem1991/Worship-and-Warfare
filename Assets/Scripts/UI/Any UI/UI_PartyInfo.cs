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

    public void RefreshInfo(Party party)
    {
        PartySlot hero = party.GetHeroSlot();
        List<PartySlot> units = party.GetUnitSlots();

        heroSlot.UpdateSlot(this, hero);
        for (int i = 0; i < PartyConstants.MAX_UNITS; i++)
        {
            unitSlots[i].UpdateSlot(this, units[i]);
        }
    }

    public override bool DNDCanDragThis(AUI_DNDSlot_Front slotFront)
    {
        TownUI_PartySlot slotBack = slotFront.slotBack as TownUI_PartySlot;

        bool result = true;
        if (!slotBack)
        {
            result = false;
        }
        else
        {
            PartyElementType slotType = slotBack.partySlot.slotType;
            if (!canDragHero && slotType == PartyElementType.HERO)
            {
                result = false;
            }
            if (!canDragUnits && slotType == PartyElementType.CREATURE)
            {
                result = false;
            }
        }
        return result;
    }

    public override void DNDBeginDrag(AUI_DNDSlot_Front slotFront)
    {
        TownUI_PartySlot slotBack = slotFront.slotBack as TownUI_PartySlot;

        PartySlot partySlot = slotBack.partySlot;
        partySlot.isBeingDragged = true;
        base.DNDBeginDrag(slotFront);
    }

    public override void DNDDrop(AUI_DNDSlot_Front slotFrontDragged, AUI_DNDSlot targetSlot)
    {
        this.slotFrontDragged = slotFrontDragged;
        if (slotFrontDragged)
        {
            TownUI_PartySlot draggedSlot = slotFrontDragged.slotBack as TownUI_PartySlot;
            PartySlot actualPartySlot = draggedSlot.partySlot;

            TownUI_PartySlot tuiPartySlot = targetSlot as TownUI_PartySlot;
            if (tuiPartySlot)
            {
                AbstractPartyElement item = actualPartySlot.Get();
                if (item)
                {
                    tuiPartySlot.partySlot.Set(item);
                    actualPartySlot.Set(null);
                }
            }

            actualPartySlot.isBeingDragged = false;
        }

        base.DNDDrop(slotFrontDragged, targetSlot);
    }
}
