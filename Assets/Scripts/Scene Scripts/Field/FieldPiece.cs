using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldPiece : AbstractPiece
{
    [Header("Party contents")]
    public Hero hero;
    public List<Unit> units;

    [Header("Animator parameters")]
    public bool anim_movement;
    public float anim_directionX;
    public float anim_directionZ = -1;

    public void Initialize(Player owner, Hero hero, List<Unit> units)
    {
        this.owner = owner;
        this.hero = hero;
        this.units = units;

        if (hero != null)
        {
            SetAnimatorOverrideController(hero.dbData.classs.animatorField);
            name = "P" + owner.id + " - " + hero.dbData.heroName + ", " + hero.dbData.classs.className;
            //name = hero.dbData.heroName + "´s army";
        }
        else
        {
            Unit relevantUnit = units[0];
            SetAnimatorOverrideController(relevantUnit.dbData.animatorField);
            name = "P" + owner.id + " - Stack of " + relevantUnit.GetName();
            //name = "Army of " + relevantUnit.dbData.namePlural;
        }

        CalculateMovementPoints();
    }

    protected override void AnimatorParameters()
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

    public override void CalculateMovementPoints()
    {
        //TODO ACTUAL CALCULATIONS
        movementPoints = 1000;
    }

    public override void StartTurn()
    {
        CalculateMovementPoints();
    }

    public override void EndTurn()
    {
        throw new System.NotImplementedException();
    }

    public override void InteractWithTile(AbstractTile target, bool canPathfind)
    {
        if (canPathfind)
        {
            if (target)
            {
                if (HasPath(target)) Move();
                else FieldManager.Instance.pieceHandler.Pathfind(this, target as FieldTile);
            }
        }
        else
        {
            if (HasPath()) Move();
        }
    }

    public override void InteractWithPiece(AbstractPiece target, bool canPathfind)
    {
        InteractWithTile(target.targetTile, canPathfind);
    }

    public override void PerformPieceInteraction()
    {
        FieldManager.Instance.pieceHandler.PartiesAreInteracting(this, targetPiece as FieldPiece);
    }
}
