using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBHandler_Class : DBContentHandler<DB_Class>
{
    protected override bool VerifyContent(DB_Class item)
    {
        Debug.LogWarning(GetType() + " - No specific content verification was done!");
        return true;
    }
}
