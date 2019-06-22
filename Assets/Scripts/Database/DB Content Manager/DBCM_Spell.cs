using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBCM_Spell : DBContentManager
{
    protected override Type ContentType()
    {
        return typeof(DB_Spell);
    }

    protected override bool VerifyContent(DBContent item)
    {
        Debug.LogWarning("No specific content verification was done!");
        return true;
    }
}
