using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBHandler_CombatUnit : AbstractDBContentHandler<DB_CombatUnit>
{
    protected override bool VerifyContent(DB_CombatUnit item)
    {
        Debug.LogWarning(GetType() + " - No specific content verification was done!");
        return true;
    }
}
