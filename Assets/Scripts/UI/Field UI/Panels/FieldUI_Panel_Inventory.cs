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

    public override void DNDBeginDrag(AUI_DNDSlot_Front slotFront)
    {
        FieldUI_InventorySlot_Back fuiInvSlot = slotFront.slotBack as FieldUI_InventorySlot_Back;

        InventorySlot invSlot = fuiInvSlot.invSlot;
        invSlot.beingDragged = true;
        invSlot.inventory.RecalculateStats();

        base.DNDBeginDrag(slotFront);
    }

    public override void DNDDrop(AUI_DNDSlot slot)
    {
        FieldUI_InventorySlot_Back fuiInvSlot = slot as FieldUI_InventorySlot_Back;

        if (slotFrontDragged)
        {
            InventorySlot actualInvSlot = fuiInvSlot.invSlot;

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
