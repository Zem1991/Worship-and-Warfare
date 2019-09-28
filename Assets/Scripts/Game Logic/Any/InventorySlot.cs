using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public Inventory inventory;
    public ArtifactType type;
    public string slotName;
    public Artifact artifact;

    public void Initialize(Inventory inventory, ArtifactType type, string appendId = null)
    {
        this.inventory = inventory;
        this.type = type;
        slotName = type.ToString();
        if (appendId != null) slotName += " " + appendId;
    }

    public bool HasArtifact()
    {
        return artifact != null;
    }

    public bool CheckType(ArtifactType type)
    {
        return this.type == type;
    }

    public bool AddArtifact(Artifact item)
    {
        if (HasArtifact())
        {
            if (!RemoveArtifact())
                return false;
        }
        artifact = item;
        return true;
    }

    public bool RemoveArtifact()
    {
        if (HasArtifact())
        {
            if (!inventory.AddArtifactToBackpack(artifact))
                return false;
        }
        artifact = null;
        return true;
    }
}
