using System;
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

    public void Copy(AttributeStats attributeStats)
    {
        atrOffense = attributeStats.atrOffense;
        atrDefense = attributeStats.atrDefense;
        atrSupport = attributeStats.atrSupport;
        atrCommand = attributeStats.atrCommand;
        atrMagic = attributeStats.atrMagic;
        atrTech = attributeStats.atrTech;
    }

    public static void RecalculateStats(AttributeStats attributeStats, AttributeStats levelUp, DB_HeroUnit dbHeroClass, Inventory inventory)
    {
        attributeStats.atrOffense = levelUp.atrOffense +    dbHeroClass.attributeStats.atrOffense +     inventory.equipAttributeStats.atrOffense;
        attributeStats.atrDefense = levelUp.atrDefense +    dbHeroClass.attributeStats.atrDefense +     inventory.equipAttributeStats.atrDefense;
        attributeStats.atrSupport = levelUp.atrSupport +    dbHeroClass.attributeStats.atrSupport +     inventory.equipAttributeStats.atrSupport;
        attributeStats.atrCommand = levelUp.atrCommand +    dbHeroClass.attributeStats.atrCommand +     inventory.equipAttributeStats.atrCommand;
        attributeStats.atrMagic =   levelUp.atrMagic +      dbHeroClass.attributeStats.atrMagic +       inventory.equipAttributeStats.atrMagic;
        attributeStats.atrTech =    levelUp.atrTech +       dbHeroClass.attributeStats.atrTech +        inventory.equipAttributeStats.atrTech;
    }
}
