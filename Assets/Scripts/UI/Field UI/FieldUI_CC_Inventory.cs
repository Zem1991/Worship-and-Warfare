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
    public AnyUI_Draggable invSlotDraggable;

    public bool isDraggingInvSlot = false;
    public FUI_InventorySlot_Front fuiInvSlotFrontDragged = null;

    public void UpdatePanel(PartyPiece2 p)
    {
        Hero hero = p.partyHero;
        heroInfo.RefreshInfo(p.partyHero);

        mainHand.UpdateSlot(hero.inventory.mainHand);
        offHand.UpdateSlot(hero.inventory.offHand);
        helmet.UpdateSlot(hero.inventory.helmet);
        armor.UpdateSlot(hero.inventory.armor);
        trinket1.UpdateSlot(hero.inventory.trinket1);
        trinket2.UpdateSlot(hero.inventory.trinket2);
        trinket3.UpdateSlot(hero.inventory.trinket3);
        trinket4.UpdateSlot(hero.inventory.trinket4);
    }

    public void InvSlotBeginDrag(FUI_InventorySlot_Front invSlotFront)
    {
        InventorySlot invSlot = invSlotFront.invSlotBack.invSlot;
        invSlot.beingDragged = true;
        invSlot.inventory.RecalculateParameters();

        isDraggingInvSlot = true;
        fuiInvSlotFrontDragged = invSlotFront;
    }

    public void InvSlotDrag(FUI_InventorySlot_Front invSlotFront)
    {
        invSlotDraggable.Drag(invSlotFront.slotImg.sprite);
    }

    public void InvSlotDrop(FUI_InventorySlot_Back invSlotBack)
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
            actualInvSlot.inventory.RecalculateParameters();
        }

        isDraggingInvSlot = false;
        fuiInvSlotFrontDragged = null;
    }

    public void InvSlotEndDrag(FUI_InventorySlot_Front invSlotFront)
    {
        if (isDraggingInvSlot) InvSlotDrop(null);
        invSlotDraggable.EndDrag();
    }
}
