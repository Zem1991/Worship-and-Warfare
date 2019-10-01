using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUnitPiece : AbstractCombatPiece
{
    [Header("Unit data")]
    public Unit unit;

    public override void Update()
    {
        base.Update();
        Attack();
        Hurt();
    }

    public void Initialize(Unit unit, Player owner, int spawnId, bool defenderSide = false)
    {
        this.unit = unit;
        imgProfile = unit.imgProfile;
        hasRangedAttack = unit.hasRangedAttack;

        this.owner = owner;
        this.spawnId = spawnId;
        this.defenderSide = defenderSide;

        FlipSpriteHorizontally(defenderSide);
        SetAnimatorOverrideController(unit.animatorCombat);
    }

    private void Attack()
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

    private void Hurt()
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

    public override void InteractWithPiece(AbstractPiece target)
    {
        CombatUnitPiece targetUnit = target as CombatUnitPiece;
        if (targetUnit)
        {
            if (targetUnit.owner != owner)
            {
                isAttacking_Start = true;
                actionTarget = targetUnit;
                //Debug.LogWarning("InteractWithPiece insta-killed the target!");
                //targetUnit.hitPointsCurrent = 0;
            }
        }
        else
        {
            Debug.LogWarning("InteractWithPiece IS DESTROYING PIECES!");
            Destroy(target.gameObject);
        }
    }

    public override int CalculateDamage()
    {
        CombatUnitPiece targetUnit = actionTarget as CombatUnitPiece;
        CombatPieceHandler cph = CombatManager.Instance.pieceHandler;
        return CombatLogic.DamageCalculation(unit, targetUnit.unit, cph.attackerHero?.hero, cph.defenderHero?.hero);
    }

    public override bool TakeDamage(float amount)
    {
        int amountFixed = Mathf.CeilToInt(amount);
        int stackLost = amountFixed / unit.hitPointsMax;
        int hpLost = amountFixed % unit.hitPointsMax;
        unit.hitPointsCurrent -= hpLost;
        if (unit.hitPointsCurrent <= 0)
        {
            stackLost++;
            unit.hitPointsCurrent += unit.hitPointsMax;
        }
        unit.stackSizeCurrent -= stackLost;
        string log = unit.GetName() + " took " + amountFixed + " damage. " + stackLost + " units died.";
        CombatManager.Instance.AddEntryToLog(log);

        if (unit.stackSizeCurrent > 0)
        {
            isHurt = true;
            return false;
        }
        else
        {
            Die();
            return true;
        }
    }

    public override void Die()
    {
        unit.stackSizeCurrent = 0;
        unit.hitPointsCurrent = 0;
        isDead = true;

        spriteRenderer.sortingOrder--;

        currentTile.occupantPiece = null;
        (currentTile as CombatTile).deadPieces.Add(this);
    }
}
