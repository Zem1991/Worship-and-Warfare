using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public const int BACKPACK_LIMIT = 4;

    [Header("Hero data")]
    public Hero hero;

    public InventorySlot mainHand;
    public InventorySlot offHand;
    public InventorySlot helmet;
    public InventorySlot armor;
    public InventorySlot trinket1;
    public InventorySlot trinket2;
    public InventorySlot trinket3;
    public InventorySlot trinket4;
    public List<InventorySlot> backpack = new List<InventorySlot>();

    public void Initialize(Hero hero, InventorySlot prefabInventorySlot)
    {
        this.hero = hero;

        mainHand = Instantiate(prefabInventorySlot, transform);
        mainHand.Initialize(this, ArtifactType.MAIN_HAND);

        offHand = Instantiate(prefabInventorySlot, transform);
        offHand.Initialize(this, ArtifactType.OFF_HAND);

        helmet = Instantiate(prefabInventorySlot, transform);
        helmet.Initialize(this, ArtifactType.HELMET);

        armor = Instantiate(prefabInventorySlot, transform);
        armor.Initialize(this, ArtifactType.ARMOR);

        trinket1 = Instantiate(prefabInventorySlot, transform);
        trinket1.Initialize(this, ArtifactType.TRINKET, "1");

        trinket2 = Instantiate(prefabInventorySlot, transform);
        trinket2.Initialize(this, ArtifactType.TRINKET, "2");

        trinket3 = Instantiate(prefabInventorySlot, transform);
        trinket3.Initialize(this, ArtifactType.TRINKET, "3");

        trinket4 = Instantiate(prefabInventorySlot, transform);
        trinket4.Initialize(this, ArtifactType.TRINKET, "4");
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
