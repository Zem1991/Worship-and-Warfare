using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractTownStructure : MonoBehaviour
{
    [Header("Database reference")]
    public DB_TownStructure dbTownStructure;

    public void Initialize(DB_TownStructure dbTownStructure)
    {
        this.dbTownStructure = dbTownStructure;
        name = dbTownStructure.structureName;
    }
}
