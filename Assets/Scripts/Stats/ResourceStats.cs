using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceStats : MonoBehaviour
{
    [Header("Costs")]
    public long gold;
    public long ore;
    public long ale;
    public long crystals;
    public long sulphur;

    public void Initialize(ResourceData resourceData)
    {
        if (resourceData == null) return;

        gold = resourceData.gold;
        ore = resourceData.ore;
        ale = resourceData.ale;
        crystals = resourceData.crystals;
        sulphur = resourceData.sulphur;
    }
}
