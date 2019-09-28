using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldUI_CC_Inventory : AUIPanel
{
    [Header("Hero info")]
    public AnyUI_HeroInfo heroInfo;

    [Header("Inventory Slots")]
    public FUI_InventorySlot_Back mainHand;
    public FUI_InventorySlot_Back offHand;
    public FUI_InventorySlot_Back helmet;
    public FUI_InventorySlot_Back armor;
    public FUI_InventorySlot_Back trinket1;
    public FUI_InventorySlot_Back trinket2;
    public FUI_InventorySlot_Back trinket3;
    public FUI_InventorySlot_Back trinket4;

    [Header("Backpack Slots")]
    public FUI_InventorySlot_Back backpack1;
    public FUI_InventorySlot_Back backpack2;
    public FUI_InventorySlot_Back backpack3;
    public FUI_InventorySlot_Back backpack4;

    [Header("Buttons")]
    public Button btnClose;

    [Header("Other")]
    public Image invSlotDragImg;

    private bool draggingInvSlot = false;
    private FUI_InventorySlot_Front invSlotDragged = null;

    public void UpdatePanel(FieldPiece p)
    {
        Hero hero = p.hero;
        heroInfo.UpdatePanel(p.hero);

        mainHand.invSlot = hero.inventory.mainHand;
        offHand.invSlot = hero.inventory.offHand;
        helmet.invSlot = hero.inventory.helmet;
        armor.invSlot = hero.inventory.armor;
        trinket1.invSlot = hero.inventory.trinket1;
        trinket2.invSlot = hero.inventory.trinket2;
        trinket3.invSlot = hero.inventory.trinket3;
        trinket4.invSlot = hero.inventory.trinket4;
    }

    public void InvSlotDrag(FUI_InventorySlot_Front invSlotFront)
    {
        draggingInvSlot = true;
        invSlotDragged = invSlotFront;

        invSlotDragImg.transform.position = InputManager.Instance.mouseScreenPos;
    }

    public void InvSlotDragEnd(FUI_InventorySlot_Front invSlotFront)
    {
        invSlotDragImg.transform.localPosition = Vector3.zero;
    }

    public void InvSlotDrop(FUI_InventorySlot_Back invSlotBack)
    {
        draggingInvSlot = false;
        invSlotDragged = null;
    }
}
