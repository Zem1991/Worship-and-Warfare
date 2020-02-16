using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldUI_Panel_Inventory : AUIPanel
{
    [Header("Hero info")]
    public UI_HeroInfo heroInfo;
    public UI_AttributeInfo attributeInfo;

    [Header("Inventory Slots")]
    public FieldUI_InventorySlot_Back mainHand;
    public FieldUI_InventorySlot_Back offHand;
    public FieldUI_InventorySlot_Back helmet;
    public FieldUI_InventorySlot_Back armor;
    public FieldUI_InventorySlot_Back trinket1;
    public FieldUI_InventorySlot_Back trinket2;
    public FieldUI_InventorySlot_Back trinket3;
    public FieldUI_InventorySlot_Back trinket4;

    [Header("Backpack Slots")]
    public FieldUI_InventorySlot_Back backpack1;
    public FieldUI_InventorySlot_Back backpack2;
    public FieldUI_InventorySlot_Back backpack3;
    public FieldUI_InventorySlot_Back backpack4;

    [Header("Buttons")]
    public Button btnClose;

    [Header("Draggable element handling")]
    public UI_DraggableElement draggableElement;
    public bool isDraggingElement = false;
    public FieldUI_InventorySlot_Front fuiInvSlotFrontDragged = null;

    public void UpdatePanel(PartyPiece2 p)
    {
        Hero hero = p.party.hero;
        heroInfo.RefreshInfo(p.party.hero);
        attributeInfo.RefreshInfo(p.party.hero.attributeStats);

        mainHand.UpdateSlot(hero.inventory.mainHand);
        offHand.UpdateSlot(hero.inventory.offHand);
        helmet.UpdateSlot(hero.inventory.helmet);
        armor.UpdateSlot(hero.inventory.armor);
        trinket1.UpdateSlot(hero.inventory.trinket1);
        trinket2.UpdateSlot(hero.inventory.trinket2);
        trinket3.UpdateSlot(hero.inventory.trinket3);
        trinket4.UpdateSlot(hero.inventory.trinket4);
    }

    public void InvSlotBeginDrag(FieldUI_InventorySlot_Front invSlotFront)
    {
        InventorySlot invSlot = invSlotFront.invSlotBack.invSlot;
        invSlot.beingDragged = true;
        invSlot.inventory.RecalculateStats();

        isDraggingElement = true;
        fuiInvSlotFrontDragged = invSlotFront;
    }

    public void InvSlotDrag(FieldUI_InventorySlot_Front invSlotFront)
    {
        draggableElement.Drag(invSlotFront.slotImg.sprite);
    }

    public void InvSlotDrop(FieldUI_InventorySlot_Back invSlotBack)
    {
        if (fuiInvSlotFrontDragged)
        {
            InventorySlot actualInvSlot = fuiInvSlotFrontDragged.invSlotBack.invSlot;

            if (invSlotBack)
            {
                Artifact item = actualInvSlot.artifact;
                if (item && invSlotBack.invSlot.AddArtifact(item))
                {
                    actualInvSlot.artifact = null;
                }
            }

            actualInvSlot.beingDragged = false;
            actualInvSlot.inventory.RecalculateStats();
        }

        isDraggingElement = false;
        fuiInvSlotFrontDragged = null;
    }

    public void InvSlotEndDrag(FieldUI_InventorySlot_Front invSlotFront)
    {
        if (isDraggingElement) InvSlotDrop(null);
        draggableElement.EndDrag();
    }
}
