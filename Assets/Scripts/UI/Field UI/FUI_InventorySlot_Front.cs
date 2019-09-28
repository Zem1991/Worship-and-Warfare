using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FUI_InventorySlot_Front : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [Header("Edit mode stuff")]
    public Image slotImg;

    private FieldUI_CC_Inventory invWindow;
    private FUI_InventorySlot_Back invSlotBack;

    void Awake()
    {
        invWindow = GetComponentInParent<FieldUI_CC_Inventory>();
        invSlotBack = GetComponentInParent<FUI_InventorySlot_Back>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        invWindow.InvSlotDrag(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        invWindow.InvSlotDragEnd(this);
    }
}
