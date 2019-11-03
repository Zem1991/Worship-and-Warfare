using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FUI_InventorySlot_Back : MonoBehaviour, IDropHandler
{
    [Header("Edit mode stuff")]
    public Image slotImg;

    [Header("Runtime stuff")]
    public InventorySlot invSlot;
    public string slotName;
    public FUI_InventorySlot_Front invSlotFront;

    private FieldUI_Panel_Inventory invWindow;

    void Awake()
    {
        invSlotFront = GetComponentInChildren<FUI_InventorySlot_Front>();
        invWindow = GetComponentInParent<FieldUI_Panel_Inventory>();
    }

    public void UpdateSlot(InventorySlot invSlot)
    {
        this.invSlot = invSlot;
        slotName = invSlot.slotName;

        Artifact artifact = invSlot.artifact;
        invSlotFront.ChangeImage(artifact?.dbData.image);
    }

    public void OnDrop(PointerEventData eventData)
    {
        RectTransform invPanel = transform as RectTransform;
        if (RectTransformUtility.RectangleContainsScreenPoint(invPanel, InputManager.Instance.mouseScreenPos))
        {
            invWindow.InvSlotDrop(this);
        }
    }
}
