﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceMovement))]
public abstract class AbstractCombatantPiece2 : AbstractCombatPiece2, ICommandablePiece, IMovablePiece
{
    protected PieceMovement pieceMovement;

    [Header("Combat identification")]
    public int spawnId;
    public bool defenderSide;

    [Header("Combat settings")]
    public bool hasRangedAttack;

    [Header("Combatant actions")]
    public AbstractCombatPiece2 retaliationTarget;
    public bool isAttacking_Start;
    public bool isAttacking_End;
    public bool isHurt;

    [Header("Animator parameters")]
    private bool anim_movement;
    private float anim_directionX;
    private bool anim_attacking_start;
    private bool anim_attacking_end;
    private bool anim_hurt;
    private bool anim_dead;

    protected override void Awake()
    {
        base.Awake();
        pieceMovement = GetComponent<PieceMovement>();
    }

    protected override void Update()
    {
        base.Update();
        ACtP_MakeAttack();
        ACtP_MakeHurt();
    }

    public override void AP2_AnimatorParameters()
    {
        anim_movement = pieceMovement.inMovement;
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

    public override void AP2_PieceInteraction()
    {
        if (pieceMovement.targetPiece)
        {
            Player ownerFound;
            if (pieceMovement.targetPiece.FindOwner(out ownerFound) && ownerFound != owner)
            {
                //TODO check ranged interaction
                ACtP_Attack(hasRangedAttack);
            }
        }
    }

    public void ICP_StartTurn()
    {
        IMP_ResetMovementPoints();
    }

    public void ICP_EndTurn()
    {
        CombatManager cm = CombatManager.Instance;
        if (cm.currentPiece == this) cm.NextUnit();
        else if (cm.retaliatorPiece == this) cm.NextUnit();
    }

    public void ICP_InteractWithTile(AbstractTile aTile, bool canPathfind)
    {
        if (canPathfind)
        {
            CombatTile cTile = aTile as CombatTile;
            if (cTile)
            {
                AbstractPiece2 targetPiece = cTile.occupantPiece;
                if (targetPiece)
                {
                    ICP_InteractWithPiece(targetPiece, canPathfind);
                }
                else
                {
                    if (pieceMovement.HasPath(cTile)) IMP_Move();
                    else CombatManager.Instance.pieceHandler.Pathfind(this, cTile);
                    //if (!HasPath(cTile)) CombatManager.Instance.pieceHandler.Pathfind(this, cTile);
                    //Move();
                }
            }
        }
        else
        {
            if (pieceMovement.HasPath()) IMP_Move();
        }
    }

    public void ICP_InteractWithPiece(AbstractPiece2 aPiece, bool canPathfind)
    {
        bool justInteract = false;

        AbstractCombatantPiece2 targetACP = aPiece as AbstractCombatantPiece2;
        if (targetACP &&
            !targetACP.isDead &&
            owner != targetACP.owner &&
            hasRangedAttack)
        {
            //TODO add check if we are not in melee range
            //TODO maybe add cases for ally ranged interactions ?
            justInteract = true;
        }

        if (justInteract)
        {
            pieceMovement.targetPiece = targetACP;
            AP2_PieceInteraction();
            return;
        }

        CombatTile cTile = aPiece.currentTile as CombatTile;
        if (canPathfind)
        {
            if (pieceMovement.HasPath(cTile)) IMP_Move();
            else CombatManager.Instance.pieceHandler.Pathfind(this, cTile);
        }
        else
        {
            CombatManager.Instance.pieceHandler.Pathfind(this, cTile);
            if (pieceMovement.HasPath(cTile)) IMP_Move();
        }
    }

    public bool ICP_IsIdle()
    {
        return !pieceMovement.inMovement
            && !isAttacking_Start
            && !isAttacking_End
            && !isHurt
            && !isDead;
    }

    public void IMP_ResetMovementPoints()
    {
        ACtP_ResetMovementPoints();
    }

    public void IMP_Move()
    {
        pieceMovement.inMovement = true;
    }

    public void IMP_Stop()
    {
        pieceMovement.stopWasCalled = true;
    }

    public abstract void ACtP_ResetMovementPoints();
    public abstract void ACtP_MakeAttack();
    public abstract void ACtP_MakeHurt();
    public abstract int ACtP_CalculateDamage();
    public abstract void ACtP_Attack(bool ranged);
    public abstract void ACtP_Retaliate();
}
