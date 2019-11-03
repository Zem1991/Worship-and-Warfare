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

    public void Initialize(Hero hero, InventoryData inventoryData)
    {
        DBContentHandler<DB_Artifact> dbArtifacts = DBHandler_Artifact.Instance;

        InventorySlot prefabInventorySlot = AllPrefabs.Instance.inventorySlot;

        this.hero = hero;

        DB_Artifact mh = null;
        DB_Artifact oh = null;
        DB_Artifact he = null;
        DB_Artifact ar = null;
        DB_Artifact t1 = null;
        DB_Artifact t2 = null;
        DB_Artifact t3 = null;
        DB_Artifact t4 = null;
        if (inventoryData != null)
        {
            if (inventoryData.mainHandId != null) mh = dbArtifacts.Select(inventoryData.mainHandId);
            if (inventoryData.offHandId != null) oh = dbArtifacts.Select(inventoryData.offHandId);
            if (inventoryData.helmetId != null) he = dbArtifacts.Select(inventoryData.helmetId);
            if (inventoryData.armorId != null) ar = dbArtifacts.Select(inventoryData.armorId);
            if (inventoryData.trinket1Id != null) t1 = dbArtifacts.Select(inventoryData.trinket1Id);
            if (inventoryData.trinket2Id != null) t2 = dbArtifacts.Select(inventoryData.trinket2Id);
            if (inventoryData.trinket3Id != null) t3 = dbArtifacts.Select(inventoryData.trinket3Id);
            if (inventoryData.trinket4Id != null) t4 = dbArtifacts.Select(inventoryData.trinket4Id);
        }

        mainHand = Instantiate(prefabInventorySlot, transform);
        mainHand.Initialize(this, ArtifactType.MAIN_HAND, mh);
        parameterSlots.Add(mainHand);

        offHand = Instantiate(prefabInventorySlot, transform);
        offHand.Initialize(this, ArtifactType.OFF_HAND, oh);
        parameterSlots.Add(offHand);

        helmet = Instantiate(prefabInventorySlot, transform);
        helmet.Initialize(this, ArtifactType.HELMET, he);
        parameterSlots.Add(helmet);

        armor = Instantiate(prefabInventorySlot, transform);
        armor.Initialize(this, ArtifactType.ARMOR, ar);
        parameterSlots.Add(armor);

        trinket1 = Instantiate(prefabInventorySlot, transform);
        trinket1.Initialize(this, ArtifactType.TRINKET, t1, "1");
        parameterSlots.Add(trinket1);

        trinket2 = Instantiate(prefabInventorySlot, transform);
        trinket2.Initialize(this, ArtifactType.TRINKET, t2, "2");
        parameterSlots.Add(trinket2);

        trinket3 = Instantiate(prefabInventorySlot, transform);
        trinket3.Initialize(this, ArtifactType.TRINKET, t3, "3");
        parameterSlots.Add(trinket3);

        trinket4 = Instantiate(prefabInventorySlot, transform);
        trinket4.Initialize(this, ArtifactType.TRINKET, t4, "4");
        parameterSlots.Add(trinket4);

        RecalculateStats();
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
        RecalculateStats();
        return result;
    }

    public bool RemoveArtifactFromSlot(InventorySlot slot)
    {
        bool result = slot.RemoveArtifact();
        RecalculateStats();
        return result;
    }

    public bool AddArtifactToBackpack(Artifact item)
    {
        Debug.LogError("AddArtifactToBackpack not implemented!");
        return true;
    }

    public void RecalculateStats()
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

        hero.RecalculateStats();
    }
}
