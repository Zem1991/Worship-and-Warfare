using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSlotContainer<Slot, SlotObject> : MonoBehaviour where Slot : AbstractSlot<SlotObject> where SlotObject : MonoBehaviour
{
    public abstract bool AddSlotObject(SlotObject item);

    public virtual bool AddSlotObjectToSlot(AbstractSlot<SlotObject> slot, SlotObject item)
    {
        bool result = slot.AddSlotObject(item);
        return result;
    }

    public virtual bool RemoveSlotObjectFromSlot(AbstractSlot<SlotObject> slot)
    {
        bool result = slot.RemoveSlotObject();
        return result;
    }
}
