using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBHandler_Battleground : DBContentHandler<DB_Battleground>
{
    protected override bool VerifyContent(DB_Battleground item)
    {
        Debug.LogWarning(GetType() + " - No specific content verification was done!");
        return true;
    }
}
