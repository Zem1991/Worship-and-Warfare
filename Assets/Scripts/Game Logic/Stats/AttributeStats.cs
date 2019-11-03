using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeStats : MonoBehaviour
{
    [Header("Attributes")]
    public int atrCommand;
    public int atrOffense;
    public int atrDefense;
    public int atrPower;
    public int atrFocus;
    public int atrCraft;

    public void Initialize(AttributeStats attributeStats)
    {
        atrCommand = attributeStats.atrCommand;
        atrOffense = attributeStats.atrOffense;
        atrDefense = attributeStats.atrDefense;
        atrPower = attributeStats.atrPower;
        atrFocus = attributeStats.atrFocus;
    }

    public void RecalculateStats(AttributeStats attributeStats, Inventory inventory)
    {
        atrCommand = attributeStats.atrCommand + inventory.atrCommand;
        atrOffense = attributeStats.atrOffense + inventory.atrOffense;
        atrDefense = attributeStats.atrDefense + inventory.atrDefense;
        atrPower = attributeStats.atrPower + inventory.atrPower;
        atrFocus = attributeStats.atrFocus + inventory.atrFocus;
    }
}
