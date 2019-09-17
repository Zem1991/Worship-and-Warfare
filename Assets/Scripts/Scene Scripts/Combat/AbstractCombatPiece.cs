using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCombatPiece : AbstractPiece
{
    [Header("Party")]
    public FieldPiece party;

    [Header("Sprites")]
    public Sprite imgProfile;

    [Header("Combat actions")]
    public AbstractCombatPiece actionTarget;
    public bool isAttacking_Start;
    public bool isAttacking_End;
    public bool isHurt;
    public bool isDead;

    [Header("Animator variables")]
    private bool anim_movement;
    private float anim_directionX;
    private bool anim_attacking_start;
    private bool anim_attacking_end;
    private bool anim_hurt;
    private bool anim_dead;

    public override bool IsIdle()
    {
        return base.IsIdle()
            && !isAttacking_Start
            && !isAttacking_End
            && !isHurt
            && !isDead;
    }

    protected override void Movement()
    {
        if (CombatManager.Instance.IsCombatRunning())
        {
            base.Movement();
        }
    }

    protected override void AnimatorVariables()
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

    public abstract int CalculateDamage();
    public abstract bool TakeDamage(float amount);
    public abstract void Die();
}
