using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AUI_PanelDragAndDrop : AbstractUIPanel
{
    [Header("Drag and Drop - local reference")]
    public UI_DraggableElement draggableElement;

    [Header("Drag and Drop - runtime")]
    public AUI_DNDSlot_Front slotFrontDragged = null;
    //public bool isDraggingElement = false;

    public virtual void DNDBeginDrag(AUI_DNDSlot_Front slotFront)
    {
        //Remember to call this in overriden function using "base"
        draggableElement.BeginDrag(slotFront.slotBack.imgSlot.sprite);
        slotFrontDragged = slotFront;
    }

    public virtual void DNDDrag()
    {
        draggableElement.Drag();
    }

    public virtual void DNDEndDrag()
    {
        if (slotFrontDragged) DNDDrop(null);
        draggableElement.EndDrag();
    }

    public virtual void DNDDrop(AUI_DNDSlot slot)
    {
        //Remember to call this in overriden function using "base"
        slotFrontDragged = null;
    }
}
