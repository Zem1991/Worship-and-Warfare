using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceOwner))]
[RequireComponent(typeof(PieceController))]
[RequireComponent(typeof(PieceMovement2))]
public class PartyPiece2 : AbstractFieldPiece2, IStartTurnEndTurn, ICommandablePiece, IMovablePiece
{
    [Header("Other references")]
    public PieceOwner pieceOwner;
    public PieceController pieceController;
    public PieceMovement2 pieceMovement;

    [Header("Party contents")]
    public Party party;

    [Header("Animator parameters")]
    public bool anim_movement;
    public float anim_directionX;
    public float anim_directionZ = -1;

    protected override void ManualAwake()
    {
        base.ManualAwake();

        pieceOwner = GetComponent<PieceOwner>();
        pieceController = GetComponent<PieceController>();
        pieceMovement = GetComponent<PieceMovement2>();
    }

    public void Initialize(Player owner, Party party)
    {
        ManualAwake();

        pieceOwner.SetOwner(owner);
        pieceController.SetController(owner);

        this.party = party;
        name = "P" + owner.id + " - Party";

        Hero hero = party.hero.GetSlotObject() as Hero;
        Unit relevantUnit = party.GetRelevantUnit();

        if (hero)
        {
            SetAnimatorOverrideController(hero.dbData.heroClass.animatorField);
        }
        else
        {
            SetAnimatorOverrideController(relevantUnit.dbData.animatorField);
        }

        if (!mainSpriteRenderer) mainSpriteRenderer = GetComponentsInChildren<SpriteRenderer>()[0];
        mainSpriteRenderer.sortingOrder = SpriteOrderConstants.PIECE;

        SetFlagSprite(owner.dbColor.imgFlag);

        IMP_ResetMovementPoints();
    }

    public bool ApplyExperience()
    {
        return ApplyExperience(0);
    }

    public bool ApplyExperience(int amountToAdd)
    {
        if (party.hero.GetSlotObject())
        {
            Hero hero = party.hero.GetSlotObject() as Hero;
            hero.RecalculateExperience(amountToAdd);
            return hero.levelUps > 0;
        }
        return false;
    }

    protected override void AP2_UpdateAnimatorParameters()
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

    public override string AFP2_GetPieceTitle()
    {
        Hero hero = party.hero.GetSlotObject() as Hero;
        Unit unit = party.GetRelevantUnit();

        string result = "Unknown party piece title";
        if (hero) result = hero.dbData.heroName + "'s party";
        else if (unit) result = "Non-commissioned party";
        return result;
    }

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
        return pieceMovement.IsIdle();
    }

    public void ICP_Stop()
    {
        //StartCoroutine(pieceMovement.Stop());
        pieceMovement.Stop();
    }

    public void ICP_InteractWith(AbstractTile tile)
    {
        if (!tile) return;
        targetTile = tile;
        targetPiece = tile?.occupantPiece;

        if (targetPiece) StartCoroutine(ICP_InteractWithTargetPiece(tile.occupantPiece));
        else StartCoroutine(ICP_InteractWithTargetTile(tile, false));
    }

    public virtual IEnumerator ICP_InteractWithTargetTile(AbstractTile targetTile, bool endTurnWhenDone)
    {
        bool hasPath = pieceMovement.HasPath(targetTile);
        yield return StartCoroutine(pieceMovement.Movement(targetTile));
        if (hasPath && endTurnWhenDone) ISTET_EndTurn();
    }

    public virtual IEnumerator ICP_InteractWithTargetPiece(AbstractPiece2 targetPiece)
    {
        //TODO consider making an PieceFieldActions2 class to handle each interaction.
        bool neighbours = currentTile.IsNeighbour(targetPiece.currentTile);
        if (neighbours && pathTargetTile == targetPiece.currentTile)
        {
            TownPiece2 targetTown = targetPiece as TownPiece2;
            PartyPiece2 targetParty = targetPiece as PartyPiece2;
            AbstractPickupPiece2 targetPickup = targetPiece as AbstractPickupPiece2;

            if (targetTown)
            {
                //yield return StartCoroutine(FieldManager.Instance.PartiesAreInteracting(this, targetParty));
                FieldManager.Instance.PartyInteraction(this, targetTown);
            }
            else if (targetParty)
            {
                //yield return StartCoroutine(FieldManager.Instance.PartiesAreInteracting(this, targetParty));
                FieldManager.Instance.PartyInteraction(this, targetParty);
            }
            else if (targetPickup)
            {
                //yield return StartCoroutine(FieldManager.Instance.PartyFoundPickup(this, targetPickup));
                FieldManager.Instance.PartyInteraction(this, targetPickup);
            }
        }
        else
        {
            yield return StartCoroutine(ICP_InteractWithTargetTile(targetPiece.currentTile, false));
        }
    }

    public void IMP_ResetMovementPoints()
    {
        //TODO ACTUAL CALCULATIONS
        if (!pieceMovement) pieceMovement = GetComponent<PieceMovement2>();
        int movementPointsMax = 1500;
        pieceMovement.ResetMovementPoints(movementPointsMax);
    }
}
