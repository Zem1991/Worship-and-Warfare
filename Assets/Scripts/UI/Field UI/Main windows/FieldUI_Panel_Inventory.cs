using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldUI_Panel_Inventory : AUI_PanelDragAndDrop
{
    [Header("Hero info")]
    public UI_HeroInfo heroInfo;
    public UI_AttributeInfo attributeInfo;

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

    [Header("Buttons")]
    public Button btnClose;

    private PartyPiece2 partyPiece;

    private void ClearBackpackSlots()
    {
        foreach (FieldUI_InventorySlot_Holder item in backpack) Destroy(item.gameObject);
        backpack.Clear();
    }

    public void UpdatePanel(PartyPiece2 partyPiece, bool refreshBackpackSlots)
    {
        this.partyPiece = partyPiece;

        Hero hero = partyPiece.party.hero.slotObj as Hero;
        Inventory inv = hero.inventory;
        mainHand.slot.UpdateSlot(this, inv.GetEquipmentSlot(ArtifactType.MAIN_HAND));
        offHand.slot.UpdateSlot(this, inv.GetEquipmentSlot(ArtifactType.OFF_HAND));
        head.slot.UpdateSlot(this, inv.GetEquipmentSlot(ArtifactType.HEAD));
        torso.slot.UpdateSlot(this, inv.GetEquipmentSlot(ArtifactType.TORSO));
        trinket1.slot.UpdateSlot(this, inv.GetEquipmentSlot(ArtifactType.TRINKET, 1));
        trinket2.slot.UpdateSlot(this, inv.GetEquipmentSlot(ArtifactType.TRINKET, 2));
        trinket3.slot.UpdateSlot(this, inv.GetEquipmentSlot(ArtifactType.TRINKET, 3));
        trinket4.slot.UpdateSlot(this, inv.GetEquipmentSlot(ArtifactType.TRINKET, 4));

        if (refreshBackpackSlots)
        {
            ClearBackpackSlots();
            foreach (InventorySlot backpackSlot in inv.GetBackpackSlots(true))
            {
                FieldUI_InventorySlot_Holder prefabSlotHolder = AllPrefabs.Instance.fuiInvSlotHolder;
                FieldUI_InventorySlot_Holder newSlotHolder = Instantiate(prefabSlotHolder, backpackSlotContainer.transform);
                newSlotHolder.slot.UpdateSlot(this, backpackSlot);
                backpack.Add(newSlotHolder);
            }
            //No need to add an extra empty backpack slot, because the Inventory class already does that.
        }

        hero.RecalculateStats();
        heroInfo.RefreshInfo(hero);
        attributeInfo.RefreshInfo(hero.attributeStats);
    }

    public override void DNDBeginDrag(AUI_DNDSlot_Front slotFront)
    {
        FieldUI_InventorySlot fuiInvSlot = slotFront.slotBack as FieldUI_InventorySlot;

        InventorySlot invSlot = fuiInvSlot.invSlot;
        invSlot.isBeingDragged = true;
        invSlot.inventory.RecalculateStats();

        base.DNDBeginDrag(slotFront);
        UpdatePanel(partyPiece, false);
    }

    public override void DNDDrop(AUI_DNDSlot slot)
    {
        if (slotFrontDragged)
        {
            FieldUI_InventorySlot sourceSlot = slotFrontDragged.slotBack as FieldUI_InventorySlot;
            InventorySlot sourceInvSlot = sourceSlot.invSlot;
            Inventory sourceInv = sourceInvSlot.inventory;

            FieldUI_InventorySlot targetSlot = slot as FieldUI_InventorySlot;
            InventorySlot targetInvSlot;
            Inventory targetInv = null;

            if (targetSlot)
            {
                targetInvSlot = targetSlot.invSlot;
                targetInv = targetInvSlot.inventory;
                targetInv.AddFromSlot(sourceInvSlot, targetInvSlot);
            }

            sourceInvSlot.isBeingDragged = false;
            sourceInv.RecalculateStats();
            if (targetInv && sourceInv != targetInv) targetInv.RecalculateStats();
        }

        base.DNDDrop(slot);
        UpdatePanel(partyPiece, true);
    }
}
