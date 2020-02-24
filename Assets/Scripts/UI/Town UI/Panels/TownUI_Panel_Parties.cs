using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownUI_Panel_Parties : AUI_PanelDragAndDrop
{
    [Header("Garrison")]
    public Text txtGarrison;
    public UI_HeroInfo garrisonHeroInfo;
    public UI_UnitsInfo garrisonUnitsInfo;

    [Header("Visitor")]
    public Text txtVisitor;
    public UI_HeroInfo visitorHeroInfo;
    public UI_UnitsInfo visitorUnitsInfo;

    public void UpdatePanel(Party visitor, Party garrison)
    {
        UpdateVisitor(visitor);
        UpdateGarrison(garrison);
    }

    private void UpdateGarrison(Party garrison)
    {
        if (garrison != null)
        {
            PartySlot hero = garrison.hero;
            PartySlot[] units = garrison.units;
            garrisonHeroInfo.RefreshInfo(hero);
            garrisonUnitsInfo.RefreshInfo(units);
        }
        else
        {
            garrisonHeroInfo.ClearInfo();
            garrisonUnitsInfo.RefreshInfo(null);
        }
    }

    private void UpdateVisitor(Party visitor)
    {
        if (visitor != null)
        {
            PartySlot hero = visitor.hero;
            PartySlot[] units = visitor.units;
            visitorHeroInfo.RefreshInfo(hero);
            visitorUnitsInfo.RefreshInfo(units);
        }
        else
        {
            visitorHeroInfo.ClearInfo();
            visitorUnitsInfo.RefreshInfo(null);
        }
    }

    public override void DNDBeginDrag(AUI_DNDSlot_Front slotFront)
    {
        TownUI_PartySlot tuiPartySlot = slotFront.slotBack as TownUI_PartySlot;

        PartySlot partySlot = tuiPartySlot.partySlot;
        partySlot.isBeingDragged = true;

        base.DNDBeginDrag(slotFront);
    }

    public override void DNDDrop(AUI_DNDSlot slot)
    {
        if (slotFrontDragged)
        {
            TownUI_PartySlot draggedSlot = slotFrontDragged.slotBack as TownUI_PartySlot;
            PartySlot actualPartySlot = draggedSlot.partySlot;

            TownUI_PartySlot tuiPartySlot = slot as TownUI_PartySlot;
            if (tuiPartySlot)
            {
                AbstractPartyElement item = actualPartySlot.slotObj;
                if (item && tuiPartySlot.partySlot.AddSlotObject(item))
                {
                    actualPartySlot.slotObj = null;
                }
            }

            actualPartySlot.isBeingDragged = false;
        }

        base.DNDDrop(slot);
    }
}
