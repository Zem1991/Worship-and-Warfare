using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBHandler_TownDefense : AbstractDBContentHandler<DB_TownDefense>
{
    protected override bool VerifyContent(DB_TownDefense item)
    {
        Debug.LogWarning(GetType() + " - No specific content verification was done!");
        return true;
    }
}
