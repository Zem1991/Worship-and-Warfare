using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatantUnitPiece2 : AbstractCombatantPiece2
{
    [Header("Unit data")]
    public Unit unit;

    [Header("Stack size management")]
    public int stackSizeCurrent;
    public int stackSizeStart;

    public void Initialize(Player owner, Unit unit, int spawnId, bool defenderSide = false)
    {
        this.owner = owner;
        this.unit = unit;

        profilePicture = unit.dbData.profilePicture;

        hitPointsMax = unit.hitPointsMax;
        hitPointsCurrent = hitPointsMax;

        this.spawnId = spawnId;
        this.defenderSide = defenderSide;
        hasRangedAttack = unit.hasRangedAttack;

        stackSizeStart = unit.stackSizeCurrent;
        hitPointsCurrent = stackSizeStart;

        ACtP_ResetMovementPoints();
        FlipSpriteHorizontally(defenderSide);
        SetAnimatorOverrideController(unit.dbData.animatorCombat);

        name = "P" + owner.id + " - Stack of " + unit.GetName();
        //name = "P" + owner.id + " - Stack of " + unit.stackSizeCurrent + " " + unit.GetName();
    }

    public override bool ACP_TakeDamage(float amount)
    {
        int amountFixed = Mathf.CeilToInt(amount);
        int stackLost = amountFixed / unit.hitPointsMax;
        int hpLost = amountFixed % unit.hitPointsMax;
        hitPointsCurrent -= hpLost;
        if (hitPointsCurrent <= 0)
        {
            stackLost++;
            hitPointsCurrent += hitPointsMax;
        }
        stackSizeCurrent -= stackLost;
        string log = unit.GetName() + " took " + amountFixed + " damage. " + stackLost + " units died.";
        CombatManager.Instance.AddEntryToLog(log);

        if (stackSizeCurrent > 0)
        {
            isHurt = true;
            return false;
        }
        else
        {
            ACP_Die();
            return true;
        }
    }

    public override void ACP_Die()
    {
        stackSizeCurrent = 0;
        hitPointsCurrent = 0;
        isDead = true;
        spriteRenderer.sortingOrder--;

        currentTile.occupantPiece = null;
        (currentTile as CombatTile).deadPieces.Add(this);
        CombatManager.Instance.RemoveUnitFromTurnSequence(this);
    }

    public override void ACtP_ResetMovementPoints()
    {
        pieceMovement.movementPointsMax = unit.movementRange * 100;
        pieceMovement.movementPointsCurrent = pieceMovement.movementPointsMax;
    }

    public override void ACtP_MakeAttack()
    {
        if (!isAttacking_Start && !isAttacking_End) return;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if (state.normalizedTime >= 1)
        {
            AbstractCombatPiece2 acp = pieceMovement.targetPiece as AbstractCombatPiece2;
            AbstractCombatantPiece2 actp = pieceMovement.targetPiece as AbstractCombatantPiece2;

            if (isAttacking_Start &&
                state.IsName("Attack Start"))
            {
                isAttacking_Start = false;
                isAttacking_End = true;
                float dmg = ACtP_CalculateDamage();
                acp.ACP_TakeDamage(dmg);
            }
            if (isAttacking_End &&
                state.IsName("Attack End"))
            {
                isAttacking_End = false;
                pieceMovement.targetPiece = null;
                if (actp && actp.retaliationTarget && !actp.isDead)
                {
                    actp.ACtP_Retaliate();
                }
                else
                {
                     ICP_EndTurn();
                }
            }
        }
    }

    public override void ACtP_MakeHurt()
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

    public override int ACtP_CalculateDamage()
    {
        CombatantUnitPiece2 targetUnit = pieceMovement.targetPiece as CombatantUnitPiece2;
        CombatPieceHandler cph = CombatManager.Instance.pieceHandler;
        Hero attackerHero = cph.GetHero(owner);
        Hero defenderHero = cph.GetHero(targetUnit.owner);
        return CombatLogic.DamageCalculation(unit, targetUnit.unit, attackerHero, defenderHero);
    }

    public override void ACtP_Attack(bool ranged)
    {
        AbstractCombatantPiece2 actp = pieceMovement.targetPiece as AbstractCombatantPiece2;

        if (!ranged && actp)
        {
            CombatManager.Instance.retaliatorPiece = actp;
            actp.retaliationTarget = this;
        }
        isAttacking_Start = true;
        //Debug.LogWarning("InteractWithPiece insta-killed the target!");
        //targetUnit.hitPointsCurrent = 0;
    }

    public override void ACtP_Retaliate()
    {
        pieceMovement.targetPiece = retaliationTarget;
        retaliationTarget = null;
        isAttacking_Start = true;
    }
}
