using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownUI_PartySlot_Front : AUI_DNDSlot_Front
{
    public override bool CheckSlotFilled()
    {
        Debug.LogError("NOT DONE");
        return true;
        //FieldUI_InventorySlot fuiInvSlot = slotBack as FieldUI_InventorySlot;
        //return fuiInvSlot.invSlot.artifact;
    }
}
