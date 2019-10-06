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
        //hasRangedAttack = hero.hasRangedAttack;

        this.owner = owner;
        this.spawnId = spawnId;
        this.defenderSide = defenderSide;

        FlipSpriteHorizontally(defenderSide);
        SetAnimatorOverrideController(hero.dbData.classs.animatorCombat);

        name = "P" + owner.id + " - " + hero.dbData.heroName + ", " + hero.dbData.classs.className;
        //name = "P" + owner.id + " - " + hero.dbData.heroName + ", level " + hero.level + " " + hero.dbData.classs.className;
    }

    public override void PerformPieceInteraction()
    {
        Debug.LogWarning("THIS HERO DOESN'T KNOW HOW TO HANDLE OTHER PIECES!");
        //CombatPieceManager.Instance.UnitsAreInteracting(this, target as HeroCombatPiece);
    }

    public override void MakeAttack()
    {
        return;
        throw new System.NotImplementedException();
    }

    public override void MakeHurt()
    {
        return;
        throw new System.NotImplementedException();
    }

    public override int CalculateDamage()
    {
        throw new System.NotImplementedException();
    }

    public override void Attack(bool ranged)
    {
        throw new System.NotImplementedException();
    }

    public override bool TakeDamage(float amount)
    {
        throw new System.NotImplementedException();
    }

    public override void Retaliate()
    {
        throw new System.NotImplementedException();
    }

    public override void Die()
    {
        throw new System.NotImplementedException();
    }
}
