using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [Header("Hero parameters")]
    [SerializeField] private int levelUps;

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
        experienceStats.Initialize(experienceData);

        inventory = Instantiate(prefabInventory, transform);
        inventory.Initialize(this, inventoryData);
    }

    public void RecalculateStats()
    {
        DB_Class classs = dbData.classs;
        attributeStats.RecalculateStats(classs.attributeStats, inventory);
    }

    public void RecalculateExperience(int experience)
    {
        experienceStats.experience += experience;
        levelUps = ExperienceCalculation.CalculateLevelUps(experienceStats.level, experienceStats.experience);
    }

    public bool ApplyLevelUp(AttributeType attributeUp)
    {
        if (levelUps <= 0) return false;

        switch (attributeUp)
        {
            case AttributeType.COMMAND:
                attributeStats.atrCommand++;
                break;
            case AttributeType.OFFENSE:
                attributeStats.atrOffense++;
                break;
            case AttributeType.DEFENSE:
                attributeStats.atrDefense++;
                break;
            case AttributeType.KNOWLEDGE:
                attributeStats.atrPower++;
                break;
            case AttributeType.SPIRIT:
                attributeStats.atrFocus++;
                break;
            case AttributeType.CRAFT:
                attributeStats.atrCraft++;
                break;
        }
        levelUps--;
        return true;
    }
}
