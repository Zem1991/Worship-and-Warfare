using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public override void RightClick()
    {
        Inventory inv = invSlot.inventory;
        inv.SwapTypeBased(invSlot);

        UI_InventoryInfo invInfo = panelDND as UI_InventoryInfo;
        invInfo.RefreshInfo(true);
    }
}
