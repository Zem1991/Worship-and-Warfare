using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldPiece : AbstractPiece
{
    [Header("Party Contents")]
    public Hero hero;
    public Unit[] units;

    [Header("Animator Variables")]
    public bool anim_movement;
    public float anim_directionX;
    public float anim_directionZ = -1;

    protected override void AnimatorVariables()
    {
        anim_movement = inMovement;
        animator.SetBool("Movement", anim_movement);

        anim_directionX = 0;
        if (direction.x < 0) anim_directionX = -1;
        if (direction.x > 0) anim_directionX = 1;
        animator.SetFloat("Direction X", anim_directionX);

        anim_directionZ = 0;
        if (direction.z < 0) anim_directionZ = -1;
        if (direction.z > 0) anim_directionZ = 1;
        animator.SetFloat("Direction Z", anim_directionZ);
    }

    public override void InteractWithTile(AbstractTile targetTile, bool canPathfind)
    {
        if (canPathfind)
        {
            if (targetTile)
            {
                if (HasPath(targetTile))
                {
                    Move();
                }
                else
                {
                    FieldManager.Instance.pieceHandler.Pathfind(this, targetTile as FieldTile);
                }
            }
        }
        else
        {
            if (HasPath()) Move();
        }
    }

    public override void InteractWithPiece(AbstractPiece target)
    {
        FieldManager.Instance.pieceHandler.PartiesAreInteracting(this, target as FieldPiece);
    }
}
