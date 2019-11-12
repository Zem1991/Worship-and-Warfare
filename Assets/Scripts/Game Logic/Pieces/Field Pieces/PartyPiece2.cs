﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceMovement2))]
public class PartyPiece2 : AbstractFieldPiece2, IStartTurnEndTurn, ICommandablePiece, IMovablePiece
{
    public PieceMovement2 pieceMovement;

    [Header("Party contets")]
    public Hero partyHero;
    public List<Unit> partyUnits;

    [Header("Animator parameters")]
    public bool anim_movement;
    public float anim_directionX;
    public float anim_directionZ = -1;

    protected override void Awake()
    {
        base.Awake();

        canBeOwned = true;
        canBeControlled = true;

        pieceMovement = GetComponent<PieceMovement2>();
    }

    public void Initialize(Player owner, Hero hero, List<Unit> units)
    {
        canBeOwned = true;
        canBeControlled = true;

        base.owner = owner;
        partyHero = hero;
        partyUnits = units;

        name = "P" + owner.id + " - Party";
        if (hero != null)
        {
            SetAnimatorOverrideController(hero.dbData.classs.animatorField);
        }
        else
        {
            Unit relevantUnit = units[0];
            SetAnimatorOverrideController(relevantUnit.dbData.animatorField);
        }

        SetFlagSprite(owner.dbColor.imgFlag);

        IMP_ResetMovementPoints();
    }

    public void ApplyExperience(int experience)
    {
        if (partyHero)
        {
            partyHero.RecalculateExperience(experience);
        }
    }

    public override void AP2_UpdateAnimatorParameters()
    {
        anim_movement = pieceMovement.stateMove;
        animator.SetBool("Movement", anim_movement);

        anim_directionX = 0;
        if (pieceMovement.direction.x < 0) anim_directionX = -1;
        if (pieceMovement.direction.x > 0) anim_directionX = 1;
        animator.SetFloat("Direction X", anim_directionX);

        anim_directionZ = 0;
        if (pieceMovement.direction.z < 0) anim_directionZ = -1;
        if (pieceMovement.direction.z > 0) anim_directionZ = 1;
        animator.SetFloat("Direction Z", anim_directionZ);
    }

    //public override void AP2_TileInteraction()
    //{
    //    throw new NotImplementedException();
    //}

    //public override void AP2_PieceInteraction()
    //{
    //    PartyPiece2 targetParty = targetPiece as PartyPiece2;
    //    PickupPiece2 targetPickup = targetPiece as PickupPiece2;

    //    if (targetParty) FieldManager.Instance.PartiesAreInteracting(this, targetParty);
    //    else if (targetPickup) FieldManager.Instance.PartyFoundPickup(this, targetPickup);
    //}

    public void ISTET_StartTurn()
    {
        IMP_ResetMovementPoints();
    }

    public void ISTET_EndTurn()
    {
        throw new System.NotImplementedException();
    }

    public bool ICP_IsIdle()
    {
        return !pieceMovement.stateMove;
    }

    public void ICP_Stop()
    {
        pieceMovement.Stop();
    }

    //public void ICP_InteractWith(AbstractTile aTile, bool canPathfind)
    //{
    //    if (canPathfind)
    //    {
    //        if (aTile)
    //        {
    //            if (pieceMovement.HasPath(aTile)) IMP_Move();
    //            else FieldManager.Instance.pieceHandler.Pathfind(this, aTile as FieldTile);
    //        }
    //    }
    //    else
    //    {
    //        if (pieceMovement.HasPath()) IMP_Move();
    //    }
    //}

    public void IMP_ResetMovementPoints()
    {
        //TODO ACTUAL CALCULATIONS
        int movementPointsMax = 1000;
        pieceMovement.ResetMovementPoints(movementPointsMax);
    }
}
