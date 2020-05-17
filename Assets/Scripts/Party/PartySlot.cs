using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartySlot : AbstractSlot<AbstractUnit>
{
    [Header("Party data")]
    public Party party;
    public UnitType slotType;

    public void Initialize(Party party, UnitType slotType, int identification = 0)
    {
        this.party = party;
        this.slotType = slotType;
        name = slotType.ToString() + " slot";

        switch (slotType)
        {
            case UnitType.HERO:
                name = "Hero slot";
                break;
            case UnitType.CREATURE:
                name = "Creature slot, #" + identification;
                break;
            //case PartyElementType.SIEGE_ENGINE:
            //    break;
            //default:
            //    break;
        }
    }

    //public bool CheckSlotObjectType(AbstractPartyElement slotObject)
    //{
    //    bool typeCheck = slotType == slotObject.partyElementType;
    //    return typeCheck;
    //}

    //public bool AddSlotObject(AbstractPartyElement slotObject)
    //{
    //    if (!CheckSlotObjectType(slotObject)) return false;
    //    if (HasSlotObject(slotObject)) return false;
    //    if (HasSlotObject())
    //    {
    //        if (MergeUnits(slotObject)) return true;
    //        else return false;
    //    }

    //    slotObj = slotObject;
    //    slotObj.transform.parent = transform;
    //    return true;
    //}

    //public bool RemoveSlotObject()
    //{
    //    return false;   //TODO this later, maybe?
    //}

    //public bool MergeUnits(AbstractPartyElement slotObject)
    //{
    //    Unit thisAsUnit = slotObj as Unit;
    //    if (thisAsUnit.partyElementType != PartyElementType.CREATURE) return false;

    //    if (thisAsUnit.CompareDatabaseEntry(slotObject))
    //    {
    //        Unit otherAsUnit = slotObject as Unit;
    //        int amount = otherAsUnit.stackStats.stack_current;

    //        thisAsUnit.stackStats.stack_current += amount;
    //        thisAsUnit.stackStats.stack_maximum += amount;

    //        return true;
    //    }
    //    return false;
    //}
}
