using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceOwner3))]
[RequireComponent(typeof(PieceController3))]
[RequireComponent(typeof(PieceCombatActions3))]
public class CombatantPiece3 : AbstractCombatPiece3, IFlaggablePiece, IStartTurnEndTurn, ICommandablePiece
{
    [Header("Object components")]
    public SpriteRenderer flagSpriteRenderer;

    [Header("Object components")]
    public PieceOwner3 pieceOwner;
    public PieceController3 pieceController;
    public PieceCombatActions3 pieceCombatActions;

    [Header("Stats references")]
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
        pieceOwner.SetOwner(owner);
        pieceController.SetController(owner);

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

    public virtual void Initialize(Player owner, int spawnId, bool onDefenderSide, AbstractUnit abstractUnit)
    {
        Initialize(owner, spawnId, onDefenderSide);

        settingsStats = Instantiate(abstractUnit.settingsStats, transform);
        healthStats = Instantiate(abstractUnit.healthStats, transform);
        movementStats = Instantiate(abstractUnit.movementStats, transform);
        attributeStats = Instantiate(abstractUnit.attributeStats, transform);
        offenseStats = Instantiate(abstractUnit.offenseStats, transform);
        defenseStats = Instantiate(abstractUnit.defenseStats, transform);
        abilityStats = Instantiate(abstractUnit.abilityStats, transform);
    }

    protected override void AP3_UpdateAnimatorParameters()
    {
        animator.SetBool("Hurt", stateHurt);
        animator.SetBool("Dead", stateDead);

        animator.SetBool("Melee attack start", pieceCombatActions.meleeAttackStart);
        animator.SetBool("Melee attack end", pieceCombatActions.meleeAttackEnd);

        animator.SetBool("Ranged attack start", pieceCombatActions.rangedAttackStart);
        animator.SetBool("Ranged attack end", pieceCombatActions.rangedAttackEnd);
    }

    //public override string AFP3_GetPieceTitle()
    //{
    //    AbstractUnit ape = party.GetMostRelevant();
    //    UnitType partyElementType = ape.unitType;

    //    string result = "Unknown party piece title";
    //    switch (partyElementType)
    //    {
    //        case UnitType.HERO:
    //            result = (ape as HeroUnit).dbData.heroName + "'s party";
    //            break;
    //        case UnitType.CREATURE:
    //            result = "Non-commissioned party";
    //            break;
    //        case UnitType.SUPPORT:
    //            break;
    //    }
    //    return result;
    //}

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
        if (pieceOwner.GetOwner() != targetCombatPiece.pieceOwner.GetOwner())
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
