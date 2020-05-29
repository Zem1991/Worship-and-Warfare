using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceOwner3))]
[RequireComponent(typeof(PieceController3))]
[RequireComponent(typeof(PieceMovement3))]
public class PartyPiece3 : AbstractFieldPiece3, IFlaggablePiece, IStartTurnEndTurn, ICommandablePiece, IMovablePiece, IPieceForCombat
{
    [Header("Object components")]
    public SpriteRenderer flagSpriteRenderer;

    [Header("Object components")]
    public PieceOwner3 pieceOwner;
    public PieceController3 pieceController;
    public PieceMovement3 pieceMovement;

    [Header("Object components")]
    [SerializeField] private Party party;

    [Header("Animator parameters")]
    public bool anim_movement;
    public float anim_directionX;
    public float anim_directionZ = -1;

    public void Initialize(Player owner)
    {
        pieceOwner.SetOwner(owner);
        pieceController.SetController(owner);
        name = "P" + owner.id + " Party";

        AbstractUnit ape = party.GetMostRelevant();
        if (ape)
        {
            UnitType partyElementType = ape.GetDBUnit().unitType;
            switch (partyElementType)
            {
                case UnitType.HERO:
                    SetAnimatorOverrideController((ape as HeroUnit).dbHeroPerson.heroClass.animatorField);
                    break;
                case UnitType.CREATURE:
                    SetAnimatorOverrideController((ape as CombatUnit).GetDBCombatUnit().animatorField);
                    break;
                case UnitType.SUPPORT:
                    //TODO support units
                    break;
            }
        }

        if (!mainSpriteRenderer) mainSpriteRenderer = GetComponentsInChildren<SpriteRenderer>()[0];
        mainSpriteRenderer.sortingOrder = SpriteOrderConstants.PIECE;

        SetMainSprite(mainSpriteRenderer.sprite, SpriteOrderConstants.PIECE);
        IFP_SetFlagSprite(owner.dbColor.imgFlag);

        IMP_ResetMovementPoints();
    }

    protected override void AP3_UpdateAnimatorParameters()
    {
        Vector3 dir = pieceMovement.GetDirection();

        anim_movement = !pieceMovement.IsIdle();
        animator.SetBool("Movement", anim_movement);

        anim_directionX = 0;
        if (dir.x < 0) anim_directionX = -1;
        if (dir.x > 0) anim_directionX = 1;
        animator.SetFloat("Direction X", anim_directionX);

        anim_directionZ = 0;
        if (dir.z < 0) anim_directionZ = -1;
        if (dir.z > 0) anim_directionZ = 1;
        animator.SetFloat("Direction Z", anim_directionZ);
    }

    public override string AFP3_GetPieceTitle()
    {
        AbstractUnit ape = party.GetMostRelevant();
        UnitType partyElementType = ape.GetDBUnit().unitType;

        string result = "Unknown party piece title";
        switch (partyElementType)
        {
            case UnitType.HERO:
                result = (ape as HeroUnit).dbHeroPerson.heroName + "'s party";
                break;
            case UnitType.CREATURE:
                result = "Non-commissioned party";
                break;
            case UnitType.SUPPORT:
                break;
        }
        return result;
    }

    public void IFP_SetFlagSprite(Sprite sprite)
    {
        flagSpriteRenderer.sprite = sprite;
        flagSpriteRenderer.sortingOrder = SpriteOrderConstants.FLAG;
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

    public virtual IEnumerator ICP_InteractWithTargetPiece(AbstractPiece3 targetPiece)
    {
        //TODO consider making an PieceFieldActions2 class to handle each interaction.
        bool neighbours = currentTile.IsNeighbour(targetPiece.currentTile);
        if (neighbours && pathArrivalTile == targetPiece.currentTile)
        {
            TownPiece3 targetTown = targetPiece as TownPiece3;
            PartyPiece3 targetParty = targetPiece as PartyPiece3;
            PickupPiece3 targetPickup = targetPiece as PickupPiece3;

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
        if (!pieceMovement) pieceMovement = GetComponent<PieceMovement3>();
        int movementPointsMax = 1500;
        pieceMovement.ResetMovementPoints(movementPointsMax);
    }

    public Player IPFC_GetPlayerForCombat()
    {
        return pieceOwner.GetOwner();
    }

    public Party IPFC_GetPartyForCombat()
    {
        return party;
    }

    public Town IPFC_GetPTownForCombat()
    {
        return null;
    }
}
