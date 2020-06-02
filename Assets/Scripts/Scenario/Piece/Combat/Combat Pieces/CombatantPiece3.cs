using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceOwner3))]
[RequireComponent(typeof(PieceController3))]
[RequireComponent(typeof(PieceCombatActions3))]
[RequireComponent(typeof(SettingsStats2))]
[RequireComponent(typeof(HealthStats2))]
[RequireComponent(typeof(MovementStats2))]
[RequireComponent(typeof(AttributeStats2))]
[RequireComponent(typeof(OffenseStats2))]
[RequireComponent(typeof(DefenseStats2))]
[RequireComponent(typeof(AbilityStats2))]
public class CombatantPiece3 : AbstractCombatPiece3, IFlaggablePiece, IStartTurnEndTurn, ICommandablePiece
{
    [Header("Object components")]
    public SpriteRenderer flagSpriteRenderer;

    [Header("Object components")]
    public PieceOwner3 pieceOwner;
    public PieceController3 pieceController;
    public PieceCombatActions3 pieceCombatActions;

    [Header("Object components")]
    public SettingsStats2 settingsStats;
    public HealthStats2 healthStats;
    public MovementStats2 movementStats;
    public AttributeStats2 attributeStats;
    public OffenseStats2 offenseStats;
    public DefenseStats2 defenseStats;
    public AbilityStats2 abilityStats;

    [Header("Combat identification")]
    public int spawnId;
    public bool onDefenderSide;

    [Header("Combat states")]       //TODO passar isso para combat actions?
    public bool stateHurt;
    public bool stateDead;

    //[Header("Stats")]             //TODO referencia p/ objeto a ser clonado e p/ o clone
    //public CombatPieceStats combatPieceStats;

    //public virtual void Initialize(Player owner, CombatPieceStats cps, int spawnId, bool onDefenderSide)      TODO passar o party element por parametro?

    public virtual void Initialize(Player owner, int spawnId, bool onDefenderSide)
    {
        pieceOwner.Set(owner);
        pieceController.Set(owner);

        this.spawnId = spawnId;
        this.onDefenderSide = onDefenderSide;

        pieceCombatActions.canWait = settingsStats.canWait;
        pieceCombatActions.canDefend = settingsStats.canDefend;
        pieceCombatActions.canRetaliate = settingsStats.canRetaliate;
        pieceCombatActions.canCounter = settingsStats.canCounter;
        pieceCombatActions.retaliations = settingsStats.retaliationsMax;

        FlipSpriteHorizontally(onDefenderSide);
        SetMainSprite(mainSpriteRenderer.sprite, SpriteOrderConstants.PIECE);
        IFP_SetFlagSprite(owner.dbColor.imgFlag);
    }

    protected override void AP3_UpdateAnimatorParameters()
    {
        animator.SetBool("Hurt", stateHurt);
        animator.SetBool("Dead", stateDead);

        animator.SetBool("Melee attack start", pieceCombatActions.meleeAttackStart);
        animator.SetBool("Melee attack end", pieceCombatActions.meleeAttackEnd);

        animator.SetBool("Ranged attack start", pieceCombatActions.rangedAttackStart);
        animator.SetBool("Ranged attack end", pieceCombatActions.rangedAttackEnd);

        animator.SetBool("Ability 1 start", pieceCombatActions.ability1Start);
        animator.SetBool("Ability 1 end", pieceCombatActions.ability1End);

        animator.SetBool("Ability 2 start", pieceCombatActions.ability2Start);
        animator.SetBool("Ability 2 end", pieceCombatActions.ability2End);

        animator.SetBool("Ability 3 start", pieceCombatActions.ability3Start);
        animator.SetBool("Ability 3 end", pieceCombatActions.ability3End);
    }

    public void IFP_SetFlagSprite(Sprite sprite)
    {
        flagSpriteRenderer.sprite = sprite;
        flagSpriteRenderer.sortingOrder = SpriteOrderConstants.FLAG;
    }

    public virtual void ISTET_StartTurn()
    {
        pieceCombatActions.stateWait = false;
        pieceCombatActions.stateDefend = false;

        pieceCombatActions.retaliations = settingsStats.retaliationsMax;
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

    public virtual IEnumerator ICP_InteractWithTargetPiece(AbstractPiece3 targetPiece)
    {
        CombatantPiece3 targetCombatPiece = targetPiece as CombatantPiece3;
        if (pieceOwner.Get() != targetCombatPiece.pieceOwner.Get())
        {
            yield return
                StartCoroutine(pieceCombatActions.Attack(targetCombatPiece));
        }
        else
        {
            //TODO ALLY INTERACTIONS
            //throw new System.NotImplementedException();
        }
    }

    /*
    *   BEGIN:      Receive healing, receive damage, become either hurt or dead
    */
    public virtual IEnumerator ReceiveHealing(int amount)
    {
        healthStats.ReceiveHealing(amount);
        yield return null;
    }
    public virtual IEnumerator ReceiveDamage(int amount)
    {
        bool defeated = healthStats.ReceiveDamage(amount);
        //string log = unit.GetName() + " took " + amount + " damage. " + stackLost + " units died.";   for CombatUnits
        //CombatManager.Instance.AddEntryToLog(log);                                                    for CombatUnits
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
        healthStats.hitPoints_current = 0;

        stateDead = true;
        string stateName = "Dead";
        yield return StartCoroutine(WaitForAnimationStartAndEnd(stateName));

        mainSpriteRenderer.sortingOrder--;
        currentTile.occupantPiece = null;
        (currentTile as CombatTile).deadPieces.Add(this);
        CombatManager.Instance.RemoveUnitFromTurnSequence(this);
    }
    public virtual void Die()
    {
        //TODO: GetStackHealthStats().Subtract(GetStackHealthStats().Get());        for CombatUnits
        StartCoroutine(DamagedDead());
    }
    /*
    *   END:        Receive healing, receive damage, become either hurt or dead
    */
}
