using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeStats2 : MonoBehaviour
{
    [Header("Attributes")]
    public AttributeStruct attributes;

    //public void Initialize(DB_Unit dbUnit)        //TODO: don't remember if I need this at all
    //{
    //    if (levelUpData == null) return;

    //    level = levelUpData.level;
    //    experience = ExperienceCalculation.CalculateExperienceToLevel(level);

    //    attributes = new AttributeStruct
    //    {
    //        offense = dbUnit.offense,
    //        defense = levelUpData.defense,
    //        support = levelUpData.support,
    //        command = levelUpData.command,
    //        magic = levelUpData.magic,
    //        tech = levelUpData.tech
    //    };
    //}

    public static void RecalculateStats(AttributeStats2 attributeStats, LevelUpStats2 levelUp, DB_HeroUnit dbHeroClass, Inventory inventory)
    {
        attributeStats.attributes.offense = levelUp.attributes.offense + dbHeroClass.attributeStats.attributes.offense + inventory.equipAttributeStats.attributes.offense;
        attributeStats.attributes.defense = levelUp.attributes.defense + dbHeroClass.attributeStats.attributes.defense + inventory.equipAttributeStats.attributes.defense;
        attributeStats.attributes.support = levelUp.attributes.support + dbHeroClass.attributeStats.attributes.support + inventory.equipAttributeStats.attributes.support;
        attributeStats.attributes.command = levelUp.attributes.command + dbHeroClass.attributeStats.attributes.command + inventory.equipAttributeStats.attributes.command;
        attributeStats.attributes.magic = levelUp.attributes.magic + dbHeroClass.attributeStats.attributes.magic + inventory.equipAttributeStats.attributes.magic;
        attributeStats.attributes.tech = levelUp.attributes.tech + dbHeroClass.attributeStats.attributes.tech + inventory.equipAttributeStats.attributes.tech;
    }
}
