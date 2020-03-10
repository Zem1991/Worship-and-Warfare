using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBHandler_HeroClass : AbstractDBContentHandler<DB_HeroClass>
{
    protected override bool VerifyContent(DB_HeroClass item)
    {
        Debug.LogWarning(GetType() + " - No specific content verification was done!");
        return true;
    }
}
