using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBHandler_Hero : AbstractDBContentHandler<DB_Hero>
{
    protected override bool VerifyContent(DB_Hero item)
    {
        Debug.LogWarning(GetType() + " - No specific content verification was done!");
        return true;
    }
}
