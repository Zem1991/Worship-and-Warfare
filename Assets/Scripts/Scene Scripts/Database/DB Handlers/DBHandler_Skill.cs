using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBHandler_Skill : DBContentHandler<DB_Skill>
{
    protected override bool VerifyContent(DB_Skill item)
    {
        Debug.LogWarning(GetType() + " - No specific content verification was done!");
        return true;
    }
}
