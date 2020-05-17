using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldUI_Panel_TradeScreen : AbstractUIPanel
{
    [Header("Parties")]
    public FieldUI_Panel_TradeScreen_Party fuiLeftParty;
    public FieldUI_Panel_TradeScreen_Party fuiRightParty;

    [Header("Buttons")]
    public Button btnClose;

    private PartyPiece3 partyLeft;
    private PartyPiece3 partyRight;

    public void UpdatePanel(PartyPiece3 partyLeft, PartyPiece3 partyRight)
    {
        this.partyLeft = partyLeft;
        this.partyRight = partyRight;

        fuiLeftParty.UpdatePanel(partyLeft, true);
        fuiRightParty.UpdatePanel(partyRight, true);
    }

    //public override void DNDBeginDrag(AUI_DNDSlot_Front slotFront)
    //{
    //    FieldUI_InventorySlot fuiInvSlot = slotFront.slotBack as FieldUI_InventorySlot;

    //    InventorySlot invSlot = fuiInvSlot.invSlot;
    //    invSlot.isBeingDragged = true;
    //    invSlot.inventory.RecalculateStats();

    //    base.DNDBeginDrag(slotFront);
    //}

    //public override void DNDDrop(AUI_DNDSlot slot)
    //{
    //    if (slotFrontDragged)
    //    {
    //        FieldUI_InventorySlot sourceSlot = slotFrontDragged.slotBack as FieldUI_InventorySlot;
    //        InventorySlot sourceInvSlot = sourceSlot.invSlot;
    //        Inventory sourceInv = sourceInvSlot.inventory;

    //        FieldUI_InventorySlot targetSlot = slot as FieldUI_InventorySlot;
    //        InventorySlot targetInvSlot;
    //        Inventory targetInv = null;

    //        if (targetSlot)
    //        {
    //            targetInvSlot = targetSlot.invSlot;
    //            targetInv = targetInvSlot.inventory;
    //            targetInv.AddFromSlot(sourceInvSlot, targetInvSlot);
    //        }

    //        sourceInvSlot.isBeingDragged = false;
    //        sourceInv.RecalculateStats();
    //        if (targetInv && sourceInv != targetInv) targetInv.RecalculateStats();
    //    }

    //    base.DNDDrop(slot);
    //    UpdatePanel(partyLeft, partyRight);
    //}
}
