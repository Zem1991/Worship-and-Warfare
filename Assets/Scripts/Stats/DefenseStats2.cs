using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseStats2 : MonoBehaviour
{
    public int armor_physical;
    public int armor_magical;

    public void CopyFrom(DefenseStats2 defenseStats)
    {
        armor_physical = defenseStats.armor_physical;
        armor_magical = defenseStats.armor_magical;
    }

    //public void Initialize(int armor_physical, int armor_magical)
    //{
    //    this.armor_physical = armor_physical;
    //    this.armor_magical = armor_magical;
    //}
}
