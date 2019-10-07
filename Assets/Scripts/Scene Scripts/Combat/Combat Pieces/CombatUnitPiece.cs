using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUnitPiece : AbstractCombatPiece
{
    [Header("Unit data")]
    public Unit unit;

    public void Initialize(Player owner, Unit unit, int spawnId, bool defenderSide = false)
    {
        this.owner = owner;
        this.unit = unit;
        this.spawnId = spawnId;
        this.defenderSide = defenderSide;

        hasRangedAttack = unit.hasRangedAttack;

        FlipSpriteHorizontally(defenderSide);
        SetAnimatorOverrideController(unit.dbData.animatorCombat);

        name = "P" + owner.id + " - Stack of " + unit.GetName();
        //name = "P" + owner.id + " - Stack of " + unit.stackSizeCurrent + " " + unit.GetName();
    }

    public override void PerformPieceInteraction()
    {
        if (targetPiece)
        {
            if (targetPiece.owner != owner)
            {
                //TODO check ranged interaction
                Attack(hasRangedAttack);
            }
        }
    }

    public override void MakeAttack()
    {
        if (!isAttacking_Start && !isAttacking_End) return;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if (state.normalizedTime >= 1)
        {
            AbstractCombatPiece acp = targetPiece as AbstractCombatPiece;
            if (isAttacking_Start &&
                state.IsName("Attack Start"))
            {
                isAttacking_Start = false;
                isAttacking_End = true;
                float dmg = CalculateDamage();
                acp.TakeDamage(dmg);
            }
            if (isAttacking_End &&
                state.IsName("Attack End"))
            {
                isAttacking_End = false;
                targetPiece = null;
                if (acp.retaliationTarget)
                {
                    acp.Retaliate();
                }
                else
                {
                    EndTurn();
                }
            }
        }
    }

    public override void MakeHurt()
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

    public override int CalculateDamage()
    {
        CombatUnitPiece targetUnit = targetPiece as CombatUnitPiece;
        CombatPieceHandler cph = CombatManager.Instance.pieceHandler;
        Hero attackerHero = cph.GetHero(owner);
        Hero defenderHero = cph.GetHero(targetUnit.owner);
        return CombatLogic.DamageCalculation(unit, targetUnit.unit, attackerHero, defenderHero);
    }

    public override void Attack(bool ranged)
    {
        AbstractCombatPiece acp = targetPiece as AbstractCombatPiece;
        if (!ranged) acp.retaliationTarget = this;
        isAttacking_Start = true;
        //Debug.LogWarning("InteractWithPiece insta-killed the target!");
        //targetUnit.hitPointsCurrent = 0;
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

    public override void Retaliate()
    {
        targetPiece = retaliationTarget;
        retaliationTarget = null;
        isAttacking_Start = true;
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
