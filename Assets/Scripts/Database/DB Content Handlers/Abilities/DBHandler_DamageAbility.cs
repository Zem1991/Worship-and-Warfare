using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBHandler_DamageAbility : AbstractDBContentHandler<DB_DamageAbility>
{
    protected override bool VerifyContent(DB_DamageAbility item)
    {
        Debug.LogWarning(GetType() + " - No specific content verification was done!");
        return true;
    }
}
