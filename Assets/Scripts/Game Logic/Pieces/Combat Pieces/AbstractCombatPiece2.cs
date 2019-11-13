using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceCombatActions2))]
public abstract class AbstractCombatPiece2 : AbstractPiece2, IStartTurnEndTurn, ICommandablePiece
{
    public PieceCombatActions2 pieceCombatActions { get; private set; }

    [Header("Combat identification")]
    public int spawnId;
    public bool onDefenderSide;

    [Header("Hit point management")]
    public int hitPointsCurrent;
    public int hitPointsMax;

    [Header("States")]
    public bool stateHurt;
    public bool stateDead;

    [Header("Stats")]
    public CombatPieceStats combatPieceStats;

    protected override void Awake()
    {
        base.Awake();

        canBeOwned = true;
        canBeControlled = true;

        pieceCombatActions = GetComponent<PieceCombatActions2>();
    }

    public override void AP2_UpdateAnimatorParameters()
    {
        animator.SetBool("Hurt", stateHurt);
        animator.SetBool("Dead", stateDead);

        animator.SetBool("Melee attack start", pieceCombatActions.meleeAttackStart);
        animator.SetBool("Melee attack end", pieceCombatActions.meleeAttackEnd);
    }

    /*
    *   BEGIN:      Take damage, become either hurt or dead
    */
    public virtual IEnumerator TakeDamage(int amount)
    {
        hitPointsCurrent -= amount;
        if (hitPointsCurrent > 0) yield return StartCoroutine(DamagedHurt());
        else yield return StartCoroutine(DamagedDead());
    }
    protected virtual IEnumerator DamagedHurt()
    {
        stateHurt = true;
        AnimatorStateInfo state = GetAnimatorStateInfo();
        while (state.IsName("Hurt")) yield return null;
        stateHurt = false;
    }
    protected virtual IEnumerator DamagedDead()
    {
        stateDead = true;
        hitPointsCurrent = 0;
        mainSpriteRenderer.sortingOrder--;

        currentTile.occupantPiece = null;
        (currentTile as CombatTile).deadPieces.Add(this);

        CombatManager.Instance.RemoveUnitFromTurnSequence(this);

        AnimatorStateInfo state = GetAnimatorStateInfo();
        while (state.IsName("Dead")) yield return null;
    }
    /*
    *   END:        Take damage, become either hurt or dead
    */

    public virtual void ISTET_StartTurn()
    {
        pieceCombatActions.stateWait = false;
        pieceCombatActions.stateDefend = false;
    }

    public virtual void ISTET_EndTurn()
    {
        CombatManager cm = CombatManager.Instance;
        if (cm.currentPiece == this) cm.NextUnit();
        else if (cm.retaliatorPiece == this) cm.NextUnit();
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

    public virtual void ICP_InteractWith(AbstractTile tile, bool canPathfind)
    {
        targetTile = tile;
        targetPiece = tile.occupantPiece;

        if (targetPiece) ICP_InteractWithTargetPiece(canPathfind);
        else ICP_InteractWithTargetTile(canPathfind);
    }

    public virtual void ICP_InteractWithTargetTile(bool canPathfind)
    {
        throw new System.NotImplementedException();
    }

    public virtual void ICP_InteractWithTargetPiece(bool canPathfind)
    {
        if (GetOwner() != targetPiece.GetOwner())
        {
            pieceCombatActions.Attack(combatPieceStats.attack_primary);
        }
        else
        {
            //TODO ALLY INTERACTIONS
        }
    }
}
