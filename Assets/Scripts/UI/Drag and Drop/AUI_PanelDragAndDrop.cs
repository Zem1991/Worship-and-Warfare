using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AUI_PanelDragAndDrop : AbstractUIPanel
{
    [Header("Drag and Drop - settings")]
    public UI_DraggableElement draggableElement;

    [Header("Drag and Drop - runtime")]
    public AUI_DNDSlot_Front slotFrontDragged = null;

    public abstract bool DNDCanDragThis(AUI_DNDSlot_Front slotFront);

    public virtual void DNDBeginDrag(AUI_DNDSlot_Front slotFront)
    {
        Debug.Log("1/3 DNDBeginDrag");
        //Remember to call this in overriden function using "base"
        draggableElement.BeginDrag(slotFront.slotBack.imgSlotFront.sprite);
        slotFrontDragged = slotFront;
    }

    public void DNDDrag()
    {
        Debug.Log("2/3 DNDDrag");
        draggableElement.Drag();
    }

    public virtual void DNDDrop(AUI_DNDSlot_Front slotFrontDragged, AUI_DNDSlot targetSlot)
    {
        Debug.Log("3/3 DNDDrop");
        //Remember to call this in overriden function using "base"
        this.slotFrontDragged = null;
        draggableElement.EndDrag();
    }

    public void DNDForceDrop()
    {
        Debug.Log("C/3 DNDForceDrop");
        if (slotFrontDragged) DNDDrop(null, null);
        //draggableElement.EndDrag();
    }

    //public void DNDEndDrag()
    //{
    //    Debug.Log("4/3 DNDEndDrag");
    //    //if (slotFrontDragged) DNDDrop(slotFrontDragged, null);    //Not required.
    //    if (slotFrontDragged) DNDDrop(slotFrontDragged, null);
    //    //draggableElement.EndDrag();
    //}
}
