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
    public FieldUI_InventorySlot mainHand;
    public FieldUI_InventorySlot offHand;
    public FieldUI_InventorySlot helmet;
    public FieldUI_InventorySlot armor;
    public FieldUI_InventorySlot trinket1;
    public FieldUI_InventorySlot trinket2;
    public FieldUI_InventorySlot trinket3;
    public FieldUI_InventorySlot trinket4;

    [Header("Backpack Slots")]
    public FieldUI_InventorySlot backpack1;
    public FieldUI_InventorySlot backpack2;
    public FieldUI_InventorySlot backpack3;
    public FieldUI_InventorySlot backpack4;

    [Header("Buttons")]
    public Button btnClose;

    public void UpdatePanel(PartyPiece2 p)
    {
        Hero hero = p.party.hero;
        heroInfo.RefreshInfo(p.party.hero);
        attributeInfo.RefreshInfo(p.party.hero.attributeStats);

        mainHand.UpdateSlot(this, hero.inventory.mainHand);
        offHand.UpdateSlot(this, hero.inventory.offHand);
        helmet.UpdateSlot(this, hero.inventory.helmet);
        armor.UpdateSlot(this, hero.inventory.armor);
        trinket1.UpdateSlot(this, hero.inventory.trinket1);
        trinket2.UpdateSlot(this, hero.inventory.trinket2);
        trinket3.UpdateSlot(this, hero.inventory.trinket3);
        trinket4.UpdateSlot(this, hero.inventory.trinket4);
    }

    public override void DNDBeginDrag(AUI_DNDSlot_Front slotFront)
    {
        FieldUI_InventorySlot fuiInvSlot = slotFront.slotBack as FieldUI_InventorySlot;

        InventorySlot invSlot = fuiInvSlot.invSlot;
        invSlot.beingDragged = true;
        invSlot.inventory.RecalculateStats();

        base.DNDBeginDrag(slotFront);
    }

    public override void DNDDrop(AUI_DNDSlot slot)
    {
        if (slotFrontDragged)
        {
            FieldUI_InventorySlot draggedSlot = slotFrontDragged.slotBack as FieldUI_InventorySlot;
            InventorySlot actualInvSlot = draggedSlot.invSlot;

            FieldUI_InventorySlot fuiInvSlot = slot as FieldUI_InventorySlot;
            if (fuiInvSlot)
            {
                Artifact item = actualInvSlot.artifact;
                if (item && fuiInvSlot.invSlot.AddArtifact(item))
                {
                    actualInvSlot.artifact = null;
                }
            }

            actualInvSlot.beingDragged = false;
            actualInvSlot.inventory.RecalculateStats();
        }

        base.DNDDrop(slot);
    }
}
