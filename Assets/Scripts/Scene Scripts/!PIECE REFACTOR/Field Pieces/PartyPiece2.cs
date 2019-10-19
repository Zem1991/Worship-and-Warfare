using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceMovement))]
public class PartyPiece2 : AbstractFieldPiece2, ICommandablePiece, IMovablePiece
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
        pieceMovement = GetComponent<PieceMovement>();
    }

    public void Initialize(Player owner, Hero hero, List<Unit> units)
    {
        base.owner = owner;
        partyHero = hero;
        partyUnits = units;

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

        IMP_ResetMovementPoints();
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
        FieldManager.Instance.pieceHandler.PartiesAreInteracting(this, pieceMovement.targetPiece as PartyPiece2);
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
}
