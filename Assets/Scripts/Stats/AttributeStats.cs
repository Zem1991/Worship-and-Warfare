﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeStats : MonoBehaviour
{
    [Header("Attributes")]
    public int atrOffense;
    public int atrDefense;
    public int atrSupport;
    public int atrCommand;
    public int atrMagic;
    public int atrTech;

    public void Initialize(AttributeStats attributeStats)
    {
        atrOffense = attributeStats.atrOffense;
        atrDefense = attributeStats.atrDefense;
        atrSupport = attributeStats.atrSupport;
        atrCommand = attributeStats.atrCommand;
        atrMagic = attributeStats.atrMagic;
        atrTech = attributeStats.atrTech;
    }

    public static void RecalculateStats(AttributeStats attributeStats, AttributeStats levelUp, DB_HeroClass dbHeroClass, Inventory inventory)
    {
        attributeStats.atrOffense = levelUp.atrOffense +    dbHeroClass.attributeStats.atrOffense +     inventory.attributeStats.atrOffense;
        attributeStats.atrDefense = levelUp.atrDefense +    dbHeroClass.attributeStats.atrDefense +     inventory.attributeStats.atrDefense;
        attributeStats.atrSupport = levelUp.atrSupport +    dbHeroClass.attributeStats.atrSupport +     inventory.attributeStats.atrSupport;
        attributeStats.atrCommand = levelUp.atrCommand +    dbHeroClass.attributeStats.atrCommand +     inventory.attributeStats.atrCommand;
        attributeStats.atrMagic =   levelUp.atrMagic +      dbHeroClass.attributeStats.atrMagic +       inventory.attributeStats.atrMagic;
        attributeStats.atrTech =    levelUp.atrTech +       dbHeroClass.attributeStats.atrTech +        inventory.attributeStats.atrTech;
    }
}
