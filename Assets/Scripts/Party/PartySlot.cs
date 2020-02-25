using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartySlot : AbstractSlot<AbstractPartyElement>
{
    [Header("Party data")]
    public Party party;
    public PartyElementType slotType;

    public void Initialize(Party party, PartyElementType slotType, int identification = 0)
    {
        this.party = party;
        this.slotType = slotType;

        switch (slotType)
        {
            case PartyElementType.HERO:
                name = "Hero slot";
                break;
            case PartyElementType.CREATURE:
                name = "Creature slot, #" + identification;
                break;
            //case PartyElementType.SIEGE_ENGINE:
            //    break;
            //default:
            //    break;
        }
    }

    public override bool CheckSlotObjectType(AbstractPartyElement slotObject)
    {
        bool typeCheck = slotType == slotObject.partyElementType;
        return typeCheck;
    }

    public override bool AddSlotObject(AbstractPartyElement slotObject)
    {
        if (!CheckSlotObjectType(slotObject)) return false;
        if (HasSlotObject(slotObject)) return false;
        if (HasSlotObject())
        {
            if (MergeUnits(slotObject)) return true;
            else return false;
        }

        slotObj = slotObject;
        slotObj.transform.parent = transform;
        return true;
    }

    public override bool RemoveSlotObject()
    {
        return false;   //TODO this later, maybe?
    }

    public bool MergeUnits(AbstractPartyElement slotObject)
    {
        Unit thisAsUnit = slotObj as Unit;
        if (thisAsUnit.partyElementType != PartyElementType.CREATURE) return false;

        if (thisAsUnit.CompareDatabaseEntry(slotObject))
        {
            Unit otherAsUnit = slotObject as Unit;
            int amount = otherAsUnit.stackStats.stack_current;

            thisAsUnit.stackStats.stack_current += amount;
            thisAsUnit.stackStats.stack_maximum += amount;

            return true;
        }
        return false;
    }
}
