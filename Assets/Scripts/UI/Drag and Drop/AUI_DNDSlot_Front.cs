using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class AUI_DNDSlot_Front : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Editor references")]
    public AUI_DNDSlot slotBack;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!slotBack.panelDND.DNDCanDragThis(this)) return;
        if (!CheckSlotFilled()) return;
        slotBack.ChangeImage(slotBack.imgSlotFront.sprite);
        slotBack.panelDND.DNDBeginDrag(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!slotBack.panelDND.DNDCanDragThis(this)) return;
        if (!CheckSlotFilled()) return;
        slotBack.panelDND.DNDDrag();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!slotBack.panelDND.DNDCanDragThis(this)) return;
        //if (!CheckSlotFilled()) return;
        slotBack.panelDND.DNDEndDrag();
    }

    public abstract bool CheckSlotFilled();
}
