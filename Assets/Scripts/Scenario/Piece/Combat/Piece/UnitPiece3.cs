using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceMovement3))]
public class UnitPiece3 : CombatantPiece3, IMovablePiece
{
    [Header("Object components")]
    public PieceMovement3 pieceMovement;

    [Header("Unit references")]
    public AbstractUnit abstractUnit;

    public override void Initialize(Player owner, int spawnId, bool onDefenderSide, AbstractUnit abstractUnit)
    {
        this.abstractUnit = abstractUnit;
        base.Initialize(owner, spawnId, onDefenderSide, abstractUnit);

        //GetStackHealthStats().Initialize(unit.GetStackHealthStats().GetStackSize());  todo this for CombatUnits ?
        SetAnimatorOverrideController(abstractUnit.GetDBUnit().animatorCombat);

        IMP_ResetMovementPoints();
    }

    protected override void AP3_UpdateAnimatorParameters()
    {
        name = abstractUnit.AU_GetUnitName();      //TODO update this at every turn?

        base.AP3_UpdateAnimatorParameters();

        animator.SetBool("Movement start", pieceMovement.stateMovementStart);
        animator.SetBool("Movement going", pieceMovement.stateMovementGoing);
        animator.SetBool("Movement end", pieceMovement.stateMovementEnd);

        //anim_directionX = 0;
        //if (direction.x < 0) anim_directionX = -1;
        //if (direction.x > 0) anim_directionX = 1;
        //animator.SetFloat("Direction X", anim_directionX);
    }

    public override void ISTET_StartTurn()
    {
        base.ISTET_StartTurn();

        IMP_ResetMovementPoints();
    }

    public override bool ICP_IsIdle()
    {
        return base.ICP_IsIdle()
            && pieceMovement.IsIdle();
    }

    public override void ICP_Stop()
    {
        //StartCoroutine(pieceMovement.Stop());
        pieceMovement.Stop();
    }

    public override void ICP_InteractWith(AbstractTile tile)
    {
        if (!tile) return;
        targetTile = tile;
        targetPiece = tile?.occupantPiece;

        if (tile.occupantPiece) StartCoroutine(ICP_InteractWithTargetPiece(tile.occupantPiece));
        else StartCoroutine(ICP_InteractWithTargetTile(tile, true));
    }

    public override IEnumerator ICP_InteractWithTargetTile(AbstractTile targetTile, bool endTurnWhenDone)
    {
        bool hasPath = pieceMovement.HasPath(targetTile);
        yield return StartCoroutine(pieceMovement.Movement(targetTile));
        if (hasPath && endTurnWhenDone) ISTET_EndTurn();
    }

    public virtual void IMP_ResetMovementPoints()
    {
        int movementPointsMax = movementStats.movementRange * 100;
        pieceMovement.ResetMovementPoints(movementPointsMax);
    }
}
