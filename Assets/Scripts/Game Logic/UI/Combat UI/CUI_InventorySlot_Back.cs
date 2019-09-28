using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CUI_InventorySlot_Back : MonoBehaviour, IDropHandler
{
    [Header("Edit mode stuff")]
    public Image slotImg;

    [Header("Runtime stuff")]
    public string slotName;
    public InventorySlot invSlot;

    private FieldUI_CC_Inventory invWindow;
    private CUI_InventorySlot_Front invSlotFront;

    void Awake()
    {
        invWindow = GetComponentInParent<FieldUI_CC_Inventory>();
        invSlotFront = GetComponentInChildren<CUI_InventorySlot_Front>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        RectTransform invPanel = transform as RectTransform;
        if (RectTransformUtility.RectangleContainsScreenPoint(invPanel, InputManager.Instance.mouseScreenPos))
        {
            Debug.Log("Item dropped over slot " + slotName);
            invWindow.InvSlotDrop(this);
        }
    }
}
