using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_Class : DBContent
{
    public string className;

    [Header("Stats")]
    public CombatPieceStats combatPieceStats;
    public AttributeStats attributeStats;

    [Header("Animations")]
    public AnimatorOverrideController animatorField;
    public AnimatorOverrideController animatorCombat;

    [Header("References")]
    public DB_Faction faction;
}
