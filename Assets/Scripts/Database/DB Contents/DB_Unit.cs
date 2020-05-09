using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_Unit : AbstractDBContent
{
    [Header("Unit identification")]
    public Sprite profilePicture;

    [Header("Unit stats")]
    public ResourceStats resourceStats;
    public CombatPieceStats combatPieceStats;

    [Header("Unit animations")]
    public AnimatorOverrideController animatorField;
    public AnimatorOverrideController animatorCombat;

    [Header("Unit references")]
    public DB_Faction faction;
}
