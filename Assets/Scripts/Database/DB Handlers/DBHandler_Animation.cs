using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBHandler_Animation : DBContentHandler
{
    protected override Type ContentType()
    {
        return typeof(DB_Animation);
    }

    protected override bool VerifyContent(DBContent item)
    {
        Debug.LogWarning("No specific content verification was done!");
        return true;
    }
}
