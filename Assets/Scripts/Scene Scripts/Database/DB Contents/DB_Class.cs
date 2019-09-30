using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_Class : DBContent
{
    public string className;
    public DB_Faction faction;

    [Header("Stats")]
    public int atrCommand;
    public int atrOffense;
    public int atrDefense;
    public int atrPower;
    public int atrFocus;
    //public int logistics;     //really needed?

    [Header("Animations")]
    public AnimatorOverrideController animatorField;
    public AnimatorOverrideController animatorCombat;
}
