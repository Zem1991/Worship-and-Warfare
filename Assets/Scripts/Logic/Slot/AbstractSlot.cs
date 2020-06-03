using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSlot<SlotObject> : MonoBehaviour where SlotObject : MonoBehaviour
{
    [Header("Slot data")]
    [SerializeField] protected SlotObject slotObj;
    public bool isBeingDragged;

    public bool Has()
    {
        return slotObj != null;
    }

    public bool Has(SlotObject slotObject)
    {
        return Has() && slotObj == slotObject;
    }

    public void Clear()
    {
        Set(null);
    }

    public SlotObject Get()
    {
        return slotObj;
    }

    public void Set(SlotObject item)
    {
        slotObj = item;
        if (slotObj) slotObj.transform.parent = transform;
    }

    //public abstract bool CheckSlotObjectType(SlotObject slotObject);
    //public abstract bool AddSlotObject(SlotObject slotObject);
    //public abstract bool RemoveSlotObject();
}
