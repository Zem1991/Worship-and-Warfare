using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AES_ActionFilters : MonoBehaviour
{
    [Header("Active filters")]
    public bool hasFilterUnitCategory;
    public bool hasFilterUnitType;

    [Header("Filter details")]
    public UnitType filterUnitCategory;
    public UnitComposition filterUnitType;
    public UnitSubcomposition filterUnitSubtype;

    public bool ApplyFilters(AbstractUnit target)
    {
        return true;
    }
}
