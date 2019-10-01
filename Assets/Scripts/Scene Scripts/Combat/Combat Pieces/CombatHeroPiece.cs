using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatHeroPiece : AbstractCombatPiece
{
    [Header("Hero data")]
    public Hero hero;

    public void Initialize(Hero hero, Player owner, int spawnId, bool defenderSide = false)
    {
        this.hero = hero;
        imgProfile = hero.imgProfile;
        //hasRangedAttack = hero.hasRangedAttack;

        this.owner = owner;
        this.spawnId = spawnId;
        this.defenderSide = defenderSide;

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
