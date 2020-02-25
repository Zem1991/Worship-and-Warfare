using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownUI_Panel_Parties : AUI_PanelDragAndDrop
{
    [Header("Garrison")]
    public Text txtGarrison;
    public TownUI_PartySlot garrisonHero;
    public TownUI_PartySlot[] garrisonUnits;

    [Header("Visitor")]
    public Text txtVisitor;
    public TownUI_PartySlot visitorHero;
    public TownUI_PartySlot[] visitorUnits;

    public void UpdatePanel(Party garrison, Party visitor)
    {
        UpdateParty(garrison, garrisonHero, garrisonUnits);
        UpdateParty(visitor, visitorHero, visitorUnits);
    }

    private void UpdateParty(Party party, TownUI_PartySlot heroSlot, TownUI_PartySlot[] unitSlots)
    {
        if (party != null)
        {
            PartySlot hero = party.hero;
            PartySlot[] units = party.units;

            heroSlot.UpdateSlot(this, hero);
            for (int i = 0; i < PartyConstants.MAX_UNITS; i++)
            {
                unitSlots[i].UpdateSlot(this, units[i]);
            }
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
