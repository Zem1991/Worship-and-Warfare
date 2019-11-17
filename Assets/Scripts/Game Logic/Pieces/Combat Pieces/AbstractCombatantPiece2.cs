using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceMovement2))]
public abstract class AbstractCombatantPiece2 : AbstractCombatPiece2, IMovablePiece
{
    public PieceMovement2 pieceMovement { get; private set; }

    protected override void ManualAwake()
    {
        base.ManualAwake();

        pieceMovement = GetComponent<PieceMovement2>();
    }

    public override void Initialize(Player owner, CombatPieceStats cps, int spawnId, bool onDefenderSide)
    {
        base.Initialize(owner, cps, spawnId, onDefenderSide);

        IMP_ResetMovementPoints();
    }

    protected override void AP2_UpdateAnimatorParameters()
    {
        base.AP2_UpdateAnimatorParameters();

        animator.SetBool("Movement start", pieceMovement.movementStart);
        animator.SetBool("Movement going", pieceMovement.movementGoing);
        animator.SetBool("Movement end", pieceMovement.movementEnd);

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
        StartCoroutine(pieceMovement.Stop());
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
        if (!pieceMovement) pieceMovement = GetComponent<PieceMovement2>();
        int movementPointsMax = combatPieceStats.movementRange * 100;
        pieceMovement.ResetMovementPoints(movementPointsMax);
    }
}
