using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class AUI_DNDSlot : MonoBehaviour, IDropHandler
{
    [Header("Editor references")]
    public Image imgSlot;
    public Image imgSlotFront;
    public AUI_DNDSlot_Front slotFront;

    [Header("Runtime references")]
    //public InventorySlot invSlot;
    public AUI_PanelDragAndDrop panelDND;

    public void OnDrop(PointerEventData eventData)
    {
        RectTransform rectTransform = transform as RectTransform;
        if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, InputManager.Instance.mouseScreenPos))
        {
            AUI_DNDSlot_Front draggedObj = eventData.pointerDrag.GetComponent<AUI_DNDSlot_Front>();
            panelDND.DNDDrop(draggedObj, this);
        }
    }

    public void ChangeImage(Sprite img)
    {
        Color color = Color.white;
        if (!img) color.a = 0;
        else if (panelDND.slotFrontDragged == slotFront) color.a = 0.5F;

        imgSlotFront.sprite = img;
        imgSlotFront.color = color;
    }
}
