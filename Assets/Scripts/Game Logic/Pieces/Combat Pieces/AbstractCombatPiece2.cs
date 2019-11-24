using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceOwner))]
[RequireComponent(typeof(PieceController))]
[RequireComponent(typeof(PieceCombatActions2))]
public abstract class AbstractCombatPiece2 : AbstractPiece2, IStartTurnEndTurn, ICommandablePiece
{
    [Header("Other references")]
    public PieceOwner pieceOwner;
    public PieceController pieceController;
    public PieceCombatActions2 pieceCombatActions;

    [Header("Combat identification")]
    public int spawnId;
    public bool onDefenderSide;

    [Header("States")]
    public bool stateHurt;
    public bool stateDead;

    [Header("Stats")]
    public CombatPieceStats combatPieceStats;

    [Header("Animation settings")]
    public bool animateMovementStart;
    public bool animateMovementEnd;

    protected override void ManualAwake()
    {
        base.ManualAwake();

        pieceOwner = GetComponent<PieceOwner>();
        pieceController = GetComponent<PieceController>();
        pieceCombatActions = GetComponent<PieceCombatActions2>();
    }

    protected override void Update()
    {
        base.Update();
        if (!currentTile) Debug.LogError("NULEI!");
    }

    public virtual void Initialize(Player owner, CombatPieceStats cps, int spawnId, bool onDefenderSide)
    {
        ManualAwake();

        CombatPieceStats prefabCPS = AllPrefabs.Instance.combatPieceStats;

        pieceOwner.SetOwner(owner);
        pieceController.SetController(owner);
        this.spawnId = spawnId;
        this.onDefenderSide = onDefenderSide;

        combatPieceStats = Instantiate(prefabCPS, transform);
        combatPieceStats.Initialize(cps);

        pieceCombatActions.canWait = combatPieceStats.canWait;
        pieceCombatActions.canDefend = combatPieceStats.canDefend;
        pieceCombatActions.canRetaliate = combatPieceStats.canRetaliate;
        pieceCombatActions.canCounter = combatPieceStats.canCounter;
        pieceCombatActions.retaliations = combatPieceStats.retaliationsMax;

        FlipSpriteHorizontally(onDefenderSide);
        SetFlagSprite(owner.dbColor.imgFlag);
    }

    protected override void AP2_UpdateAnimatorParameters()
    {
        animator.SetBool("Hurt", stateHurt);
        animator.SetBool("Dead", stateDead);

        animator.SetBool("Melee attack start", pieceCombatActions.meleeAttackStart);
        animator.SetBool("Melee attack end", pieceCombatActions.meleeAttackEnd);

        animator.SetBool("Ranged attack start", pieceCombatActions.rangedAttackStart);
        animator.SetBool("Ranged attack end", pieceCombatActions.rangedAttackEnd);
    }

    /*
    *   BEGIN:      Take damage, become either hurt or dead
    */
    public virtual IEnumerator TakeDamage(int amount)
    {
        bool defeated = combatPieceStats.TakeDamage(amount);
        if (defeated) yield return StartCoroutine(DamagedDead());
        else yield return StartCoroutine(DamagedHurt());
    }
    protected virtual IEnumerator DamagedHurt()
    {
        stateHurt = true;
        string stateName = "Hurt";
        yield return StartCoroutine(WaitForAnimationStartAndEnd(stateName));
        stateHurt = false;
    }
    protected virtual IEnumerator DamagedDead()
    {
        combatPieceStats.hitPoints_current = 0;

        stateDead = true;
        string stateName = "Dead";
        yield return StartCoroutine(WaitForAnimationStartAndEnd(stateName));

        mainSpriteRenderer.sortingOrder--;
        currentTile.occupantPiece = null;
        (currentTile as CombatTile).deadPieces.Add(this);
        CombatManager.Instance.RemoveUnitFromTurnSequence(this);
    }
    /*
    *   END:        Take damage, become either hurt or dead
    */

    public virtual void ISTET_StartTurn()
    {
        pieceCombatActions.stateWait = false;
        pieceCombatActions.stateDefend = false;

        pieceCombatActions.retaliations = combatPieceStats.retaliationsMax;
    }

    public virtual void ISTET_EndTurn()
    {
        CombatManager cm = CombatManager.Instance;
        if (cm.currentPiece == this) cm.NextUnit();
    }

    public virtual bool ICP_IsIdle()
    {
        return !stateHurt
            && !stateDead
            && pieceCombatActions.IsIdle();
    }

    public virtual void ICP_Stop()
    {
        //TODO become called from overridden methods
        throw new System.NotImplementedException();
    }

    public virtual void ICP_InteractWith(AbstractTile tile)
    {
        if (!tile) return;
        targetTile = tile;
        targetPiece = tile?.occupantPiece;

        if (tile.occupantPiece) StartCoroutine(ICP_InteractWithTargetPiece(tile.occupantPiece));
        else StartCoroutine(ICP_InteractWithTargetTile(tile, false));
        //if (tile.occupantPiece) ICP_InteractWithTargetPiece(tile.occupantPiece);
        //else ICP_InteractWithTargetTile(tile, false);
    }

    public virtual IEnumerator ICP_InteractWithTargetTile(AbstractTile targetTile, bool endTurnWhenDone)
    {
        //TODO I don't think there would be ranged tile interactions, but let's leave this here in case I change my mind.
        throw new System.NotImplementedException();
    }

    public virtual IEnumerator ICP_InteractWithTargetPiece(AbstractPiece2 targetPiece)
    {
        AbstractCombatPiece2 targetCombatPiece = targetPiece as AbstractCombatPiece2;
        if (pieceOwner.GetOwner() != targetCombatPiece.pieceOwner.GetOwner())
        {
            yield return
                StartCoroutine(pieceCombatActions.Attack(targetCombatPiece));
        }
        else
        {
            //TODO ALLY INTERACTIONS
            throw new System.NotImplementedException();
        }
    }
}
