using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldUI_CC_Inventory : AUIPanel
{
    [Header("Inventory Slots")]
    public CUI_InventorySlot_Back mainHand;
    public CUI_InventorySlot_Back offHand;
    public CUI_InventorySlot_Back helmet;
    public CUI_InventorySlot_Back armor;
    public CUI_InventorySlot_Back trinket1;
    public CUI_InventorySlot_Back trinket2;
    public CUI_InventorySlot_Back trinket3;
    public CUI_InventorySlot_Back trinket4;

    [Header("Backpack Slots")]
    public CUI_InventorySlot_Back backpack1;
    public CUI_InventorySlot_Back backpack2;
    public CUI_InventorySlot_Back backpack3;
    public CUI_InventorySlot_Back backpack4;

    [Header("Buttons")]
    public Button btnClose;

    [Header("Other")]
    public Image invSlotDragImg;

    private bool draggingInvSlot = false;
    private CUI_InventorySlot_Front invSlotDragged = null;

    public void InvSlotDrag(CUI_InventorySlot_Front invSlotFront)
    {
        draggingInvSlot = true;
        invSlotDragged = invSlotFront;

        invSlotDragImg.transform.position = InputManager.Instance.mouseScreenPos;
    }

    public void InvSlotDragEnd(CUI_InventorySlot_Front invSlotFront)
    {
        invSlotDragImg.transform.localPosition = Vector3.zero;
    }

    public void InvSlotDrop(CUI_InventorySlot_Back invSlotBack)
    {
        draggingInvSlot = false;
        invSlotDragged = null;
    }

    public void LoadPanel(Hero h)
    {
        mainHand.invSlot = h.inventory.mainHand;
        offHand.invSlot = h.inventory.offHand;
        helmet.invSlot = h.inventory.helmet;
        armor.invSlot = h.inventory.armor;
        trinket1.invSlot = h.inventory.trinket1;
        trinket2.invSlot = h.inventory.trinket2;
        trinket3.invSlot = h.inventory.trinket3;
        trinket4.invSlot = h.inventory.trinket4;
    }

    public void UpdatePanel(HeroCombatPiece hc)
    {

    }
}
