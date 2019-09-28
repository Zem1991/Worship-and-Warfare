using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatHeroPiece : AbstractCombatPiece
{
    [Header("Hero data")]
    public Hero hero;

    public void Initialize(Hero hero, bool defenderSide = false)
    {
        this.hero = hero;
        FlipSpriteHorizontally(defenderSide);
        SetAnimatorOverrideController(hero.animatorCombat);
    }

    public override void InteractWithPiece(AbstractPiece target)
    {
        Debug.LogWarning("THIS HERO DOESN'T KNOW HOW TO HANDLE OTHER PIECES!");
        //CombatPieceManager.Instance.UnitsAreInteracting(this, target as HeroCombatPiece);
    }

    public override int CalculateDamage()
    {
        throw new System.NotImplementedException();
    }

    public override bool TakeDamage(float amount)
    {
        throw new System.NotImplementedException();
    }

    public override void Die()
    {
        throw new System.NotImplementedException();
    }
}
