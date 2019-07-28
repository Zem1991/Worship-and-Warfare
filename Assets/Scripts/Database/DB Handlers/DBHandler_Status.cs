using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBHandler_Status : DBContentHandler<DB_Status>
{
    protected override bool VerifyContent(DB_Status item)
    {
        Debug.LogWarning(GetType() + " - No specific content verification was done!");
        return true;
    }
}
