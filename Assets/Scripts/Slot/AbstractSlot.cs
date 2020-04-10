using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSlot<SlotObject> : MonoBehaviour where SlotObject : MonoBehaviour
{
    [Header("Slot data")]
    protected SlotObject slotObj;
    public bool isBeingDragged;

    public bool HasSlotObject()
    {
        return slotObj != null;
    }

    public bool HasSlotObject(SlotObject slotObject)
    {
        return HasSlotObject() && slotObj == slotObject;
        //return slotObj == slotObject;
    }

    public void ClearSlotObject()
    {
        slotObj = null;
    }

    public SlotObject GetSlotObject()
    {
        return slotObj;
    }

    public void SetSlotObject(SlotObject item)
    {
        slotObj = item;
        if (slotObj) slotObj.transform.parent = transform;
    }

    //public abstract bool CheckSlotObjectType(SlotObject slotObject);
    //public abstract bool AddSlotObject(SlotObject slotObject);
    //public abstract bool RemoveSlotObject();
}
