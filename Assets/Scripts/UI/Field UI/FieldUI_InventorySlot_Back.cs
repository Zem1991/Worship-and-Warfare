using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FieldUI_InventorySlot_Back : AUI_DNDSlot
{
    [Header("Runtime references")]
    public InventorySlot invSlot;

    public void UpdateSlot(InventorySlot invSlot)
    {
        this.invSlot = invSlot;
        //slotName = invSlot.slotName;

        Artifact artifact = invSlot.artifact;
        ChangeImage(artifact?.dbData.image);
    }
}
