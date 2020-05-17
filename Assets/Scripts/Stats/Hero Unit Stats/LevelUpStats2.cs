using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpStats2 : MonoBehaviour
{
    [Header("Experience")]
    public int level = 1;
    public int experience = 0;

    [Header("Increased attributes")]
    public AttributeStruct attributes;

    public void Initialize(LevelUpData levelUpData)
    {
        if (levelUpData == null) return;

        level = levelUpData.level;
        experience = ExperienceCalculation.CalculateExperienceToLevel(level);

        attributes = new AttributeStruct
        {
            offense = levelUpData.offense,
            defense = levelUpData.defense,
            support = levelUpData.support,
            command = levelUpData.command,
            magic = levelUpData.magic,
            tech = levelUpData.tech
        };
    }

    public void Increment(AttributeType attrType)
    {
        attributes.Increment(attrType);
    }

    //public void Decrement(AttributeType attrType)
    //{
    //    attributes.Decrement(attrType);
    //}
}
