using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBHandler_HealAbility : AbstractDBContentHandler<DB_HealAbility>
{
    protected override bool VerifyContent(DB_HealAbility item)
    {
        Debug.LogWarning(GetType() + " - No specific content verification was done!");
        return true;
    }
}
