using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBHandler_Trait : DBContentHandler<DB_Trait>
{
    protected override bool VerifyContent(DB_Trait item)
    {
        Debug.LogWarning(GetType() + " - No specific content verification was done!");
        return true;
    }
}
