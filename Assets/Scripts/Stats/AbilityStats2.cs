using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityStats2 : MonoBehaviour
{
    public DB_Ability ability1;
    public DB_Ability ability2;
    public DB_Ability ability3;

    public void CopyFrom(AbilityStats2 abilityStats)
    {
        ability1 = abilityStats.ability1;
        ability2 = abilityStats.ability2;
        ability3 = abilityStats.ability3;
    }

    //public void Initialize(DB_Ability ability1, DB_Ability ability2, DB_Ability ability3)
    //{
    //    this.ability1 = ability1;
    //    this.ability2 = ability2;
    //    this.ability3 = ability3;
    //}

    public DB_Ability GetFromId(int id)
    {
        DB_Ability result = null;
        if (id == 1) result = ability1;
        if (id == 2) result = ability2;
        if (id == 3) result = ability3;
        return result;
    }
}
