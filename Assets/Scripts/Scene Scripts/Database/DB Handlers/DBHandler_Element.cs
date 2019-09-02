using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBHandler_Element : DBContentHandler<DB_Element>
{
    protected override bool VerifyContent(DB_Element item)
    {
        Debug.LogWarning(GetType() + " - No specific content verification was done!");
        return true;
    }
}
