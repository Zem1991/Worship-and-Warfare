using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AES_ActionParameters : MonoBehaviour
{
    //[Header("Active parameters")]
    //public bool hasParamFormula;
    //public bool hasParamDmgType;
    //public bool hasParamHealType;
    //public bool hasParamCreature;

    //[Header("Parameter details")]
    [Header("Parameters")]
    public AES_Formula paramFormula;
    public DamageType paramDmgType;
    public DamageElement paramDmgElement;
    public HealType paramHealType;
    public DB_CombatUnit paramCreature;
}
