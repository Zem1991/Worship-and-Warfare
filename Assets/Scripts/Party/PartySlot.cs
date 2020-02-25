using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartySlot : AbstractSlot<AbstractPartyElement>
{
    [Header("Party data")]
    public Party party;
    public PartyElementType slotType;

    public void Initialize(Party party, PartyElementType slotType)
    {
        this.party = party;
        this.slotType = slotType;
    }

    public override bool RemoveSlotObject()
    {
        throw new System.NotImplementedException();
    }

    public override bool CheckSlotObjectType(AbstractPartyElement slotObject)
    {
        bool typeCheck = slotType == slotObject.partyElementType;
        return typeCheck;
    }
}
