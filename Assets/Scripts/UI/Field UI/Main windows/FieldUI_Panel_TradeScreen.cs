using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldUI_Panel_TradeScreen : AUI_PanelDragAndDrop
{
    [Header("Parties")]
    public FieldUI_Panel_TradeScreen_Party leftParty;
    public FieldUI_Panel_TradeScreen_Party rightParty;

    [Header("Buttons")]
    public Button btnClose;

    public void UpdatePanel(PartyPiece2 left, PartyPiece2 right)
    {
        leftParty.UpdatePanel(left);
        rightParty.UpdatePanel(right);
    }

    public override void DNDBeginDrag(AUI_DNDSlot_Front slotFront)
    {
        FieldUI_InventorySlot fuiInvSlot = slotFront.slotBack as FieldUI_InventorySlot;

        InventorySlot invSlot = fuiInvSlot.invSlot;
        invSlot.isBeingDragged = true;
        invSlot.inventory.RecalculateStats();

        base.DNDBeginDrag(slotFront);
    }

    public override void DNDDrop(AUI_DNDSlot slot)
    {
        if (slotFrontDragged)
        {
            FieldUI_InventorySlot draggedSlot = slotFrontDragged.slotBack as FieldUI_InventorySlot;
            InventorySlot sourceInvSlot = draggedSlot.invSlot;
            Inventory sourceInv = sourceInvSlot.inventory;

            FieldUI_InventorySlot targetInvSlot = slot as FieldUI_InventorySlot;
            Inventory targetInv = null;

            if (targetInvSlot)
            {
                targetInv = targetInvSlot.invSlot.inventory;
                Artifact item = sourceInvSlot.slotObj;
                if (item && targetInvSlot.invSlot.AddSlotObject(item))
                {
                    sourceInvSlot.slotObj = null;
                }
            }

            sourceInvSlot.isBeingDragged = false;
            sourceInv.RecalculateStats();
            if (targetInv && sourceInv != targetInv) targetInv.RecalculateStats();
        }

        base.DNDDrop(slot);
    }
}
