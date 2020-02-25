using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : AbstractSlot<Artifact>
{
    [Header("Slot data")]
    public Inventory inventory;
    public ArtifactType slotType;

    public void Initialize(Inventory inventory, ArtifactType slotType, DB_Artifact dbArtifact = null)
    {
        this.inventory = inventory;
        this.slotType = slotType;

        if (dbArtifact != null)
        {
            Artifact item = CreateArtifact(dbArtifact);
            AddSlotObject(item);
            if (!HasSlotObject(item))
            {
                Debug.LogWarning("Created artifact was destroyed due to slot rejection.");
                Destroy(item.gameObject);
            }
        }
    }

    public Artifact CreateArtifact(DB_Artifact dbArtifact)
    {
        Artifact prefab = AllPrefabs.Instance.artifact;
        Artifact item = Instantiate(prefab, transform);
        item.Initialize(dbArtifact);
        return item;
    }

    public override bool CheckSlotObjectType(Artifact slotObject)
    {
        bool typeCheck = slotType == slotObject.dbData.artifactType;
        bool anyCheck = slotType == ArtifactType.ANY;
        return typeCheck || anyCheck;
    }

    public override bool AddSlotObject(Artifact slotObject)
    {
        if (!CheckSlotObjectType(slotObject)) return false;
        if (HasSlotObject(slotObject)) return false;
        if (HasSlotObject() && !RemoveSlotObject()) return false;

        slotObj = slotObject;
        slotObj.transform.parent = transform;
        return true;
    }

    public override bool RemoveSlotObject()
    {
        if (HasSlotObject())
        {
            if (!inventory.AddArtifactToBackpack(slotObj))
                return false;
        }
        slotObj = null;
        return true;
    }
}
