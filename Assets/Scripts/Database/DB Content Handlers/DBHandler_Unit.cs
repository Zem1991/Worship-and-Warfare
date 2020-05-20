using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBHandler_Unit : AbstractDBContentHandler<DB_Unit>
{
    protected override bool VerifyContent(DB_Unit item)
    {
        Debug.LogWarning(GetType() + ": No specific content verification was done!");
        return true;
    }
}
