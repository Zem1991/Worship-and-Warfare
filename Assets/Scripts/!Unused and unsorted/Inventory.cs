using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public const int BACKPACK_LIMIT = 4;

    public InventorySlot mainHand;
    public InventorySlot offHand;
    public InventorySlot helmet;
    public InventorySlot armor;
    public InventorySlot trinket1;
    public InventorySlot trinket2;
    public InventorySlot trinket3;
    public InventorySlot trinket4;
    public List<InventorySlot> backpack = new List<InventorySlot>();

    void Start()
    {
        mainHand = new InventorySlot(this, ArtifactType.MAIN_HAND);
        offHand = new InventorySlot(this, ArtifactType.OFF_HAND);
        helmet = new InventorySlot(this, ArtifactType.HELMET);
        armor = new InventorySlot(this, ArtifactType.ARMOR);
        trinket1 = new InventorySlot(this, ArtifactType.TRINKET);
        trinket2 = new InventorySlot(this, ArtifactType.TRINKET);
        trinket3 = new InventorySlot(this, ArtifactType.TRINKET);
        trinket4 = new InventorySlot(this, ArtifactType.TRINKET);
        for (int i = 0; i < BACKPACK_LIMIT; i++)
        {
            backpack.Add(new InventorySlot(this, ArtifactType.ANY));
        }
    }

    public bool AddArtifactToSlot(InventorySlot slot, Artifact item)
    {
        bool result = slot.AddArtifact(item);
        return result;
    }

    public bool RemoveArtifactFromSlot(InventorySlot slot)
    {
        bool result = slot.RemoveArtifact();
        return result;
    }

    public bool AddArtifactToBackpack(Artifact item)
    {
        Debug.LogError("AddArtifactToBackpack not implemented!");
        return true;
    }
}
