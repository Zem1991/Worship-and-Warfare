using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBHandler_Artifact : DBContentHandler<DB_Artifact>
{
    protected override bool VerifyContent(DB_Artifact item)
    {
        Debug.LogWarning(GetType() + " - No specific content verification was done!");
        return true;
    }
}
