using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AbstractCombatantPiece2))]
public class PieceCombatActions2 : MonoBehaviour
{
    private AbstractCombatPiece2 piece;

    [Header("Settings")]
    public bool canWait;
    public bool canDefend;
    public bool canRetaliate;
    public bool canCounter;

    [Header("States")]
    public bool stateWait;
    public bool stateDefend;
    public bool stateAttack;
    public bool stateRetaliation;

    [Header("Actions")]
    public bool meleeAttackStart;
    public bool meleeAttackEnd;
    public bool rangedAttackStart;
    public bool rangedAttackEnd;

    [Header("Parameters")]
    public int retaliations;

    [Header("References")]
    public Projectile projectile;

    private void Awake()
    {
        piece = GetComponent<AbstractCombatantPiece2>();
    }

    public bool IsIdle()
    {
        return !stateAttack
            && !stateRetaliation;
    }

    /*
    *   BEGIN:      Wait and Defend
    */
    public IEnumerator Wait()
    {
        stateWait = true;
        CombatManager.Instance.AddUnitToWaitSequence(piece);
        piece.ISTET_EndTurn();
        yield return true;
    }
    public IEnumerator Defend()
    {
        stateDefend = true;
        piece.ISTET_EndTurn();
        yield return true;
    }
    /*
    *   END:        Wait and Defend
    */

    /*
    *   BEGIN:      Attack
    */
    public IEnumerator Attack(AbstractCombatPiece2 target, PathfindResults attackTargetPathfind)
    {
        AbstractCombatantPiece2 pieceAsCombatant = piece as AbstractCombatantPiece2;
        if (pieceAsCombatant) pieceAsCombatant.pieceMovement.SetPath(attackTargetPathfind, pieceAsCombatant.targetTile);
        yield return StartCoroutine(Attack(target));
    }
    public IEnumerator Attack(AbstractCombatPiece2 target)
    {
        AttackStats attack = EvaluateRangedAttack(target) ? piece.combatPieceStats.attack_ranged : piece.combatPieceStats.attack_melee;
        if (attack.isRanged)
        {
            stateAttack = true;
            yield return StartCoroutine(AttackRanged(attack, target));
            stateAttack = false;
        }
        else
        {
            AbstractCombatantPiece2 pieceAsCombatant = piece as AbstractCombatantPiece2;
            if (pieceAsCombatant)
            {
                AbstractTile targetTile = piece.targetPiece.currentTile;
                bool hadPath = pieceAsCombatant.pieceMovement.HasPath(targetTile);
                yield return StartCoroutine(pieceAsCombatant.pieceMovement.Movement(targetTile));
                if (!hadPath) yield break;
            }

            if (EvaluateMeleeAttack(target))
            {
                PieceCombatActions2 targetCombatActions = target.pieceCombatActions;

                bool willCounter = targetCombatActions.EvaluateCounter();
                if (willCounter) yield return StartCoroutine(targetCombatActions.Retaliate(piece));

                stateAttack = true;
                yield return StartCoroutine(AttackMelee(attack, target));
                stateAttack = false;

                bool willRetaliate = willCounter ? false : targetCombatActions.EvaluateRetaliation();
                if (willRetaliate) yield return StartCoroutine(targetCombatActions.Retaliate(piece));
            }
        }
        piece.ISTET_EndTurn();
    }
    private IEnumerator AttackMelee(AttackStats attack, AbstractCombatPiece2 target)
    {
        yield return StartCoroutine(AttackStart(attack));
        int damage = CalculateDamage(attack, target);

        IEnumerator[] ienumerators =
        {
            AttackEnd(attack),
            target.TakeDamage(damage)
        };
        yield return ienumerators.Select(StartCoroutine).ToArray().GetEnumerator();
    }
    private IEnumerator AttackRanged(AttackStats attack, AbstractCombatPiece2 target)
    {
        yield return StartCoroutine(AttackStart(attack));
        SpawnProjectile(attack, target);

        IEnumerator attackEnd = AttackEnd(attack);
        StartCoroutine(attackEnd);
        yield return StartCoroutine(projectile.MakeTrajectory());

        int damage = CalculateDamage(attack, target);

        IEnumerator[] ienumerators =
        {
            attackEnd,
            target.TakeDamage(damage)
        };
        yield return ienumerators.Select(StartCoroutine).ToArray().GetEnumerator();
    }
    private IEnumerator AttackStart(AttackStats attack)
    {
        if (attack.isRanged) rangedAttackStart = true;
        else meleeAttackStart = true;

        string stateName = attack.AttackType() + " start";
        yield return StartCoroutine(piece.WaitForAnimationStartAndEnd(stateName));

        rangedAttackStart = false;
        meleeAttackStart = false;
    }
    private IEnumerator AttackEnd(AttackStats attack)
    {
        if (attack.isRanged) rangedAttackEnd = true;
        else meleeAttackEnd = true;

        string stateName = attack.AttackType() + " end";
        yield return StartCoroutine(piece.WaitForAnimationStartAndEnd(stateName));

        rangedAttackEnd = false;
        meleeAttackEnd = false;
    }
    private int CalculateDamage(AttackStats attack, AbstractCombatPiece2 target)
    {
        CombatPieceHandler cph = CombatManager.Instance.pieceHandler;
        CombatantHeroPiece2 attackerHero = cph.GetHero(piece.pieceOwner.GetOwner());
        CombatantHeroPiece2 defenderHero = cph.GetHero(target.pieceOwner.GetOwner());
        return DamageCalculation.FullDamageCalculation(attack, piece, target, attackerHero, defenderHero);
    }
    private void SpawnProjectile(AttackStats attack, AbstractCombatPiece2 target)
    {
        Projectile prefab = AllPrefabs.Instance.projectile;
        projectile = Instantiate(prefab, transform);
        projectile.SetupAndGo(attack, piece, target);
    }
    public bool EvaluateMeleeAttack(AbstractCombatPiece2 targetPiece)
    {
        bool condition = piece.combatPieceStats.attack_melee && piece.currentTile.IsNeighbour(targetPiece.currentTile);
        return condition;
    }
    public bool EvaluateRangedAttack(AbstractCombatPiece2 targetPiece)
    {
        bool condition = piece.combatPieceStats.attack_ranged && !piece.currentTile.IsNeighbour(targetPiece.currentTile);
        return condition;
    }
    /*
    *   END:        Attack
    */

    /*
    *   BEGIN:      Counter and Retaliation
    */
    public IEnumerator Retaliate(AbstractCombatPiece2 target)
    {
        AttackStats attack = piece.combatPieceStats.attack_melee;

        stateRetaliation = true;
        retaliations--;
        yield return StartCoroutine(AttackMelee(attack, target));
        stateRetaliation = false;
    }
    public bool EvaluateCounter()
    {
        return EvaluateRetaliation() && canCounter;
    }
    public bool EvaluateRetaliation()
    {
        bool condition = !piece.stateDead && canRetaliate;
        return condition && retaliations > 0;
    }
    /*
    *   END:        Counter and Retaliation
    */
}
