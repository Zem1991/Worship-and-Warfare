using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownUI_Panel_Parties : AUIPanel
{

    [Header("Visitor")]
    public Text txtVisitor;
    public UI_HeroInfo visitorHeroInfo;
    public UI_UnitsInfo visitorUnitsInfo;

    [Header("Garrison")]
    public Text txtGarrison;
    public UI_HeroInfo garrisonHeroInfo;
    public UI_UnitsInfo garrisonUnitsInfo;

    [Header("Draggable element handling")]
    public UI_DraggableElement draggableElement;
    public bool isDraggingElement = false;
    public TownUI_PartySlot_Front fuiInvSlotFrontDragged = null;

    public void UpdatePanel(Party visitor, Party garrison)
    {
        UpdateVisitor(visitor);
        UpdateGarrison(garrison);
    }

    private void UpdateVisitor(Party visitor)
    {
        if (visitor != null)
        {
            visitorHeroInfo.RefreshInfo(visitor.hero);
            visitorUnitsInfo.RefreshInfo(visitor.units);
        }
        else
        {
            visitorHeroInfo.RefreshInfo(null);
            visitorUnitsInfo.RefreshInfo(null);
        }
    }

    private void UpdateGarrison(Party garrison)
    {
        if (garrison != null)
        {
            garrisonHeroInfo.RefreshInfo(garrison.hero);
            garrisonUnitsInfo.RefreshInfo(garrison.units);
        }
        else
        {
            garrisonHeroInfo.RefreshInfo(null);
            garrisonUnitsInfo.RefreshInfo(null);
        }
    }

    public void InvSlotBeginDrag(TownUI_PartySlot_Front slotFront)
    {
        InventorySlot invSlot = slotFront.slotBack.invSlot;
        invSlot.beingDragged = true;
        invSlot.inventory.RecalculateStats();

        isDraggingElement = true;
        fuiInvSlotFrontDragged = slotFront;
    }

    public void InvSlotDrag(TownUI_PartySlot_Front slotFront)
    {
        draggableElement.Drag(slotFront.slotImg.sprite);
    }

    public void InvSlotDrop(TownUI_PartySlot_Back slotBack)
    {
        if (fuiInvSlotFrontDragged)
        {
            InventorySlot actualInvSlot = fuiInvSlotFrontDragged.slotBack.invSlot;

            if (slotBack)
            {
                Artifact item = actualInvSlot.artifact;
                if (item && slotBack.invSlot.AddArtifact(item))
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

    public void InvSlotEndDrag(TownUI_PartySlot_Front slotFront)
    {
        if (isDraggingElement) InvSlotDrop(null);
        draggableElement.EndDrag();
    }
}
