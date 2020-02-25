using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownUI_PartySlot_Front : AUI_DNDSlot_Front
{
    public override bool CheckSlotFilled()
    {
        TownUI_PartySlot tuiPartySlot = slotBack as TownUI_PartySlot;
        return tuiPartySlot.partySlot.slotObj;
    }
}
