using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : AbstractPartyElement
{
    [Header("Hero parameters")]
    public int levelUps;

    [Header("Stats")]
    public CombatPieceStats combatPieceStats;
    public ExperienceStats experienceStats;
    public AttributeStats attributeStats;
    public AttributeStats levelUpAttributes;

    [Header("Inventory")]
    public Inventory inventory;

    [Header("Database reference")]
    public DB_Hero dbData;

    public void Initialize(DB_Hero dbData, ExperienceData experienceData, InventoryData inventoryData)
    {
        partyElementType = PartyElementType.HERO;

        CombatPieceStats prefabCPS = AllPrefabs.Instance.combatPieceStats;
        AttributeStats prefabAS = AllPrefabs.Instance.attributeStats;
        ExperienceStats prefabES = AllPrefabs.Instance.experienceStats;
        Inventory prefabInventory = AllPrefabs.Instance.inventory;

        this.dbData = dbData;
        name = dbData.heroName;

        combatPieceStats = Instantiate(prefabCPS, transform);
        combatPieceStats.Initialize(dbData.heroClass.combatPieceStats);

        experienceStats = Instantiate(prefabES, transform);
        experienceStats.Initialize(experienceData);

        attributeStats = Instantiate(prefabAS, transform);
        attributeStats.name = "Attribute Stats";
        levelUpAttributes = Instantiate(prefabAS, transform);
        levelUpAttributes.name = "LevelUp Atributes";

        inventory = Instantiate(prefabInventory, transform);
        inventory.Initialize(this, inventoryData);

        RecalculateStats();
    }

    public override Sprite GetProfileImage()
    {
        return dbData.profilePicture;
    }

    public void RecalculateStats()
    {
        AttributeStats.RecalculateStats(attributeStats, levelUpAttributes, dbData.heroClass, inventory);
    }

    public void RecalculateExperience(int amountToAdd)
    {
        experienceStats.experience += amountToAdd;
        levelUps = ExperienceCalculation.CalculateLevelUps(experienceStats.level, experienceStats.experience);
    }

    public AttributeType LevelUp_SelectRandomAttribute()
    {
        AttributeType result = AttributeType.COMMAND;
        bool hasSelection = true;
        do
        {
            //TODO add table for random selection chances
            int aux = UnityEngine.Random.Range(0, 6);
            result = (AttributeType)aux;
            hasSelection = result != dbData.heroClass.GetPrimaryAttribute();
        }
        while (!hasSelection);
        return result;
    }

    public List<AttributeType> LevelUp_ListAttributeOptions(AttributeType pickedAtRandom)
    {
        AttributeType primary = dbData.heroClass.GetPrimaryAttribute();

        List<AttributeType> result = new List<AttributeType>();

        AttributeType[] allValues = (AttributeType[])Enum.GetValues(typeof(AttributeType));
        foreach (AttributeType item in allValues)
        {
            if (item == primary) continue;
            if (item == pickedAtRandom) continue;
            result.Add(item);
        }
        return result;
    }

    public bool LevelUp_ApplyLevelUp(AttributeType randomSelection, AttributeType userSelection)
    {
        if (levelUps <= 0) return false;

        AttributeType primary = dbData.heroClass.GetPrimaryAttribute();
        LevelUp_ApplyLevelUp(primary);
        LevelUp_ApplyLevelUp(randomSelection);
        LevelUp_ApplyLevelUp(userSelection);

        levelUps--;
        return true;
    }

    private void LevelUp_ApplyLevelUp(AttributeType attributeType)
    {
        switch (attributeType)
        {
            case AttributeType.OFFENSE:
                attributeStats.atrOffense++;
                break;
            case AttributeType.DEFENSE:
                attributeStats.atrDefense++;
                break;
            case AttributeType.SUPPORT:
                attributeStats.atrSupport++;
                break;
            case AttributeType.COMMAND:
                attributeStats.atrCommand++;
                break;
            case AttributeType.MAGIC:
                attributeStats.atrMagic++;
                break;
            case AttributeType.TECH:
                attributeStats.atrTech++;
                break;
        }
    }
}
