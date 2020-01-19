using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBHandler_Faction : AbstractDBContentHandler<DB_Faction>
{
    protected override bool VerifyContent(DB_Faction item)
    {
        Debug.LogWarning(GetType() + " - No specific content verification was done!");
        return true;
    }
}
