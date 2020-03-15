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
    public FieldUI_InventorySlot_Holder helmet;
    public FieldUI_InventorySlot_Holder armor;
    public FieldUI_InventorySlot_Holder trinket1;
    public FieldUI_InventorySlot_Holder trinket2;
    public FieldUI_InventorySlot_Holder trinket3;
    public FieldUI_InventorySlot_Holder trinket4;

    [Header("Backpack Slots")]
    public RectTransform backpackSlotContainer;
    public List<FieldUI_InventorySlot_Holder> backpack;

    [Header("Buttons")]
    public Button btnClose;

    private PartyPiece2 partyPiece;

    public void ClearPanel()
    {
        foreach (var item in backpack) Destroy(item.gameObject);
        backpack.Clear();
    }

    public void UpdatePanel(PartyPiece2 partyPiece)
    {
        ClearPanel();

        this.partyPiece = partyPiece;

        Hero hero = partyPiece.party.hero.slotObj as Hero;
        hero.RecalculateStats();

        heroInfo.RefreshInfo(hero);
        attributeInfo.RefreshInfo(hero.attributeStats);

        mainHand.slot.UpdateSlot(this, hero.inventory.mainHand);
        offHand.slot.UpdateSlot(this, hero.inventory.offHand);
        helmet.slot.UpdateSlot(this, hero.inventory.helmet);
        armor.slot.UpdateSlot(this, hero.inventory.armor);
        trinket1.slot.UpdateSlot(this, hero.inventory.trinket1);
        trinket2.slot.UpdateSlot(this, hero.inventory.trinket2);
        trinket3.slot.UpdateSlot(this, hero.inventory.trinket3);
        trinket4.slot.UpdateSlot(this, hero.inventory.trinket4);

        FieldUI_InventorySlot_Holder prefabSlotHolder = AllPrefabs.Instance.fuiInvSlotHolder;
        foreach (var backpackItem in hero.inventory.GetBackpackSlots(true))
        {
            FieldUI_InventorySlot_Holder newSlotHolder = Instantiate(prefabSlotHolder, backpackSlotContainer.transform);
            newSlotHolder.slot.UpdateSlot(this, backpackItem);
            backpack.Add(newSlotHolder);
        }
        //No need to add an extra empty backpack slot, because the Inventory class already does that.
    }

    public override void DNDBeginDrag(AUI_DNDSlot_Front slotFront)
    {
        FieldUI_InventorySlot fuiInvSlot = slotFront.slotBack as FieldUI_InventorySlot;

        InventorySlot invSlot = fuiInvSlot.invSlot;
        invSlot.isBeingDragged = true;
        invSlot.inventory.RecalculateStats();

        base.DNDBeginDrag(slotFront);
        UpdatePanel(partyPiece);
    }

    public override void DNDDrop(AUI_DNDSlot slot)
    {
        if (slotFrontDragged)
        {
            FieldUI_InventorySlot sourceSlot = slotFrontDragged.slotBack as FieldUI_InventorySlot;
            InventorySlot sourceInvSlot = sourceSlot.invSlot;
            Inventory sourceInv = sourceInvSlot.inventory;

            FieldUI_InventorySlot targetSlot = slot as FieldUI_InventorySlot;
            Inventory targetInv = null;

            if (targetSlot)
            {
                InventorySlot targetInvSlot = targetSlot.invSlot;
                targetInv = targetInvSlot.inventory;

                Artifact item = sourceInvSlot.slotObj;
                if (item)
                {
                    if (sourceInvSlot.SwapSlots(targetInvSlot))
                        sourceInvSlot.slotObj = null;
                    else if (targetInvSlot.AddSlotObject(item))
                        sourceInvSlot.slotObj = null;
                }
            }

            sourceInvSlot.isBeingDragged = false;
            sourceInv.RecalculateStats();
            if (targetInv && sourceInv != targetInv) targetInv.RecalculateStats();
        }

        base.DNDDrop(slot);
        UpdatePanel(partyPiece);
    }
}
