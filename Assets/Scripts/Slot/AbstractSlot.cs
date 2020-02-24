using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSlot<SlotObject> : MonoBehaviour where SlotObject : MonoBehaviour
{
    [Header("Slot object data")]
    public SlotObject slotObj;
    public bool isBeingDragged;

    public bool HasSlotObject()
    {
        return slotObj != null;
    }

    public bool HasSlotObject(SlotObject slotObject)
    {
        return slotObj == slotObject;
    }

    public bool AddSlotObject(SlotObject slotObject)
    {
        if (!CheckSlotObjectType(slotObject)) return false;
        if (HasSlotObject(slotObject)) return false;
        if (HasSlotObject() && !RemoveSlotObject()) return false;

        slotObj = slotObject;
        slotObj.transform.parent = transform;
        return true;
    }

    public abstract bool RemoveSlotObject();

    public abstract bool CheckSlotObjectType(SlotObject slotObject);
}
