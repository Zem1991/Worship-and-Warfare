using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InventoryInfo : AUI_PanelDragAndDrop
{
    [Header("Inventory Slots")]
    public FieldUI_InventorySlot_Holder mainHand;
    public FieldUI_InventorySlot_Holder offHand;
    public FieldUI_InventorySlot_Holder head;
    public FieldUI_InventorySlot_Holder torso;
    public FieldUI_InventorySlot_Holder trinket1;
    public FieldUI_InventorySlot_Holder trinket2;
    public FieldUI_InventorySlot_Holder trinket3;
    public FieldUI_InventorySlot_Holder trinket4;

    [Header("Backpack Slots")]
    public RectTransform backpackSlotContainer;
    public List<FieldUI_InventorySlot_Holder> backpack = new List<FieldUI_InventorySlot_Holder>();

    private IInventoryPanel inventoryPanel;
    private PartyPiece2 partyPiece;

    public void Start()
    {
        inventoryPanel = GetComponentInParent<IInventoryPanel>();
    }

    public void RefreshInfo(PartyPiece2 partyPiece, bool refreshBackpackSlots)
    {
        this.partyPiece = partyPiece;
        RefreshEquipmentInfo(partyPiece);
        if (refreshBackpackSlots) RefreshBackpackInfo(partyPiece);
        //hero.RecalculateStats();
    }

    private void RefreshEquipmentInfo(PartyPiece2 partyPiece)
    {
        Hero hero = partyPiece.party.GetHeroSlot().Get() as Hero;
        Inventory inv = hero.inventory;
        mainHand.slot.UpdateSlot(this, inv.GetEquipmentSlot(ArtifactType.MAIN_HAND));
        offHand.slot.UpdateSlot(this, inv.GetEquipmentSlot(ArtifactType.OFF_HAND));
        head.slot.UpdateSlot(this, inv.GetEquipmentSlot(ArtifactType.HEAD));
        torso.slot.UpdateSlot(this, inv.GetEquipmentSlot(ArtifactType.TORSO));
        trinket1.slot.UpdateSlot(this, inv.GetEquipmentSlot(ArtifactType.TRINKET, 1));
        trinket2.slot.UpdateSlot(this, inv.GetEquipmentSlot(ArtifactType.TRINKET, 2));
        trinket3.slot.UpdateSlot(this, inv.GetEquipmentSlot(ArtifactType.TRINKET, 3));
        trinket4.slot.UpdateSlot(this, inv.GetEquipmentSlot(ArtifactType.TRINKET, 4));
    }

    private void RefreshBackpackInfo(PartyPiece2 partyPiece)
    {
        //ClearBackpackSlots();
        foreach (FieldUI_InventorySlot_Holder item in backpack) Destroy(item.gameObject);
        backpack.Clear();

        Hero hero = partyPiece.party.GetHeroSlot().Get() as Hero;
        Inventory inv = hero.inventory;
        foreach (InventorySlot backpackSlot in inv.GetBackpackSlots(true))
        {
            FieldUI_InventorySlot_Holder prefabSlotHolder = AllPrefabs.Instance.fuiInvSlotHolder;
            FieldUI_InventorySlot_Holder newSlotHolder = Instantiate(prefabSlotHolder, backpackSlotContainer.transform);
            newSlotHolder.slot.UpdateSlot(this, backpackSlot);
            backpack.Add(newSlotHolder);
        }
    }

    public override bool DNDCanDragThis(AUI_DNDSlot_Front slotFront)
    {
        FieldUI_InventorySlot slotBack = slotFront.slotBack as FieldUI_InventorySlot;

        bool result = true;
        //if (!slotBack || !slotBack.invSlot)   REVERT THIS IF NO PROGRESS OCCURS
        if (!slotBack)
        {
            result = false;
        }
        return result;
    }

    public override void DNDBeginDrag(AUI_DNDSlot_Front slotFront)
    {
        FieldUI_InventorySlot fuiInvSlot = slotFront.slotBack as FieldUI_InventorySlot;

        InventorySlot invSlot = fuiInvSlot.invSlot;
        invSlot.isBeingDragged = true;
        invSlot.inventory.RecalculateStats();
        base.DNDBeginDrag(slotFront);

        inventoryPanel.CallUpdatePanel(partyPiece, false);
    }

    public override void DNDDrop(AUI_DNDSlot_Front slotFrontDragged, AUI_DNDSlot targetSlot)
    {
        this.slotFrontDragged = slotFrontDragged;
        if (slotFrontDragged)
        {
            FieldUI_InventorySlot sourceUISlot = slotFrontDragged.slotBack as FieldUI_InventorySlot;
            InventorySlot sourceInvSlot = sourceUISlot.invSlot;
            Inventory sourceInv = sourceInvSlot.inventory;

            FieldUI_InventorySlot targetUISlot = targetSlot as FieldUI_InventorySlot;
            InventorySlot targetInvSlot;
            Inventory targetInv = null;

            if (targetUISlot)
            {
                targetInvSlot = targetUISlot.invSlot;
                targetInv = targetInvSlot.inventory;
                targetInv.Swap(sourceInvSlot, targetInvSlot);
            }

            sourceInvSlot.isBeingDragged = false;
            sourceInv.RecalculateStats();
            if (targetInv && sourceInv != targetInv) targetInv.RecalculateStats();
        }

        base.DNDDrop(slotFrontDragged, targetSlot);
        inventoryPanel.CallUpdatePanel(partyPiece, true);
    }
}
