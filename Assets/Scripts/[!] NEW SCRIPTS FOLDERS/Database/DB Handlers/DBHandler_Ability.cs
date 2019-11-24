using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBHandler_Ability : AbstractDBContentHandler<DB_Ability>
{
    protected override bool VerifyContent(DB_Ability item)
    {
        Debug.LogWarning(GetType() + " - No specific content verification was done!");
        return true;
    }
}
