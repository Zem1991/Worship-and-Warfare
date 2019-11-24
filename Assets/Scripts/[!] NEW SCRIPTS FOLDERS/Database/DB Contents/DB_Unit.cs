using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_Unit : AbstractDBContent
{
    public string nameSingular;
    public string namePlural;
    public Sprite profilePicture;

    [Header("Unit settings")]
    public int tier;

    [Header("Stats")]
    public CombatPieceStats combatPieceStats;

    [Header("Animations")]
    public AnimatorOverrideController animatorField;
    public AnimatorOverrideController animatorCombat;

    [Header("References")]
    public DB_Faction faction;
}
