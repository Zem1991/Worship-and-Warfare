using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceMovement))]
public abstract class AbstractCombatantPiece2 : AbstractCombatPiece2, IPlayerOwnable, IPlayerControllable, ICommandablePiece, IMovablePiece
{
    protected PieceMovement pieceMovement;

    [Header("Combat identification")]
    public int spawnId;
    public bool defenderSide;

    [Header("Combatant actions")]
    public AbstractCombatPiece2 retaliationTarget;
    public bool isAttacking_Start;
    public bool isAttacking_End;
    public bool isHurt;

    [Header("Combatant states")]
    public bool stateWaiting;
    public bool stateDefending;

    [Header("Animator parameters")]
    private bool anim_movement;
    private float anim_directionX;
    private bool anim_attacking_start;
    private bool anim_attacking_end;
    private bool anim_hurt;
    private bool anim_dead;

    [Header("Prefab references")]
    public CombatPieceStats combatPieceStats;

    protected override void Awake()
    {
        base.Awake();
        canBeOwned = true;
        canBeControlled = true;
        pieceMovement = GetComponent<PieceMovement>();
    }

    protected override void Update()
    {
        base.Update();
        IMP_MakeMove();
        ACtP_MakeAttack();
        ACtP_MakeHurt();
    }

    public void Initialize(Player owner, CombatPieceStats cps, int spawnId, bool defenderSide)
    {
        CombatPieceStats prefabCPS = AllPrefabs.Instance.combatPieceStats;

        canBeOwned = true;
        canBeControlled = true;

        this.owner = owner;
        this.spawnId = spawnId;
        this.defenderSide = defenderSide;

        combatPieceStats = Instantiate(prefabCPS, transform);
        combatPieceStats.Initialize(cps);

        FlipSpriteHorizontally(defenderSide);
        SetFlagSprite(owner.dbColor.imgFlag);
    }

    public virtual void ACtP_ResetMovementPoints()
    {
        if (!pieceMovement) pieceMovement = GetComponent<PieceMovement>();
        pieceMovement.movementPointsMax = combatPieceStats.movementRange * 100;
        pieceMovement.movementPointsCurrent = pieceMovement.movementPointsMax;
    }

    public virtual void ACtP_MakeAttack()
    {
        if (!isAttacking_Start && !isAttacking_End) return;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if (state.normalizedTime >= 1)
        {
            AbstractCombatPiece2 acp = pieceMovement.targetPiece as AbstractCombatPiece2;
            AbstractCombatantPiece2 actp = pieceMovement.targetPiece as AbstractCombatantPiece2;

            if (isAttacking_Start &&
                state.IsName("Attack Start"))
            {
                isAttacking_Start = false;
                isAttacking_End = true;
                int dmg = ACtP_CalculateDamage();
                acp.ACP_TakeDamage(dmg);
            }
            if (isAttacking_End &&
                state.IsName("Attack End"))
            {
                isAttacking_End = false;
                pieceMovement.targetPiece = null;
                if (actp && actp.retaliationTarget && !actp.isDead)
                {
                    actp.ACtP_Retaliate();
                }
                else
                {
                    ICP_EndTurn();
                }
            }
        }
    }

    public virtual void ACtP_MakeHurt()
    {
        if (!isHurt) return;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if (state.normalizedTime >= 1)
        {
            if (state.IsName("Hurt"))
            {
                isHurt = false;
            }
        }
    }

    public virtual int ACtP_CalculateDamage()
    {
        AbstractCombatantPiece2 targetUnit = pieceMovement.targetPiece as AbstractCombatantPiece2;
        CombatPieceHandler cph = CombatManager.Instance.pieceHandler;
        CombatantHeroPiece2 attackerHero = cph.GetHero(owner);
        CombatantHeroPiece2 defenderHero = cph.GetHero(targetUnit.IPO_GetOwner());
        return DamageCalculation.FullDamageCalculation(this, targetUnit, attackerHero, defenderHero);
    }

    public virtual void ACtP_Attack(bool ranged)
    {
        AbstractCombatantPiece2 actp = pieceMovement.targetPiece as AbstractCombatantPiece2;

        if (!ranged && actp)
        {
            CombatManager.Instance.retaliatorPiece = actp;
            actp.retaliationTarget = this;
        }
        isAttacking_Start = true;
        //Debug.LogWarning("InteractWithPiece insta-killed the target!");
        //targetUnit.hitPointsCurrent = 0;
    }

    public virtual void ACtP_Retaliate()
    {
        pieceMovement.targetPiece = retaliationTarget;
        retaliationTarget = null;
        isAttacking_Start = true;
    }

    public virtual void ACtP_Wait()
    {
        stateWaiting = true;
        CombatManager.Instance.AddUnitToWaitSequence(this);
        ICP_EndTurn();
    }

    public virtual void ACtP_Defend()
    {
        stateDefending = true;
        ICP_EndTurn();
    }

