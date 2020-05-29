using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownDefense : AbstractTownStructure
{
    public void Initialize(DB_TownDefense dbTownDefense)
    {
        base.Initialize(dbTownDefense);
    }

    public DB_TownDefense GetDBTownDefense()
    {
        return dbTownStructure as DB_TownDefense;
    }
}
