using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCombatPiece : AbstractPiece
{
    [Header("Sprites")]
    public Sprite imgProfile;

    [Header("Animator Variables")]
    public bool anim_movement;
    public float anim_directionX;
    public float anim_directionZ = -1;

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

        //anim_directionZ = 0;
        //if (direction.z < 0) anim_directionZ = -1;
        //if (direction.z > 0) anim_directionZ = 1;
        //animator.SetFloat("Direction Z", anim_directionZ);
    }
}
