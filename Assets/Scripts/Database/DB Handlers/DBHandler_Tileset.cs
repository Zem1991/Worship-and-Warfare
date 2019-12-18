using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBHandler_Tileset : AbstractDBContentHandler<DB_Tileset>
{
    protected override bool VerifyContent(DB_Tileset item)
    {
        Debug.LogWarning(GetType() + " - No specific content verification was done!");
        return true;
    }
}
