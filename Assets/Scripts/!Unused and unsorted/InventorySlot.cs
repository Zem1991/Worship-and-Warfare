using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public Inventory inventory;
    public ArtifactType type;
    public Artifact artifact;

    public InventorySlot(Inventory inventory, ArtifactType type)
    {
        this.inventory = inventory;
        this.type = type;
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
