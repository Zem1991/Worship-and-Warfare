using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBHandler_Color : DBContentHandler<DB_Color>
{
    protected override bool VerifyContent(DB_Color item)
    {
        Debug.LogWarning(GetType() + " - No specific content verification was done!");
        return true;
    }
}
