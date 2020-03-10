using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBHandler_Attribute : AbstractDBContentHandler<DB_Attribute>
{
    [Header("Quick records")]
    public DB_Attribute offense;
    public DB_Attribute defense;
    public DB_Attribute support;
    public DB_Attribute command;
    public DB_Attribute magic;
    public DB_Attribute tech;

    protected override bool VerifyContent(DB_Attribute item)
    {
        Debug.LogWarning(GetType() + " - No specific content verification was done!");
        return true;
    }

    public DB_Attribute SelectFromType(AttributeType attributeType)
    {
        DB_Attribute result = null;
        switch (attributeType)
        {
            case AttributeType.OFFENSE:
                result = offense;
                break;
            case AttributeType.DEFENSE:
                result = defense;
                break;
            case AttributeType.SUPPORT:
                result = support;
                break;
            case AttributeType.COMMAND:
                result = command;
                break;
            case AttributeType.MAGIC:
                result = magic;
                break;
            case AttributeType.TECH:
                result = tech;
                break;
        }
        return result;
    }
}
