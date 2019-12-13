using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBHandler_CombatObstacle : AbstractDBContentHandler<DB_CombatObstacle>
{
    protected override bool VerifyContent(DB_CombatObstacle item)
    {
        Debug.LogWarning(GetType() + " - No specific content verification was done!");
        return true;
    }
}
