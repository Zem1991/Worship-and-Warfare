using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeStats2 : MonoBehaviour
{
    [Header("Attributes")]
    public AttributeStruct attributes;

    public void CopyFrom(AttributeStats2 attributeStats)
    {
        attributes = new AttributeStruct
        {
            offense = attributeStats.attributes.offense,
            defense = attributeStats.attributes.defense,
            support = attributeStats.attributes.support,
            command = attributeStats.attributes.command,
            magic = attributeStats.attributes.magic,
            tech = attributeStats.attributes.tech
        };
    }

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

    public void RecalculateStats(LevelUpStats2 levelUp, DB_HeroUnit dbHeroClass, Inventory inventory)
    {
        attributes.offense = levelUp.attributes.offense + dbHeroClass.attributeStats.attributes.offense + inventory.equipAttributeStats.attributes.offense;
        attributes.defense = levelUp.attributes.defense + dbHeroClass.attributeStats.attributes.defense + inventory.equipAttributeStats.attributes.defense;
        attributes.support = levelUp.attributes.support + dbHeroClass.attributeStats.attributes.support + inventory.equipAttributeStats.attributes.support;
        attributes.command = levelUp.attributes.command + dbHeroClass.attributeStats.attributes.command + inventory.equipAttributeStats.attributes.command;
        attributes.magic = levelUp.attributes.magic + dbHeroClass.attributeStats.attributes.magic + inventory.equipAttributeStats.attributes.magic;
        attributes.tech = levelUp.attributes.tech + dbHeroClass.attributeStats.attributes.tech + inventory.equipAttributeStats.attributes.tech;
    }
}
