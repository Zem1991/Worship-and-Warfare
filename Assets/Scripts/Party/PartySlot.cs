using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartySlot : AbstractSlot<AbstractPartyElement>
{
    [Header("Party data")]
    public Party party;
    public PartyElementType partyElementType;

    public void Initialize(Party party, PartyElementType partyElementType)
    {
        this.party = party;
        this.partyElementType = partyElementType;
    }

    public override bool RemoveSlotObject()
    {
        throw new System.NotImplementedException();
    }

    public override bool CheckSlotObjectType(AbstractPartyElement sObj)
    {
        return slotObj.partyElementType == partyElementType;
    }
}
