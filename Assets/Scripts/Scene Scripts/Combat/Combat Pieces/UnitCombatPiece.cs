using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCombatPiece : AbstractCombatPiece
{
    //public bool didMove;
    //public bool didAttack;

    [Header("Unit combat stats")]
    public string unitName;
    public int hitPoints;
    public int hitPointsCurrent;
    public int stackSize;
    public int stackSizeCurrent;
    public int damageMin;
    public int damageMax;
    public int resistance;
    public int speed;
    public int initiative;

    public override void Update()
    {
        base.Update();
        Attack();
        Hurt();
    }

    public void Initialize(Unit unit)
    {
        unitName = unit.unitName;
        hitPoints = unit.hitPoints;
        hitPointsCurrent = hitPoints;
        stackSize = unit.stackSize;
        stackSizeCurrent = stackSize;

        damageMin = unit.damageMin;
        damageMax = unit.damageMax;
        resistance = unit.resistance;
        speed = unit.speed;
        initiative = unit.initiative;

        imgProfile = unit.imgProfile;
        SetAnimatorOverrideController(unit.animatorCombat);
    }

    public override int CalculateDamage()
    {
        UnitCombatPiece targetUnit = actionTarget as UnitCombatPiece;
        CombatPieceHandler cph = CombatManager.Instance.pieceHandler;
        return CombatLogic.DamageCalculation(this, targetUnit, cph.attackerHero, cph.defenderHero);
    }

    public override bool TakeDamage(float amount)
    {
        hitPoints -= Mathf.CeilToInt(amount);
        hitPoints = Mathf.Max(hitPoints, 0);
        Debug.Log("Unit " + unitName + " took " + amount + " damage, and now has " + hitPoints + " hit points.");
        if (hitPoints > 0)
        {
            isHurt = true;
            return false;
        }
        else
        {
            isDead = true;
            return true;
        }
    }

    protected override void InteractWithPiece(AbstractPiece target)
    {
        UnitCombatPiece targetUnit = target as UnitCombatPiece;
        if (targetUnit)
        {
            isAttacking_Start = true;
            actionTarget = targetUnit;
            //Debug.LogWarning("InteractWithPiece insta-killed the target!");
            //targetUnit.hitPointsCurrent = 0;
        }
        else
        {
            Debug.LogWarning("InteractWithPiece IS DESTROYING PIECES!");
            Destroy(target.gameObject);
        }
    }

    protected void Attack()
    {
        if (!isAttacking_Start && !isAttacking_End) return;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if (state.normalizedTime >= 1)
        {
            if (isAttacking_Start &&
                state.IsName("Attack Start"))
            {
                isAttacking_Start = false;
                isAttacking_End = true;
                float dmg = CalculateDamage();
                actionTarget.TakeDamage(dmg);
            }
            if (isAttacking_End &&
                state.IsName("Attack End"))
            {
                isAttacking_End = false;
                actionTarget = null;
            }
        }
    }

    protected void Hurt()
    {
        if (!isHurt) return;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if (state.normalizedTime >= 1)
        {
            if (state.IsName("Hurt"))
            {
                isHurt = false;
            }
        }
    }
}
