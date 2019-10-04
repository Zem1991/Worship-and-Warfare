using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCombatPiece : AbstractPiece
{
    [Header("Combat identification")]
    public int spawnId;
    public bool defenderSide;

    [Header("Combat settings")]
    public bool hasRangedAttack;

    [Header("Combat actions")]
    public AbstractCombatPiece retaliationTarget;
    public bool isAttacking_Start;
    public bool isAttacking_End;
    public bool isHurt;
    public bool isDead;

    [Header("Animator parameters")]
    private bool anim_movement;
    private float anim_directionX;
    private bool anim_attacking_start;
    private bool anim_attacking_end;
    private bool anim_hurt;
    private bool anim_dead;

    public override void Update()
    {
        base.Update();
        MakeAttack();
        MakeHurt();
    }

    public override bool IsIdle()
    {
        return base.IsIdle()
            && !isAttacking_Start
            && !isAttacking_End
            && !isHurt
            && !isDead;
    }

    protected override void MakeMove()
    {
        if (CombatManager.Instance.IsCombatRunning())
        {
            base.MakeMove();
        }
    }

    protected override void AnimatorParameters()
    {
        anim_movement = inMovement;
        animator.SetBool("Movement", anim_movement);

        //anim_directionX = 0;
        //if (direction.x < 0) anim_directionX = -1;
        //if (direction.x > 0) anim_directionX = 1;
        //animator.SetFloat("Direction X", anim_directionX);

        anim_attacking_start = isAttacking_Start;
        animator.SetBool("Attack Start", anim_attacking_start);

        anim_attacking_end = isAttacking_End;
        animator.SetBool("Attack End", anim_attacking_end);

        anim_hurt = isHurt;
        animator.SetBool("Hurt", anim_hurt);

        anim_dead = isDead;
        animator.SetBool("Dead", anim_dead);
    }

    public override void InteractWithTile(AbstractTile target, bool canPathfind)
    {
        if (canPathfind)
        {
            CombatTile cTile = target as CombatTile;
            if (cTile)
            {
                AbstractPiece targetPiece = cTile.occupantPiece;
                if (targetPiece)
                {
                    InteractWithPiece(targetPiece, canPathfind);
                }
                else
                {
                    if (!HasPath(cTile)) CombatManager.Instance.pieceHandler.Pathfind(this, cTile);
                    Move();
                }
            }
        }
        else
        {
            if (HasPath()) Move();
        }
    }

    public override void InteractWithPiece(AbstractPiece target, bool canPathfind)
    {
        bool interactWithPieceInstead = false;

        AbstractCombatPiece targetACP = target as AbstractCombatPiece;
        if (targetACP &&
            !targetACP.isDead &&
            owner != targetACP.owner &&
            hasRangedAttack)
        {
            //TODO add check if we are not in melee range
            //TODO maybe add cases for ally ranged interactions ?
            interactWithPieceInstead = true;
        }

        CombatTile cTile = target.currentTile as CombatTile;
        if (HasPath(cTile))
        {
            if (interactWithPieceInstead) PerformPieceInteraction();
            else Move();
        }
        else
        {
            CombatManager.Instance.pieceHandler.Pathfind(this, cTile);
        }
    }

    public abstract void MakeAttack();
    public abstract void MakeHurt();
    public abstract int CalculateDamage();
    public abstract void Attack(bool ranged);
    public abstract bool TakeDamage(float amount);
    public abstract void Retaliate();
    public abstract void Die();
}
