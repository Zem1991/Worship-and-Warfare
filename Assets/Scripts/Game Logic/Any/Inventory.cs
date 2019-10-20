using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public const int BACKPACK_LIMIT = 4;

    [Header("Hero data")]
    public Hero hero;

    [Header("Slots")]
    public InventorySlot mainHand;
    public InventorySlot offHand;
    public InventorySlot helmet;
    public InventorySlot armor;
    public InventorySlot trinket1;
    public InventorySlot trinket2;
    public InventorySlot trinket3;
    public InventorySlot trinket4;
    public List<InventorySlot> backpack = new List<InventorySlot>();

    [Header("Parameters")]
    public int atrCommand;
    public int atrOffense;
    public int atrDefense;
    public int atrPower;
    public int atrFocus;

    private List<InventorySlot> parameterSlots = new List<InventorySlot>();

    public void Initialize(InventoryData inventory, Hero hero)
    {
        DatabaseManager db = DatabaseManager.Instance;
        DBHandler_Artifact dbArtifacts = db.artifacts;

        InventorySlot prefabInventorySlot = AllPrefabs.Instance.inventorySlot;

        this.hero = hero;

        DB_Artifact mh = dbArtifacts.Select(inventory.mainHand, false);
        mainHand = Instantiate(prefabInventorySlot, transform);
        mainHand.Initialize(this, ArtifactType.MAIN_HAND, mh);
        parameterSlots.Add(mainHand);

        DB_Artifact oh = dbArtifacts.Select(inventory.offHand, false);
        offHand = Instantiate(prefabInventorySlot, transform);
        offHand.Initialize(this, ArtifactType.OFF_HAND, oh);
        parameterSlots.Add(offHand);

        DB_Artifact he = dbArtifacts.Select(inventory.helmet, false);
        helmet = Instantiate(prefabInventorySlot, transform);
        helmet.Initialize(this, ArtifactType.HELMET, he);
        parameterSlots.Add(helmet);

        DB_Artifact ar = dbArtifacts.Select(inventory.armor, false);
        armor = Instantiate(prefabInventorySlot, transform);
        armor.Initialize(this, ArtifactType.ARMOR, ar);
        parameterSlots.Add(armor);

        DB_Artifact t1 = dbArtifacts.Select(inventory.trinket1, false);
        trinket1 = Instantiate(prefabInventorySlot, transform);
        trinket1.Initialize(this, ArtifactType.TRINKET, t1, "1");
        parameterSlots.Add(trinket1);

        DB_Artifact t2 = dbArtifacts.Select(inventory.trinket2, false);
        trinket2 = Instantiate(prefabInventorySlot, transform);
        trinket2.Initialize(this, ArtifactType.TRINKET, t2, "2");
        parameterSlots.Add(trinket2);

        DB_Artifact t3 = dbArtifacts.Select(inventory.trinket3, false);
        trinket3 = Instantiate(prefabInventorySlot, transform);
        trinket3.Initialize(this, ArtifactType.TRINKET, t3, "3");
        parameterSlots.Add(trinket3);

        DB_Artifact t4 = dbArtifacts.Select(inventory.trinket4, false);
        trinket4 = Instantiate(prefabInventorySlot, transform);
        trinket4.Initialize(this, ArtifactType.TRINKET, t4, "4");
        parameterSlots.Add(trinket4);

        RecalculateParameters();
    }

    public bool AddArtifact(Artifact item)
    {
        InventorySlot slot = null;
        switch (item.dbData.artifactType)
        {
            case ArtifactType.MAIN_HAND:
                slot = mainHand;
                break;
            case ArtifactType.OFF_HAND:
                slot = offHand;
                break;
            case ArtifactType.HELMET:
                slot = helmet;
                break;
            case ArtifactType.ARMOR:
                slot = armor;
                break;
        }
        if (slot) return AddArtifactToSlot(slot, item);
        else return AddArtifactToBackpack(item);
    }

    public bool AddArtifactToSlot(InventorySlot slot, Artifact item)
    {
        bool result = slot.AddArtifact(item);
        RecalculateParameters();
        return result;
    }

    public bool RemoveArtifactFromSlot(InventorySlot slot)
    {
        bool result = slot.RemoveArtifact();
        RecalculateParameters();
        return result;
    }

    public bool AddArtifactToBackpack(Artifact item)
    {
        Debug.LogError("AddArtifactToBackpack not implemented!");
        return true;
    }

    public void RecalculateParameters()
    {
        int atrCommand = 0;
        int atrOffense = 0;
        int atrDefense = 0;
        int atrPower = 0;
        int atrFocus = 0;

        foreach (InventorySlot invSlot in parameterSlots)
        {
            Artifact artifact = invSlot.artifact;
            if (!artifact || invSlot.beingDragged) continue;

            atrCommand += artifact.dbData.atrCommand;
            atrOffense += artifact.dbData.atrOffense;
            atrDefense += artifact.dbData.atrDefense;
            atrPower += artifact.dbData.atrPower;
            atrFocus += artifact.dbData.atrFocus;
        }

        this.atrCommand = atrCommand;
        this.atrOffense = atrOffense;
        this.atrDefense = atrDefense;
        this.atrPower = atrPower;
        this.atrFocus = atrFocus;

        hero.RecalculateParameters();
    }
}
