using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroUnit : AbstractUnit
{
    [Header("Hero parameters")]
    public int levelUps;

    [Header("Stats")]
    public LevelUpStats2 levelUpStats;

    [Header("Inventory")]
    public Inventory inventory;

    [Header("Database reference")]
    public DB_HeroPerson dbHeroPerson;

    public void Initialize(DB_HeroPerson dbHeroPerson, LevelUpData levelUpData, InventoryData inventoryData)
    {
        Inventory prefabInventory = AllPrefabs.Instance.inventory;

        this.dbHeroPerson = dbHeroPerson;
        name = dbHeroPerson.heroName;
        Initialize(dbHeroPerson.heroClass);

        levelUpStats.Initialize(levelUpData);

        inventory = Instantiate(prefabInventory, transform);
        inventory.Initialize(this, inventoryData);

        RecalculateStats();
    }

    public override string AU_GetUnitName()
    {
        return dbHeroPerson.heroName;
    }

    public override Sprite AU_GetProfileImage()
    {
        return dbHeroPerson.profilePicture;
    }

    public void RecalculateStats()
    {
        AttributeStats2.RecalculateStats(attributeStats, levelUpStats, dbHeroPerson.heroClass, inventory);
    }

    public void RecalculateExperience(int amountToAdd = 0)
    {
        levelUpStats.experience += amountToAdd;
        levelUps = ExperienceCalculation.CalculateLevelUps(levelUpStats.level, levelUpStats.experience);
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
            hasSelection = result != dbHeroPerson.heroClass.GetPrimaryAttribute();
        }
        while (!hasSelection);
        return result;
    }

    public List<AttributeType> LevelUp_ListAttributeOptions(AttributeType pickedAtRandom)
    {
        AttributeType primary = dbHeroPerson.heroClass.GetPrimaryAttribute();

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

        AttributeType primary = dbHeroPerson.heroClass.GetPrimaryAttribute();
        LevelUp_ApplyLevelUp(primary);
        LevelUp_ApplyLevelUp(randomSelection);
        LevelUp_ApplyLevelUp(userSelection);

        levelUps--;
        levelUpStats.level++;
        return true;
    }

    private void LevelUp_ApplyLevelUp(AttributeType attributeType)
    {
        levelUpStats.Increment(attributeType);
    }
}
