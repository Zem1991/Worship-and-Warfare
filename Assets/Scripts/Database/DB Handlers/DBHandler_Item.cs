using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBHandler_Item : DBContentHandler<DB_Item>
{
    protected override bool VerifyContent(DB_Item item)
    {
        Debug.LogWarning(GetType() + " - No specific content verification was done!");
        return true;
    }
}
