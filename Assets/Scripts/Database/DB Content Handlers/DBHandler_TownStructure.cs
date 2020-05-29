using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBHandler_TownStructure : AbstractDBContentHandler<DB_TownStructure>
{
    protected override bool VerifyContent(DB_TownStructure item)
    {
        Debug.LogWarning(GetType() + " - No specific content verification was done!");
        return true;
    }
}