    public override void AP2_AnimatorParameters()
    {
        anim_movement = pieceMovement.inMovement;
        animator.SetBool("Movement", anim_movement);

        //anim_directionX = 0;
        //if (direction.x < 0) anim_directionX = -1;
        //if (direction.x > 0) anim_directionX = 1;
        //animator.SetFloat("Direction X", anim_directionX);

        anim_attacking_start = isAttacking_Start;
        animator.SetBool("Attack Start", anim_attacking_start);

        anim_attacking_end = isAttacking_End;
        animator.SetBool("Attack End", anim_attacking_end);

        anim_hurt = isHurt;
        animator.SetBool("Hurt", anim_hurt);

        anim_dead = isDead;
        animator.SetBool("Dead", anim_dead);
    }

    public override void AP2_PieceInteraction()
    {
        if (pieceMovement.targetPiece)
        {
            AbstractCombatantPiece2 acp = pieceMovement.targetPiece as AbstractCombatantPiece2;
            bool hasOwner = acp.IPO_HasOwner();
            bool difOwner = acp.IPO_GetOwner() != owner;
            if (hasOwner && difOwner)
            {
                //TODO check ranged interaction
                ACtP_Attack(combatPieceStats.attack_primary.isRanged);
            }
        }
    }

    public override bool ACP_TakeDamage(int amount)
    {
        bool result = base.ACP_TakeDamage(amount);
        if (!result) isHurt = true;
        return result;
    }

    public override void ACP_Die()
    {
        base.ACP_Die();
        CombatManager.Instance.RemoveUnitFromTurnSequence(this);
    }

    public bool IPO_HasOwner()
    {
        return canBeOwned && owner;
    }

    public Player IPO_GetOwner()
    {
        return owner;
    }

    public bool IPC_HasController()
    {
        return canBeControlled && controller;
    }

    public Player IPC_GetController()
    {
        return controller;
    }

    public void IPC_SetController(Player player)
    {
        controller = player;
    }

    public void ICP_StartTurn()
    {
        stateWaiting = false;
        stateDefending = false;

        IMP_ResetMovementPoints();
    }

    public void ICP_EndTurn()
    {
        CombatManager cm = CombatManager.Instance;
        if (cm.currentPiece == this) cm.NextUnit();
        else if (cm.retaliatorPiece == this) cm.NextUnit();
    }

    public void ICP_InteractWithTile(AbstractTile aTile, bool canPathfind)
    {
        if (canPathfind)
        {
            CombatTile cTile = aTile as CombatTile;
            if (cTile)
            {
                AbstractPiece2 targetPiece = cTile.occupantPiece;
                if (targetPiece)
                {
                    ICP_InteractWithPiece(targetPiece, canPathfind);
                }
                else
                {
                    if (pieceMovement.HasPath(cTile)) IMP_Move();
                    else CombatManager.Instance.pieceHandler.Pathfind(this, cTile);
                    //if (!HasPath(cTile)) CombatManager.Instance.pieceHandler.Pathfind(this, cTile);
                    //Move();
                }
            }
        }
        else
        {
            if (pieceMovement.HasPath()) IMP_Move();
        }
    }

    public void ICP_InteractWithPiece(AbstractPiece2 aPiece, bool canPathfind)
    {
        bool justInteract = false;

        AbstractCombatantPiece2 targetACP = aPiece as AbstractCombatantPiece2;
        if (targetACP &&
            !targetACP.isDead &&
            owner != targetACP.owner &&
            combatPieceStats.attack_primary.isRanged)
        {
            //TODO add check if we are not in melee range
            //TODO maybe add cases for ally ranged interactions ?
            justInteract = true;
        }

        if (justInteract)
        {
            pieceMovement.targetPiece = targetACP;
            AP2_PieceInteraction();
            return;
        }

        CombatTile cTile = aPiece.currentTile as CombatTile;
        if (canPathfind)
        {
            if (pieceMovement.HasPath(cTile)) IMP_Move();
            else CombatManager.Instance.pieceHandler.Pathfind(this, cTile);
        }
        else
        {
            CombatManager.Instance.pieceHandler.Pathfind(this, cTile);
            if (pieceMovement.HasPath(cTile)) IMP_Move();
        }
    }

    public bool ICP_IsIdle()
    {
        return !pieceMovement.inMovement
            && !isAttacking_Start
            && !isAttacking_End
            && !isHurt
            && !isDead;
    }

    public void IMP_ResetMovementPoints()
    {
        ACtP_ResetMovementPoints();
    }

    public void IMP_Move()
    {
        pieceMovement.inMovement = true;
    }

    public void IMP_Stop()
    {
        pieceMovement.stopWasCalled = true;
    }

    public void IMP_MakeMove()
    {
        if (CombatManager.Instance.IsCombatRunning())
        {
            bool inMove = pieceMovement.inMovement;
            pieceMovement.MakeMove();
            if (inMove && ICP_IsIdle()) ICP_EndTurn();
        }
    }

    public PieceMovement IMP_GetPieceMovement()
    {
        return pieceMovement;
    }
}
