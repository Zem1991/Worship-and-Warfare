using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour  //AbstractSlotContainer<InventorySlot, Artifact>
{
    [Header("Static references")]
    public AttributeStats equipAttributeStats;
    public Transform equipmentSlotHolder;
    public Transform backpackSlotHolder;

    [Header("Dynamic references")]
    public Hero hero;

    [Header("Slots - equipment")]
    [SerializeField] private InventorySlot equipMainHand;
    [SerializeField] private InventorySlot equipOffHand;
    [SerializeField] private InventorySlot equipHead;
    [SerializeField] private InventorySlot equipTorso;
    [SerializeField] private InventorySlot equipBack;
    [SerializeField] private InventorySlot equipNeck;
    [SerializeField] private InventorySlot equipRing1;
    [SerializeField] private InventorySlot equipRing2;
    [SerializeField] private InventorySlot equipTrinket1;
    [SerializeField] private InventorySlot equipTrinket2;
    [SerializeField] private InventorySlot equipTrinket3;
    [SerializeField] private InventorySlot equipTrinket4;

    [Header("Slots - backpack")]
    [SerializeField] private List<InventorySlot> backpackItems;

    public void Initialize(Hero hero, InventoryData inventoryData)
    {
        this.hero = hero;

        AbstractDBContentHandler<DB_Artifact> dbArtifacts = DBHandler_Artifact.Instance;
        InventorySlot prefabInventorySlot = AllPrefabs.Instance.inventorySlot;

        DB_Artifact mh = null;
        DB_Artifact oh = null;
        DB_Artifact he = null;
        DB_Artifact to = null;
        DB_Artifact ba = null;
        DB_Artifact ne = null;
        DB_Artifact r1 = null;
        DB_Artifact r2 = null;
        DB_Artifact t1 = null;
        DB_Artifact t2 = null;
        DB_Artifact t3 = null;
        DB_Artifact t4 = null;

        if (inventoryData != null)
        {
            if (inventoryData.mainHandId != null) mh = dbArtifacts.Select(inventoryData.mainHandId);
            if (inventoryData.offHandId != null) oh = dbArtifacts.Select(inventoryData.offHandId);
            if (inventoryData.headId != null) he = dbArtifacts.Select(inventoryData.headId);
            if (inventoryData.torsoId != null) to = dbArtifacts.Select(inventoryData.torsoId);
            if (inventoryData.backId != null) ba = dbArtifacts.Select(inventoryData.backId);
            if (inventoryData.neckId != null) ne = dbArtifacts.Select(inventoryData.neckId);
            if (inventoryData.ring1Id != null) r1 = dbArtifacts.Select(inventoryData.ring1Id);
            if (inventoryData.ring2Id != null) r2 = dbArtifacts.Select(inventoryData.ring2Id);
            if (inventoryData.trinket1Id != null) t1 = dbArtifacts.Select(inventoryData.trinket1Id);
            if (inventoryData.trinket2Id != null) t2 = dbArtifacts.Select(inventoryData.trinket2Id);
            if (inventoryData.trinket3Id != null) t3 = dbArtifacts.Select(inventoryData.trinket3Id);
            if (inventoryData.trinket4Id != null) t4 = dbArtifacts.Select(inventoryData.trinket4Id);
        }

        equipMainHand = Instantiate(prefabInventorySlot, equipmentSlotHolder.transform);
        equipMainHand.Initialize(this, ArtifactType.MAIN_HAND, mh);
        equipOffHand = Instantiate(prefabInventorySlot, equipmentSlotHolder.transform);
        equipOffHand.Initialize(this, ArtifactType.OFF_HAND, oh);
        equipHead = Instantiate(prefabInventorySlot, equipmentSlotHolder.transform);
        equipHead.Initialize(this, ArtifactType.HEAD, he);
        equipTorso = Instantiate(prefabInventorySlot, equipmentSlotHolder.transform);
        equipTorso.Initialize(this, ArtifactType.TORSO, to);
        equipBack = Instantiate(prefabInventorySlot, equipmentSlotHolder.transform);
        equipBack.Initialize(this, ArtifactType.BACK, ba);
        equipNeck = Instantiate(prefabInventorySlot, equipmentSlotHolder.transform);
        equipNeck.Initialize(this, ArtifactType.NECK, ne);
        equipRing1 = Instantiate(prefabInventorySlot, equipmentSlotHolder.transform);
        equipRing1.Initialize(this, ArtifactType.RING, r1);
        equipRing2 = Instantiate(prefabInventorySlot, equipmentSlotHolder.transform);
        equipRing2.Initialize(this, ArtifactType.RING, r2);
        equipTrinket1 = Instantiate(prefabInventorySlot, equipmentSlotHolder.transform);
        equipTrinket1.Initialize(this, ArtifactType.TRINKET, t1);
        equipTrinket2 = Instantiate(prefabInventorySlot, equipmentSlotHolder.transform);
        equipTrinket2.Initialize(this, ArtifactType.TRINKET, t2);
        equipTrinket3 = Instantiate(prefabInventorySlot, equipmentSlotHolder.transform);
        equipTrinket3.Initialize(this, ArtifactType.TRINKET, t3);
        equipTrinket4 = Instantiate(prefabInventorySlot, equipmentSlotHolder.transform);
        equipTrinket4.Initialize(this, ArtifactType.TRINKET, t4);

        //Add one empty backpack slot. This makes handling the inventory and trade windows much easier.
        AddBackpackSlot();

        RecalculateStats();
    }

    public void AddBackpackSlot()
    {
        InventorySlot prefab = AllPrefabs.Instance.inventorySlot;
        InventorySlot newSlot = Instantiate(prefab, backpackSlotHolder.transform);
        newSlot.Initialize(this, ArtifactType.ANY, null);
        newSlot.name = "Backpack slot";
        backpackItems.Add(newSlot);
    }

    public bool RemoveBackpackSlot(InventorySlot slot)
    {
        bool result = backpackItems.Remove(slot);
        if (result)
        {
            backpackItems.TrimExcess();
            Destroy(slot.gameObject);
        }
        return result;
    }

    public bool HasBackpackSlot(InventorySlot slot)
    {
        return backpackItems.Contains(slot);
    }

    public void RecalculateStats()
    {
        int atrOffense = 0;
        int atrDefense = 0;
        int atrSupport = 0;
        int atrCommand = 0;
        int atrMagic = 0;
        int atrTech = 0;

        foreach (InventorySlot invSlot in GetEquipmentSlots())
        {
            Artifact artifact = invSlot.slotItem;
            if (!artifact || invSlot.isBeingDragged) continue;

            atrOffense += artifact.dbData.attributeStats.atrOffense;
            atrDefense += artifact.dbData.attributeStats.atrDefense;
            atrSupport += artifact.dbData.attributeStats.atrSupport;
            atrCommand += artifact.dbData.attributeStats.atrCommand;
            atrMagic += artifact.dbData.attributeStats.atrMagic;
            atrTech += artifact.dbData.attributeStats.atrTech;
        }

        equipAttributeStats.atrOffense = atrOffense;
        equipAttributeStats.atrDefense = atrDefense;
        equipAttributeStats.atrSupport = atrSupport;
        equipAttributeStats.atrCommand = atrCommand;
        equipAttributeStats.atrMagic = atrMagic;
        equipAttributeStats.atrTech = atrTech;

        hero.RecalculateStats();
    }

    public InventorySlot GetEquipmentSlot(ArtifactType type, int id = 0)
    {
        InventorySlot result = null;
        switch (type)
        {
            case ArtifactType.MAIN_HAND:
                result = equipMainHand;
                break;
            case ArtifactType.OFF_HAND:
                result = equipOffHand;
                break;
            case ArtifactType.HEAD:
                result = equipHead;
                break;
            case ArtifactType.TORSO:
                result = equipTorso;
                break;
            case ArtifactType.BACK:
                result = equipBack;
                break;
            case ArtifactType.NECK:
                result = equipNeck;
                break;
            case ArtifactType.RING:
                if (id == 1) result = equipRing1;
                if (id == 2) result = equipRing2;
                break;
            case ArtifactType.TRINKET:
                if (id == 1) result = equipTrinket1;
                if (id == 2) result = equipTrinket2;
                if (id == 3) result = equipTrinket3;
                if (id == 4) result = equipTrinket4;
                break;
        }
        return result;
    }

    public List<InventorySlot> GetEquipmentSlots()
    {
        List<InventorySlot> result = new List<InventorySlot>
        {
            equipMainHand,
            equipOffHand,
            equipHead,
            equipTorso,
            equipBack,
            equipNeck,
            equipRing1,
            equipRing2,
            equipTrinket1,
            equipTrinket2,
            equipTrinket3,
            equipTrinket4
        };
        return result;
    }

    public List<InventorySlot> GetBackpackSlots(bool reverseOrder)
    {
        List<InventorySlot> result = new List<InventorySlot>(backpackItems);
        result.OrderBy(a => a.slotItem);
        if (reverseOrder) result.Reverse();
        return result;
    }

    public bool AddFromPickup(Artifact artifact)
    {
        InventorySlot slot = null;
        switch (artifact.dbData.artifactType)
        {
            case ArtifactType.MAIN_HAND:
                slot = equipMainHand.HasSlotObject() ? null : equipMainHand;
                break;
            case ArtifactType.OFF_HAND:
                slot = equipOffHand.HasSlotObject() ? null : equipOffHand;
                break;
            case ArtifactType.HEAD:
                slot = equipHead.HasSlotObject() ? null : equipHead;
                break;
            case ArtifactType.TORSO:
                slot = equipTorso.HasSlotObject() ? null : equipTorso;
                break;
            case ArtifactType.BACK:
                slot = equipBack.HasSlotObject() ? null : equipBack;
                break;
            case ArtifactType.NECK:
                slot = equipNeck.HasSlotObject() ? null : equipNeck;
                break;
            case ArtifactType.RING:
                slot = equipRing1.HasSlotObject() ? null : equipRing1;
                if (!slot) slot = equipRing2.HasSlotObject() ? null : equipRing2;
                break;
            case ArtifactType.TRINKET:
                slot = equipTrinket1.HasSlotObject() ? null : equipTrinket1;
                if (!slot) slot = equipTrinket2.HasSlotObject() ? null : equipTrinket2;
                if (!slot) slot = equipTrinket3.HasSlotObject() ? null : equipTrinket3;
                if (!slot) slot = equipTrinket4.HasSlotObject() ? null : equipTrinket4;
                break;
        }

        bool result;
        if (slot)
        {
            //Create an temporary InventorySlot just to reuse the AddFromSlot function.
            InventorySlot temp = CreateTempSlot(artifact);
            result = AddFromSlot(temp, slot);
            Destroy(temp.gameObject);
        }
        else
        {
            result = AddToBackpack(artifact);
        }
        return result;
    }

    

    public bool AddFromSlot(InventorySlot fromSlot, InventorySlot toSlot)
    {
        Artifact fromItem = fromSlot.slotItem;
        Artifact toItem = toSlot.slotItem;

        bool fromBackpack = HasBackpackSlot(fromSlot);
        bool toBackpack = HasBackpackSlot(toSlot);
        bool equipAndBackpack = fromBackpack != toBackpack;
        bool sameTypeSlots = fromSlot.slotType == toSlot.slotType;
        bool sameTypeArtifacts = fromItem && toItem && fromItem.dbData.artifactType == toItem.dbData.artifactType;
        bool canSendToSlot = fromItem && fromItem.dbData.artifactType == toSlot.slotType;

        //If the slots are of the same type, then an simple swap is done.
        //Else if we are exchanging between equipped items and backpack items, then we do specific actions.
        bool result = false;
        if (sameTypeSlots)
        {
            //Here we just exchange items between slots.
            SwapSlotContents(fromSlot, toSlot);
            result = true;
        }
        else if (equipAndBackpack)
        {
            //If the items are of the same type, then an simple swap is done.
            //Else, we check for which one of the slots are from the backpack and act accordingly.
            if (sameTypeArtifacts)
            {
                SwapSlotContents(fromSlot, toSlot);
                result = true;
            }
            else
            {
                if (toBackpack)
                {
                    AddToBackpack(fromItem);
                    fromSlot.Clear();
                    result = true;
                }
                else if (fromBackpack && canSendToSlot)
                {
                    toSlot.Set(fromItem);
                    RemoveBackpackSlot(fromSlot);
                    result = true;
                }
            }
        }
        return result;
    }

    public bool AddToBackpack(Artifact artifact)
    {
        //Create an temporary InventorySlot just to reuse the AddFromSlot function.
        InventorySlot temp = CreateTempSlot(artifact);
        bool result = false;
        foreach (InventorySlot slot in backpackItems)
        {
            if (!slot.HasSlotObject())
            {
                SwapSlotContents(temp, slot);
                result = true;
                break;
            }
        }
        if (!result) Debug.LogError("Somehow the item was not added to the backpack!");
        else AddBackpackSlot();
        Destroy(temp.gameObject);
        return result;
    }

    private InventorySlot CreateTempSlot(Artifact artifact)
    {
        InventorySlot prefabInvSlot = AllPrefabs.Instance.inventorySlot;
        InventorySlot temp = Instantiate(prefabInvSlot, transform);
        temp.Initialize(this, artifact.dbData.artifactType, null);
        temp.slotItem = artifact;
        return temp;
    }

    private void SwapSlotContents(InventorySlot from, InventorySlot to)
    {
        Artifact fromItem = from.slotItem;
        from.Set(to.slotItem);
        to.Set(fromItem);
    }
}
