using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : AbstractSlotContainer<InventorySlot, Artifact>
{
    public const int BACKPACK_LIMIT = 4;

    [Header("Hero reference")]
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
    [SerializeField] private List<InventorySlot> backpackSlots = new List<InventorySlot>();

    [Header("Stats")]
    public AttributeStats attributeStats;
    //public int atrOffense;
    //public int atrDefense;
    //public int atrSupport;
    //public int atrCommand;
    //public int atrMagic;
    //public int atrTech;

    private List<InventorySlot> parameterSlots = new List<InventorySlot>();

    public void Initialize(Hero hero, InventoryData inventoryData)
    {
        AbstractDBContentHandler<DB_Artifact> dbArtifacts = DBHandler_Artifact.Instance;

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
        helmet.Initialize(this, ArtifactType.HEAD, he);
        parameterSlots.Add(helmet);

        armor = Instantiate(prefabInventorySlot, transform);
        armor.Initialize(this, ArtifactType.TORSO, ar);
        parameterSlots.Add(armor);

        trinket1 = Instantiate(prefabInventorySlot, transform);
        trinket1.Initialize(this, ArtifactType.TRINKET, t1);
        parameterSlots.Add(trinket1);

        trinket2 = Instantiate(prefabInventorySlot, transform);
        trinket2.Initialize(this, ArtifactType.TRINKET, t2);
        parameterSlots.Add(trinket2);

        trinket3 = Instantiate(prefabInventorySlot, transform);
        trinket3.Initialize(this, ArtifactType.TRINKET, t3);
        parameterSlots.Add(trinket3);

        trinket4 = Instantiate(prefabInventorySlot, transform);
        trinket4.Initialize(this, ArtifactType.TRINKET, t4);
        parameterSlots.Add(trinket4);

        //Add one empty backpack slot. This makes handling the inventory and trade windows much easier.
        AddBackpackSlot();

        RecalculateStats();
    }

    public void AddBackpackSlot()
    {
        InventorySlot prefabInventorySlot = AllPrefabs.Instance.inventorySlot;
        InventorySlot newSlot = Instantiate(prefabInventorySlot, transform);
        newSlot.Initialize(this, ArtifactType.ANY, null);
        newSlot.name = "Backpack";
        backpackSlots.Add(newSlot);
    }

    public void RemoveBackpackSlot(InventorySlot slot)
    {
        backpackSlots.Remove(slot);
        backpackSlots.TrimExcess();
    }

    public override bool AddSlotObject(Artifact item)
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
            case ArtifactType.HEAD:
                slot = helmet;
                break;
            case ArtifactType.TORSO:
                slot = armor;
                break;
            //TODO trinket slots
        }
        if (slot) return AddSlotObjectToSlot(slot, item);
        else return AddArtifactToBackpack(item);
    }

    public override bool AddSlotObjectToSlot(AbstractSlot<Artifact> slot, Artifact item)
    {
        bool result = base.AddSlotObjectToSlot(slot, item);
        RecalculateStats();
        return result;
    }

    public override bool RemoveSlotObjectFromSlot(AbstractSlot<Artifact> slot)
    {
        bool result = base.RemoveSlotObjectFromSlot(slot);
        RecalculateStats();
        return result;
    }

    public List<InventorySlot> GetBackpackSlots(bool invertOrder)
    {
        List<InventorySlot> result = new List<InventorySlot>(backpackSlots);
        if (invertOrder) result.Reverse();
        return result;
    }

    public bool AddArtifactToBackpack(Artifact artifact)
    {
        foreach (var item in backpackSlots)
        {
            if (!item.HasSlotObject())
            {
                if (item.AddSlotObject(artifact)) return true;
            }
        }
        return false;
    }

    public void RecalculateStats()
    {
        int atrOffense = 0;
        int atrDefense = 0;
        int atrSupport = 0;
        int atrCommand = 0;
        int atrMagic = 0;
        int atrTech = 0;

        foreach (InventorySlot invSlot in parameterSlots)
        {
            Artifact artifact = invSlot.slotObj;
            if (!artifact || invSlot.isBeingDragged) continue;

            atrOffense += artifact.dbData.attributeStats.atrOffense;
            atrDefense += artifact.dbData.attributeStats.atrDefense;
            atrSupport += artifact.dbData.attributeStats.atrSupport;
            atrCommand += artifact.dbData.attributeStats.atrCommand;
            atrMagic += artifact.dbData.attributeStats.atrMagic;
            atrTech += artifact.dbData.attributeStats.atrTech;
        }

        attributeStats.atrOffense = atrOffense;
        attributeStats.atrDefense = atrDefense;
        attributeStats.atrSupport = atrSupport;
        attributeStats.atrCommand = atrCommand;
        attributeStats.atrMagic = atrMagic;
        attributeStats.atrTech = atrTech;

        hero.RecalculateStats();
    }
}
