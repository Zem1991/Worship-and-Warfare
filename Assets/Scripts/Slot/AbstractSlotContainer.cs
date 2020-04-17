using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSlotContainer<Slot, SlotObject> : MonoBehaviour where Slot : AbstractSlot<SlotObject> where SlotObject : MonoBehaviour
{
    protected virtual AbstractSlot<SlotObject> CreateTempSlot(AbstractSlot<SlotObject> prefab, SlotObject item)
    {
        AbstractSlot<SlotObject> temp = Instantiate(prefab, transform);
        //temp.Initialize(this, item.dbData.artifactType, null);
        temp.Set(item);
        return temp;
    }

    public abstract bool Add(SlotObject item);
    public abstract bool Remove(SlotObject item);
    public abstract bool Swap(AbstractSlot<SlotObject> fromSlot, AbstractSlot<SlotObject> toSlot);
}
