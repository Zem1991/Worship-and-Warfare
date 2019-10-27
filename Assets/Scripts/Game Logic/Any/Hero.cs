using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [Header("Prefab references")]
    public CombatPieceStats combatPieceStats;
    public AttributeStats attributeStats;
    public ExperienceStats experienceStats;
    public Inventory inventory;

    [Header("Database reference")]
    public DB_Hero dbData;

    public void Initialize(DB_Hero dbData, ExperienceData experienceData, InventoryData inventoryData)
    {
        CombatPieceStats prefabCPS = AllPrefabs.Instance.combatPieceStats;
        AttributeStats prefabAS = AllPrefabs.Instance.attributeStats;
        ExperienceStats prefabES = AllPrefabs.Instance.experienceStats;
        Inventory prefabInventory = AllPrefabs.Instance.inventory;

        this.dbData = dbData;
        name = dbData.heroName;

        combatPieceStats = Instantiate(prefabCPS, transform);
        combatPieceStats.Initialize(dbData.classs.combatPieceStats);

        attributeStats = Instantiate(prefabAS, transform);
        attributeStats.Initialize(dbData.classs.attributeStats);

        experienceStats = Instantiate(prefabES, transform);
        if (experienceData != null) experienceStats.Initialize(experienceData);

        inventory = Instantiate(prefabInventory, transform);
        if (inventoryData != null) inventory.Initialize(inventoryData, this);
    }

    public void RecalculateParameters()
    {
        DB_Class classs = dbData.classs;
        attributeStats.RecalculateParameters(classs.attributeStats, inventory);
    }
}
