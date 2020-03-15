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

        name = slotType.ToString() + " slot";
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
        bool isTheSameArtifact = HasSlotObject(slotObject);
        bool acceptableType = CheckSlotObjectType(slotObject);

        if (isTheSameArtifact) return false;
        if (!acceptableType) return false;

        if (!isTheSameArtifact && acceptableType)
        {
            //Here we just swap positions because both slots are compatible.
        }

        if (!RemoveSlotObject()) return false;

        slotObj = slotObject;
        slotObj.transform.parent = transform;

        //If this slot is part of the backpack, we add one more slot for more itens to be stored.
        if (inventory.GetBackpackSlots(false).Contains(this)) inventory.AddBackpackSlot();

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

        ////If this slot is part of the backpack, ???
        //if (inventory.GetBackpackSlots(false).Contains(this)) inventory.RemoveBackpackSlot();

        return true;
    }

    public bool SwapSlots(InventorySlot other)
    {
        bool typeCheck = slotType == other.slotType;
        if (!typeCheck) return false;
        //bool anyCheck = slotType == ArtifactType.ANY || other.slotType == ArtifactType.ANY;
        //if (!typeCheck && !anyCheck) return false;

        Artifact thisArtifact = slotObj as Artifact;
        Artifact otherArtifact = other.slotObj as Artifact;
        slotObj = otherArtifact;
        slotObj.transform.parent = transform;
        other.slotObj = thisArtifact;
        other.slotObj.transform.parent = other.transform;
        return true;
    }
}
