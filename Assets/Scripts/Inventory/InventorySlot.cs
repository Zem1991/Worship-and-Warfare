using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour  //AbstractSlot<Artifact>
{
    [Header("Item data")]
    public Artifact slotItem;
    public bool isBeingDragged;

    [Header("Slot data")]
    public Inventory inventory;
    public ArtifactType slotType;

    public void Initialize(Inventory inventory, ArtifactType slotType, DB_Artifact dbArtifact = null)
    {
        this.inventory = inventory;
        this.slotType = slotType;
        name = slotType.ToString() + " slot";

        if (dbArtifact != null)
        {
            Artifact prefab = AllPrefabs.Instance.artifact;
            Artifact item = Instantiate(prefab, transform);
            item.Initialize(dbArtifact);
            Set(item);

            if (!HasSlotObject())
            {
                Debug.LogWarning("Created artifact was destroyed due to slot rejection.");
                Destroy(item.gameObject);
            }
        }
    }

    public void Set(Artifact item)
    {
        slotItem = item;
        if (slotItem) slotItem.transform.parent = transform;
    }

    public void Clear()
    {
        slotItem = null;
    }

    public bool HasSlotObject()
    {
        return slotItem != null;
    }

    //public bool HasSlotObject(Artifact slotObject)
    //{
    //    return HasSlotObject() && (slotObj == slotObject);
    //}
}
