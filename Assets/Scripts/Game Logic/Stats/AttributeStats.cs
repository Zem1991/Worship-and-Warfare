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

    public void Initialize(AttributeStats attributeStats)
    {
        atrCommand = attributeStats.atrCommand;
        atrOffense = attributeStats.atrOffense;
        atrDefense = attributeStats.atrDefense;
        atrPower = attributeStats.atrPower;
        atrFocus = attributeStats.atrFocus;
    }

    public void RecalculateParameters(AttributeStats attributeStats, Inventory inventory)
    {
        atrCommand = attributeStats.atrCommand + inventory.atrCommand;
        atrOffense = attributeStats.atrCommand + inventory.atrOffense;
        atrDefense = attributeStats.atrCommand + inventory.atrDefense;
        atrPower = attributeStats.atrCommand + inventory.atrPower;
        atrFocus = attributeStats.atrCommand + inventory.atrFocus;
    }
}
