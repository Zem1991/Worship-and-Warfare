using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatantHeroPiece2 : AbstractCombatantPiece2
{
    [Header("Hero data")]
    public Hero hero;

    public void Initialize(Hero hero, Player owner, int spawnId, bool defenderSide = false)
    {
        this.owner = owner;
        this.hero = hero;

        this.spawnId = spawnId;
        this.defenderSide = defenderSide;
        //initiative = hero.initiative;             //TODO LATER ?
        //hasRangedAttack = hero.hasRangedAttack;   //TODO LATER ?

        ACtP_ResetMovementPoints();
        FlipSpriteHorizontally(defenderSide);
        SetAnimatorOverrideController(hero.dbData.classs.animatorCombat);

        name = "P" + owner.id + " - " + hero.dbData.heroName + ", " + hero.dbData.classs.className;
        //name = "P" + owner.id + " - " + hero.dbData.heroName + ", level " + hero.level + " " + hero.dbData.classs.className;
    }

    public override void ACtP_ResetMovementPoints()
    {
        if (!pieceMovement) pieceMovement = GetComponent<PieceMovement>();
        Debug.Log("throw new System.NotImplementedException()");
    }

    public override void ACtP_MakeAttack()
    {
        Debug.Log("throw new System.NotImplementedException()");
    }

    public override void ACtP_MakeHurt()
    {
        Debug.Log("throw new System.NotImplementedException()");
    }

    public override int ACtP_CalculateDamage()
    {
        throw new System.NotImplementedException();
    }

    public override void ACtP_Attack(bool ranged)
    {
        throw new System.NotImplementedException();
    }

    public override void ACtP_Retaliate()
    {
        throw new System.NotImplementedException();
    }
}
