using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCombatPiece : AbstractCombatPiece
{
    //public bool didMove;
    //public bool didAttack;

    [Header("Unit combat stats")]
    public string nameSingular;
    public string namePlural;
    public int hitPointsMax;
    public int hitPointsCurrent;
    public int stackSizeStart;
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

    public void Initialize(Unit unit, bool defenderSide = false)
    {
        nameSingular = unit.nameSingular;
        namePlural = unit.namePlural;
        hitPointsMax = unit.hitPoints;
        hitPointsCurrent = hitPointsMax;
        stackSizeStart = unit.stackSize;
        stackSizeCurrent = stackSizeStart;

        damageMin = unit.damageMin;
        damageMax = unit.damageMax;
        hasRangedAttack = unit.hasRangedAttack;
        resistance = unit.resistance;
        speed = unit.speed;
        initiative = unit.initiative;

        imgProfile = unit.imgProfile;

        FlipSpriteHorizontally(defenderSide);
        SetAnimatorOverrideController(unit.animatorCombat);
    }

    public string GetName()
    {
        if (stackSizeCurrent == 1) return nameSingular;
        return namePlural;
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
        UnitCombatPiece targetUnit = target as UnitCombatPiece;
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
        UnitCombatPiece targetUnit = actionTarget as UnitCombatPiece;
        CombatPieceHandler cph = CombatManager.Instance.pieceHandler;
        return CombatLogic.DamageCalculation(this, targetUnit, cph.attackerHero, cph.defenderHero);
    }

    public override bool TakeDamage(float amount)
    {
        int amountFixed = Mathf.CeilToInt(amount);
        int stackLost = amountFixed / hitPointsMax;
        int hpLost = amountFixed % hitPointsMax;
        hitPointsCurrent -= hpLost;
        if (hitPointsCurrent <= 0)
        {
            stackLost++;
            stackSizeCurrent -= stackLost;
            hitPointsCurrent += hitPointsMax;
        }
        string log = GetName() + " took " + amountFixed + " damage. " + stackLost + " units died.";
        CombatManager.Instance.AddEntryToLog(log);

        if (stackSizeCurrent > 0)
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
        stackSizeCurrent = 0;
        hitPointsCurrent = 0;
        isDead = true;

        spriteRenderer.sortingOrder--;

        currentTile.occupantPiece = null;
        (currentTile as CombatTile).deadPieces.Add(this);
    }
}
