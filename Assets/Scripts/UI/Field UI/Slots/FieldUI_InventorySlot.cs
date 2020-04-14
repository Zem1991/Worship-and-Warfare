using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FieldUI_InventorySlot : AUI_DNDSlot
{
    [Header("Editor references - Inventory")]
    public Text txtType;

    [Header("Runtime references - Inventory")]
    public InventorySlot invSlot;

    public void UpdateSlot(AUI_PanelDragAndDrop panelDND, InventorySlot invSlot)
    {
        this.panelDND = panelDND;
        this.invSlot = invSlot;

        Artifact artifact = invSlot?.Get();
        ChangeImage(artifact?.dbData.image);
    }
}
