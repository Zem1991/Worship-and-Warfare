using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBHandler_Tech : AbstractDBContentHandler<DB_Tech>
{
    protected override bool VerifyContent(DB_Tech item)
    {
        Debug.LogWarning(GetType() + " - No specific content verification was done!");
        return true;
    }
}
