using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBHandler_HeroUnit : AbstractDBContentHandler<DB_HeroUnit>
{
    protected override bool VerifyContent(DB_HeroUnit item)
    {
        Debug.LogWarning(GetType() + " - No specific content verification was done!");
        return true;
    }
}
