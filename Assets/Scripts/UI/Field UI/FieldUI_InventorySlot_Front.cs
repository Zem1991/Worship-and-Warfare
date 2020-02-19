using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FieldUI_InventorySlot_Front : AUI_DNDSlot_Front
{
    public override bool CheckSlotFilled()
    {
        FieldUI_InventorySlot_Back fuiInvSlot = slotBack as FieldUI_InventorySlot_Back;
        return fuiInvSlot.invSlot.artifact;
    }
}
