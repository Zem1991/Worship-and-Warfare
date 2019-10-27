using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_Unit : DBContent
{
    public string nameSingular;
    public string namePlural;
    public Sprite profilePicture;

    [Header("Stats")]
    public CombatPieceStats combatPieceStats;

    [Header("Animations")]
    public AnimatorOverrideController animatorField;
    public AnimatorOverrideController animatorCombat;

    [Header("References")]
    public DB_Faction faction;
}
