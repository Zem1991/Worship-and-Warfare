using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownBuilding : MonoBehaviour
{
    //public string buildingName;
    //public int level;

    [Header("Database reference")]
    public DB_TownBuilding dbTownBuilding;

    public void Initialize(DB_TownBuilding dbTownBuilding)
    {
        this.dbTownBuilding = dbTownBuilding;

        name = dbTownBuilding.townBuildingName;
    }
}
