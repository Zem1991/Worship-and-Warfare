using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBHandler_Animation : DBContentHandler<DB_Animation>
{
    protected override bool VerifyContent(DB_Animation item)
    {
        Debug.LogWarning(GetType() + " - No specific content verification was done!");
        return true;
    }
}
