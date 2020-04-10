using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : AbstractSlot<Artifact>
{
    [Header("Inventory data")]
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
            SetSlotObject(item);

            if (!HasSlotObject())
            {
                Debug.LogWarning("Created artifact was destroyed due to slot rejection.");
                Destroy(item.gameObject);
            }
        }
    }
}
