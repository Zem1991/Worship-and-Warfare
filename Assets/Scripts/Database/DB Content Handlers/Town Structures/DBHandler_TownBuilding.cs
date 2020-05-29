using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBHandler_TownBuilding : AbstractDBContentHandler<DB_TownBuilding>
{
    protected override bool VerifyContent(DB_TownBuilding item)
    {
        Debug.LogWarning(GetType() + " - No specific content verification was done!");
        return true;
    }
}
