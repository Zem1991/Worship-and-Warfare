using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatantUnitPiece2 : AbstractCombatantPiece2
{
    [Header("Unit data")]
    public Unit unit;

    [Header("Prefab references")]
    public StackStats stackStats;

    public void Initialize(Unit unit, Player owner, int spawnId, bool defenderSide)
    {
        Initialize(owner, unit.combatPieceStats, spawnId, defenderSide);

        StackStats prefabSS = AllPrefabs.Instance.stackStats;

        this.unit = unit;
        name = "P" + owner.id + " - Stack of " + unit.GetName();
        profilePicture = unit.dbData.profilePicture;

        stackStats = Instantiate(prefabSS, transform);
        stackStats.Initialize(unit.stackStats);

        ACtP_ResetMovementPoints();
        SetAnimatorOverrideController(unit.dbData.animatorCombat);
    }

    public override bool ACP_TakeDamage(int amount)
    {
        bool result = combatPieceStats.TakeDamage(amount, stackStats, out int stackLost);
        string log = unit.GetName() + " took " + amount + " damage. " + stackLost + " units died.";
        CombatManager.Instance.AddEntryToLog(log);

        if (result)
        {
            ACP_Die();
            return true;
        }
        else
        {
            isHurt = true;
            return false;
        }
    }

    public override void ACP_Die()
    {
        stackStats.stack_current = 0;
        base.ACP_Die();
    }

    public override void ACtP_ResetMovementPoints()
    {
        if (!pieceMovement) pieceMovement = GetComponent<PieceMovement>();
        pieceMovement.movementPointsMax = unit.combatPieceStats.movementRange * 100;
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
                int dmg = ACtP_CalculateDamage();
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
        AbstractCombatantPiece2 targetUnit = pieceMovement.targetPiece as AbstractCombatantPiece2;
        CombatPieceHandler cph = CombatManager.Instance.pieceHandler;
        CombatantHeroPiece2 attackerHero = cph.GetHero(owner);
        CombatantHeroPiece2 defenderHero = cph.GetHero(targetUnit.IPO_GetOwner());
        return CombatLogic.DamageCalculation(this, targetUnit, attackerHero, defenderHero);
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
