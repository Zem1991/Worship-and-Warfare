using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPartyElement : MonoBehaviour
{
    public PartyElementType partyElementType;

    public bool CompareDatabaseEntry(AbstractPartyElement compareWithThis)
    {
        if (partyElementType != compareWithThis.partyElementType) return false;

        switch (partyElementType)
        {
            case PartyElementType.HERO:
                Hero thisHero = this as Hero;
                Hero otherHero = compareWithThis as Hero;
                if (thisHero.dbData == otherHero.dbData) return true;
                break;
            case PartyElementType.CREATURE:
                Unit thisUnit = this as Unit;
                Unit otherUnit = compareWithThis as Unit;
                if (thisUnit.dbData == otherUnit.dbData) return true;
                break;
            //case PartyElementType.SIEGE_ENGINE:
            //    break;
            //default:
            //    break;
        }
        return false;
    }

    public abstract Sprite GetProfileImage();
}
