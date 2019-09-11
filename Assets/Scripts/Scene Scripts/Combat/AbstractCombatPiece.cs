using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCombatPiece : AbstractPiece
{
    [Header("Sprites")]
    public Sprite imgProfile;

    [Header("Combat actions")]
    public bool isAttacking;
    public bool isHurt;
    public bool isDead;

    [Header("Animator variables")]
    public bool anim_movement;
    public float anim_directionX;
    public bool anim_attacking;
    public bool anim_hurt;
    public bool anim_dead;

    protected override void AnimatorVariables()
    {
        anim_movement = inMovement;
        animator.SetBool("Movement", anim_movement);

        //anim_directionX = 0;
        //if (direction.x < 0) anim_directionX = -1;
        //if (direction.x > 0) anim_directionX = 1;
        //animator.SetFloat("Direction X", anim_directionX);

        anim_attacking = isAttacking;
        animator.SetBool("Attacking", anim_attacking);

        anim_hurt = isHurt;
        animator.SetBool("Hurt", anim_hurt);

        anim_dead = isDead;
        animator.SetBool("Dead", anim_dead);
    }

    protected override void Movement()
    {
        if (CombatManager.Instance.IsCombatRunning())
        {
            base.Movement();
        }
    }
}
