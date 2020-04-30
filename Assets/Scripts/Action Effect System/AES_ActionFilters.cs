using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AES_ActionFilters : MonoBehaviour
{
    [Header("Active filters")]
    public bool hasFilterUnitCategory;
    public bool hasFilterUnitType;

    [Header("Filter details")]
    public UnitCategory filterUnitCategory;
    public UnitType filterUnitType;
    public UnitSubtype filterUnitSubtype;

    public bool ApplyFilters(AbstractPartyElement target)
    {
        return true;
    }
}
