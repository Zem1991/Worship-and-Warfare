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

    [Header("Parameters")]
    public int retaliations;
    public int retaliationsMax;

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
    public IEnumerator Attack(AttackStats attack)
    {
        AbstractCombatPiece2 targetasCombatPiece = piece.targetPiece as AbstractCombatPiece2;
        AbstractCombatantPiece2 pieceAsCombatant = piece as AbstractCombatantPiece2;

        stateAttack = true;
        if (attack.isRanged && EvaluateRangedAttack())
        {
            yield return StartCoroutine(AttackRanged(attack, targetasCombatPiece));
        }
        else
        {
            PieceCombatActions2 targetCombatActions = targetasCombatPiece.pieceCombatActions;
            AttackStats targetMeleeAttack = targetasCombatPiece.combatPieceStats.attack_primary;

            if (pieceAsCombatant) yield return StartCoroutine(pieceAsCombatant.pieceMovement.Movement());

            bool willCounter = targetCombatActions.EvaluateCounter();
            if (willCounter) yield return StartCoroutine(targetCombatActions.Retaliate(targetMeleeAttack, piece));

            yield return StartCoroutine(AttackMelee(attack, targetasCombatPiece));

            bool willRetaliate = willCounter ? false : targetCombatActions.EvaluateRetaliation();
            if (willRetaliate) yield return StartCoroutine(targetCombatActions.Retaliate(targetMeleeAttack, piece));
        }
        stateAttack = false;
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
        AnimatorStateInfo state = piece.GetAnimatorStateInfo();
        while (state.IsName(attack.AttackType() + " start")) yield return null;
    }
    private IEnumerator AttackEnd(AttackStats attack)
    {
        AnimatorStateInfo state = piece.GetAnimatorStateInfo();
        while (state.IsName(attack.AttackType() + " end")) yield return null;
    }
    private int CalculateDamage(AttackStats attack, AbstractCombatPiece2 target)
    {
        CombatPieceHandler cph = CombatManager.Instance.pieceHandler;
        CombatantHeroPiece2 attackerHero = cph.GetHero(piece.GetOwner());
        CombatantHeroPiece2 defenderHero = cph.GetHero(target.GetOwner());
        return DamageCalculation.FullDamageCalculation(attack, piece, target, attackerHero, defenderHero);
    }
    private void SpawnProjectile(AttackStats attack, AbstractCombatPiece2 target)
    {
        Projectile prefab = AllPrefabs.Instance.projectile;
        projectile = Instantiate(prefab, transform);
        projectile.SetupAndGo(attack, piece, target);
    }
    public bool EvaluateRangedAttack()
    {
        bool condition = piece.currentTile.IsNeighbour(piece.targetTile);
        return condition;
    }
    /*
    *   END:        Attack
    */

    /*
    *   BEGIN:      Counter and Retaliation
    */
    public IEnumerator Retaliate(AttackStats attack, AbstractCombatPiece2 target)
    {
        stateRetaliation = true;
        retaliations--;
        yield return StartCoroutine(AttackMelee(attack, target));
        stateRetaliation = false;
    }
    public bool EvaluateCounter()
    {
        bool condition = canCounter && canRetaliate;
        return condition && retaliations > 0;
    }
    public bool EvaluateRetaliation()
    {
        bool condition = canRetaliate;
        return condition && retaliations > 0;
    }
    /*
    *   END:        Counter and Retaliation
    */
}
