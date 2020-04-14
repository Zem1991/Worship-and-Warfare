using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownUI_PartySlot_Front : AUI_DNDSlot_Front
{
    public override bool CheckSlotFilled()
    {
        TownUI_PartySlot tuiPartySlot = slotBack as TownUI_PartySlot;
        AbstractPartyElement slotObj = tuiPartySlot.partySlot.Get();
        Debug.Log("Slot is filled with: " + slotObj);
        return slotObj;
    }
}
