using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCombatPiece : AbstractPiece
{
    [Header("Sprites")]
    public Sprite imgProfile;
    public Sprite imgCombat;

    protected override void Movement()
    {
        if (CombatManager.Instance.IsCombatRunning())
        {
            base.Movement();
        }
    }
}
