﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldUI_InventorySlot_Front : AUI_DNDSlot_Front
{
    public override bool CheckSlotFilled()
    {
        FieldUI_InventorySlot fuiInvSlot = slotBack as FieldUI_InventorySlot;
        Artifact slotObj = fuiInvSlot.invSlot.Get();
        Debug.Log("Slot is filled with: " + slotObj);
        return slotObj;
    }
}
