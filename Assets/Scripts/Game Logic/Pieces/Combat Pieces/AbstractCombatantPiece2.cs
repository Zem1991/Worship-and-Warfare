using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceMovement2))]
public abstract class AbstractCombatantPiece2 : AbstractCombatPiece2, IMovablePiece
{
    public PieceMovement2 pieceMovement { get; private set; }

    [Header("Combatant actions")]
    public AbstractCombatPiece2 retaliationTarget;
    public Projectile projectile;

    protected override void ManualAwake()
    {
        base.ManualAwake();

        pieceMovement = GetComponent<PieceMovement2>();
    }

    public void Initialize(Player owner, CombatPieceStats cps, int spawnId, bool onDefenderSide)
    {
        ManualAwake();

        CombatPieceStats prefabCPS = AllPrefabs.Instance.combatPieceStats;

        canBeOwned = true;
        canBeControlled = true;

        this.owner = owner;
        this.spawnId = spawnId;
        this.onDefenderSide = onDefenderSide;

        combatPieceStats = Instantiate(prefabCPS, transform);
        combatPieceStats.Initialize(cps);

        FlipSpriteHorizontally(onDefenderSide);
        SetFlagSprite(owner.dbColor.imgFlag);

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

    //public void ICP_InteractWithTile(AbstractTile aTile, bool canPathfind)
    //{
    //    if (canPathfind)
    //    {
    //        CombatTile cTile = aTile as CombatTile;
    //        if (cTile)
    //        {
    //            AbstractPiece2 targetPiece = cTile.occupantPiece;
    //            if (targetPiece)
    //            {
    //                ICP_InteractWithPiece(targetPiece, canPathfind);
    //            }
    //            else
    //            {
    //                if (pieceMovement.HasPath(cTile)) IMP_Move();
    //                else CombatManager.Instance.pieceHandler.Pathfind(this, cTile);
    //                //if (!HasPath(cTile)) CombatManager.Instance.pieceHandler.Pathfind(this, cTile);
    //                //Move();
    //            }
    //        }
    //    }
    //    else
    //    {
    //        if (pieceMovement.HasPath()) IMP_Move();
    //    }
    //}

    //public void ICP_InteractWithPiece(AbstractPiece2 aPiece, bool canPathfind)
    //{
    //    bool justInteract = false;

    //    AbstractCombatantPiece2 targetACP = aPiece as AbstractCombatantPiece2;
    //    if (targetACP &&
    //        !targetACP.isDead &&
    //        owner != targetACP.owner &&
    //        combatPieceStats.attack_primary.isRanged)
    //    {
    //        //TODO add check if we are not in melee range
    //        //TODO maybe add cases for ally ranged interactions ?
    //        justInteract = true;
    //    }

    //    if (justInteract)
    //    {
    //        pieceMovement.targetPiece = targetACP;
    //        AP2_PieceInteraction();
    //        return;
    //    }

    //    CombatTile cTile = aPiece.currentTile as CombatTile;
    //    if (canPathfind)
    //    {
    //        if (pieceMovement.HasPath(cTile)) IMP_Move();
    //        else CombatManager.Instance.pieceHandler.Pathfind(this, cTile);
    //    }
    //    else
    //    {
    //        CombatManager.Instance.pieceHandler.Pathfind(this, cTile);
    //        if (pieceMovement.HasPath(cTile)) IMP_Move();
    //    }
    //}

    public override bool ICP_IsIdle()
    {
        return base.ICP_IsIdle()
            && pieceMovement.IsIdle();
    }

    public override void ICP_Stop()
    {
        StartCoroutine(pieceMovement.Stop());
    }

    public override void ICP_InteractWithTargetTile(AbstractTile targetTile, bool canPathfind)
    {
        if (canPathfind)
        {
            if (pieceMovement.HasPath(targetTile)) StartCoroutine(pieceMovement.Movement());
            else CombatManager.Instance.pieceHandler.Pathfind(this, targetTile as CombatTile);
        }
        else
        {
            CombatManager.Instance.pieceHandler.Pathfind(this, targetTile as CombatTile);
            if (pieceMovement.HasPath(targetTile)) StartCoroutine(pieceMovement.Movement());
        }
    }

    //public override void ICP_InteractWithTargetPiece(bool canPathfind)
    //{
    //    //targetTile = aTile;
    //    //targetPiece = aTile.occupantPiece;
    //    //if (targetPiece) AP2_PieceInteraction();
    //    //else pieceMovement.Movement();
    //    throw new System.NotImplementedException();
    //}

    public virtual void IMP_ResetMovementPoints()
    {
        if (!pieceMovement) pieceMovement = GetComponent<PieceMovement2>();
        int movementPointsMax = combatPieceStats.movementRange * 100;
        pieceMovement.ResetMovementPoints(movementPointsMax);
    }
}
