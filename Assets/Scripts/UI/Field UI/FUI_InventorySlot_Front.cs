using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FUI_InventorySlot_Front : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Edit mode stuff")]
    public Image slotImg;

    [Header("Runtime stuff")]
    public FUI_InventorySlot_Back invSlotBack;

    private FieldUI_Panel_Inventory invWindow;

    void Awake()
    {
        invSlotBack = GetComponentInParent<FUI_InventorySlot_Back>();
        invWindow = GetComponentInParent<FieldUI_Panel_Inventory>();
    }

    public void ChangeImage(Sprite img)
    {
        Color color = Color.white;
        if (!img) color.a = 0;
        else if (invSlotBack.invSlot.beingDragged) color.a = 0.5F;
        slotImg.color = color;
        slotImg.sprite = img;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (invSlotBack.invSlot.artifact)
        {
            ChangeImage(slotImg.sprite);
            invWindow.InvSlotBeginDrag(this);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        invWindow.InvSlotDrag(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        invWindow.InvSlotEndDrag(this);
    }
}
