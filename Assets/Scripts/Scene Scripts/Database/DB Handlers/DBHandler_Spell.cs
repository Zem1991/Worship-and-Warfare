using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBHandler_Spell : DBContentHandler<DB_Spell>
{
    protected override bool VerifyContent(DB_Spell item)
    {
        Debug.LogWarning(GetType() + " - No specific content verification was done!");
        return true;
    }
}
