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

    [Header("Dynamic references")]
    public PartyPiece3 partyPiece;
    public IInventoryRefresh inventoryRefresh;

    public void Start()
    {
        inventoryRefresh = GetComponentInParent<IInventoryRefresh>();
    }

    public void Update()
    {
        FieldUI_InventorySlot_Front slotFront = slotFrontDragged as FieldUI_InventorySlot_Front;
        if (slotFront)
        {
            FieldUI_InventorySlot slotBack = slotFront.slotBack as FieldUI_InventorySlot;
            slotBack.UpdateSlot(this, slotBack.invSlot);
        }
    }

    public void RefreshInfo(bool refreshBackpackSlots)
    {
        RefreshInfo(partyPiece, refreshBackpackSlots);
    }

    public void RefreshInfo(PartyPiece3 partyPiece, bool refreshBackpackSlots)
    {
        this.partyPiece = partyPiece;
        RefreshEquipmentInfo(partyPiece);
        if (refreshBackpackSlots) RefreshBackpackInfo(partyPiece);
        //hero.RecalculateStats();
    }

    private void RefreshEquipmentInfo(PartyPiece3 partyPiece)
    {
        HeroUnit hero = partyPiece.party.GetHeroSlot().Get() as HeroUnit;
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

    private void RefreshBackpackInfo(PartyPiece3 partyPiece)
    {
        //ClearBackpackSlots();
        foreach (FieldUI_InventorySlot_Holder item in backpack) Destroy(item.gameObject);
        backpack.Clear();

        HeroUnit hero = partyPiece.party.GetHeroSlot().Get() as HeroUnit;
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

        inventoryRefresh.InventoryRefresh(partyPiece, false);
    }

    public override void DNDDrop(AUI_DNDSlot_Front slotFrontDragged, AUI_DNDSlot targetSlot)
    {
        bool differentSources = false;
        UI_InventoryInfo sourceInvDND = null;

        Inventory sourceInv = null;
        Inventory targetInv = null;

        this.slotFrontDragged = slotFrontDragged;
        if (slotFrontDragged)
        {
            sourceInvDND = slotFrontDragged.slotBack.panelDND as UI_InventoryInfo;

            FieldUI_InventorySlot sourceUISlot = slotFrontDragged.slotBack as FieldUI_InventorySlot;
            InventorySlot sourceInvSlot = sourceUISlot.invSlot;
            sourceInv = sourceInvSlot.inventory;

            FieldUI_InventorySlot targetUISlot = targetSlot as FieldUI_InventorySlot;
            InventorySlot targetInvSlot;

            if (targetUISlot)
            {
                targetInvSlot = targetUISlot.invSlot;
                targetInv = targetInvSlot.inventory;
                targetInv.Swap(sourceInvSlot, targetInvSlot);
            }

            sourceInvSlot.isBeingDragged = false;

            differentSources = targetInv && sourceInv != targetInv;
        }
        base.DNDDrop(slotFrontDragged, targetSlot);

        if (sourceInv) sourceInv.RecalculateStats();
        if (targetInv && differentSources) targetInv.RecalculateStats();

        inventoryRefresh.InventoryRefresh(partyPiece, true);
        if (differentSources) sourceInvDND.DNDForceDrop();
    }
}
