using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownBuilding : AbstractTownStructure
{
    public void Initialize(DB_TownBuilding dbTownBuilding)
    {
        base.Initialize(dbTownBuilding);
    }

    public DB_TownBuilding GetDBTownBuilding()
    {
        return dbTownStructure as DB_TownBuilding;
    }
}
