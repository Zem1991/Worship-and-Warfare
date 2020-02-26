using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBHandler_Resource : AbstractDBContentHandler<DB_Resource>
{
    protected override bool VerifyContent(DB_Resource item)
    {
        Debug.LogWarning(GetType() + " - No specific content verification was done!");
        return true;
    }
}
