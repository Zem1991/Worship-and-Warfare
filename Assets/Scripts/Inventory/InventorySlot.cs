using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [Header("Slot data")]
    public Inventory inventory;
    public ArtifactType type;
    public string slotName;

    [Header("Artifact data")]
    public Artifact artifact;
    public bool beingDragged;

    public void Initialize(Inventory inventory, ArtifactType type, DB_Artifact dbArtifact = null, string appendId = null)
    {
        this.inventory = inventory;
        this.type = type;
        slotName = type.ToString();

        if (dbArtifact != null)
        {
            Artifact item = CreateArtifact(dbArtifact);
            AddArtifact(item);
            if (!HasArtifact(item))
            {
                Debug.LogWarning("Created artifact was destroyed due to slot rejection.");
                Destroy(item.gameObject);
            }
        }

        if (appendId != null)
        {
            slotName += " " + appendId;
        }
    }

    public Artifact CreateArtifact(DB_Artifact dbArtifact)
    {
        Artifact prefab = AllPrefabs.Instance.artifact;
        Artifact item = Instantiate(prefab, transform);
        item.Initialize(dbArtifact);
        return item;
    }

    public bool HasArtifact()
    {
        return artifact != null;
    }

    public bool HasArtifact(Artifact item)
    {
        return artifact == item;
    }

    public bool CheckType(ArtifactType type)
    {
        bool typeCheck = this.type == type;
        bool anyCheck = this.type == ArtifactType.ANY;
        return typeCheck || anyCheck;
    }

    public bool AddArtifact(Artifact item)
    {
        if (artifact == item)
        {
            Debug.LogWarning("Artifact rejected! DBArtifact " + item.dbData.artifactName + " already exists on slot type " + type);
            return false;
        }

        if (!CheckType(item.dbData.artifactType))
        {
            Debug.LogWarning("Artifact rejected! DBArtifact " + item.dbData.artifactName + " over slot type " + type);
            return false;
        }

        if (HasArtifact())
        {
            if (!RemoveArtifact())
                return false;
        }
        artifact = item;
        artifact.transform.parent = transform;
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
