using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBHandler_HeroPerson : AbstractDBContentHandler<DB_HeroPerson>
{
    protected override bool VerifyContent(DB_HeroPerson item)
    {
        Debug.LogWarning(GetType() + " - No specific content verification was done!");
        return true;
    }
}
