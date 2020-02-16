using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TownUI_PartySlot_Front : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Edit mode stuff")]
    public Image slotImg;

    [Header("Runtime stuff")]
    public TownUI_PartySlot_Back slotBack;

    private TownUI_Panel_Parties partiesPanel;

    void Awake()
    {
        slotBack = GetComponentInParent<TownUI_PartySlot_Back>();
        partiesPanel = GetComponentInParent<TownUI_Panel_Parties>();
    }

    public void ChangeImage(Sprite img)
    {
        Color color = Color.white;
        if (!img) color.a = 0;
        else if (slotBack.invSlot.beingDragged) color.a = 0.5F;
        slotImg.color = color;
        slotImg.sprite = img;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (slotBack.invSlot.artifact)
        {
            ChangeImage(slotImg.sprite);
            partiesPanel.InvSlotBeginDrag(this);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        partiesPanel.InvSlotDrag(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        partiesPanel.InvSlotEndDrag(this);
    }
}
