using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceMovement))]
public class PartyPiece2 : AbstractFieldPiece2, IPlayerOwnable, IPlayerControllable, ICommandablePiece, IMovablePiece
{
    protected PieceMovement pieceMovement;

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
        pieceMovement = GetComponent<PieceMovement>();
    }

    protected override void Update()
    {
        base.Update();
        IMP_MakeMove();
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

    public override void AP2_AnimatorParameters()
    {
        anim_movement = pieceMovement.inMovement;
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

    public override void AP2_PieceInteraction()
    {
        PartyPiece2 targetParty = pieceMovement.targetPiece as PartyPiece2;
        PickupPiece2 targetPickup = pieceMovement.targetPiece as PickupPiece2;

        if (targetParty) FieldManager.Instance.PartiesAreInteracting(this, targetParty);
        else if (targetPickup) FieldManager.Instance.PartyFoundPickup(this, targetPickup);
    }

    public bool IPO_HasOwner()
    {
        return canBeOwned && owner;
    }

    public Player IPO_GetOwner()
    {
        return owner;
    }

    public bool IPC_HasController()
    {
        return canBeControlled && controller;
    }

    public Player IPC_GetController()
    {
        return controller;
    }

    public void IPC_SetController(Player player)
    {
        controller = player;
    }

    public void ICP_StartTurn()
    {
        IMP_ResetMovementPoints();
    }

    public void ICP_EndTurn()
    {
        throw new System.NotImplementedException();
    }

    public void ICP_InteractWithTile(AbstractTile aTile, bool canPathfind)
    {
        if (canPathfind)
        {
            if (aTile)
            {
                if (pieceMovement.HasPath(aTile)) IMP_Move();
                else FieldManager.Instance.pieceHandler.Pathfind(this, aTile as FieldTile);
            }
        }
        else
        {
            if (pieceMovement.HasPath()) IMP_Move();
        }
    }

    public void ICP_InteractWithPiece(AbstractPiece2 aPiece, bool canPathfind)
    {
        ICP_InteractWithTile(aPiece.currentTile, canPathfind);
    }

    public bool ICP_IsIdle()
    {
        return !pieceMovement.inMovement;
    }

    public void IMP_ResetMovementPoints()
    {
        //TODO ACTUAL CALCULATIONS
        pieceMovement.movementPointsMax = 1000;
        pieceMovement.movementPointsCurrent = pieceMovement.movementPointsMax;
    }

    public void IMP_Move()
    {
        pieceMovement.inMovement = true;
    }

    public void IMP_Stop()
    {
        pieceMovement.stopWasCalled = true;
    }

    public void IMP_MakeMove()
    {
        pieceMovement.MakeMove();
    }

    public PieceMovement IMP_GetPieceMovement()
    {
        return pieceMovement;
    }
}
