using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CombatantPiece3))]
public class PieceCombatActions3 : MonoBehaviour
{
    private CombatantPiece3 piece;

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
        piece = GetComponent<CombatantPiece3>();
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
    public IEnumerator Attack(CombatantPiece3 target, PathfindResults attackTargetPathfind)
    {
        UnitPiece3 pieceAsUnit = piece as UnitPiece3;
        if (pieceAsUnit) pieceAsUnit.pieceMovement.SetPath(attackTargetPathfind, pieceAsUnit.targetTile);
        yield return StartCoroutine(Attack(target));
    }
    public IEnumerator Attack(CombatantPiece3 target)
    {
        AttackStats2 attack = EvaluateRangedAttack(target) ? piece.offenseStats.attack_ranged : piece.offenseStats.attack_melee;
        if (attack.IsRanged())
        {
            stateAttack = true;
            yield return StartCoroutine(AttackRanged(attack, target));
            stateAttack = false;
        }
        else
        {
            UnitPiece3 pieceAsCombatant = piece as UnitPiece3;
            if (pieceAsCombatant)
            {
                AbstractTile targetTile = piece.targetPiece.currentTile;
                bool hasPath = pieceAsCombatant.pieceMovement.HasPath(targetTile);
                yield return StartCoroutine(pieceAsCombatant.pieceMovement.Movement(targetTile));
                if (!hasPath) yield break;
            }

            if (EvaluateMeleeAttack(target))
            {
                PieceCombatActions3 targetCombatActions = target.pieceCombatActions;

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
    private IEnumerator AttackMelee(AttackStats2 attack, CombatantPiece3 target)
    {
        yield return StartCoroutine(AttackStart(attack));
        int damage = CalculateDamage(attack, target);

        IEnumerator[] ienumerators =
        {
            AttackEnd(attack),
            target.ReceiveDamage(damage)
        };
        yield return ienumerators.Select(StartCoroutine).ToArray().GetEnumerator();
    }
    private IEnumerator AttackRanged(AttackStats2 attack, CombatantPiece3 target)
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
            target.ReceiveDamage(damage)
        };
        yield return ienumerators.Select(StartCoroutine).ToArray().GetEnumerator();
    }
    private IEnumerator AttackStart(AttackStats2 attack)
    {
        if (attack.IsRanged()) rangedAttackStart = true;
        else meleeAttackStart = true;

        string stateName = attack.GetAnimatorStateName() + " start";
        yield return StartCoroutine(piece.WaitForAnimationStartAndEnd(stateName));

        rangedAttackStart = false;
        meleeAttackStart = false;
    }
    private IEnumerator AttackEnd(AttackStats2 attack)
    {
        if (attack.IsRanged()) rangedAttackEnd = true;
        else meleeAttackEnd = true;

        string stateName = attack.GetAnimatorStateName() + " end";
        yield return StartCoroutine(piece.WaitForAnimationStartAndEnd(stateName));

        rangedAttackEnd = false;
        meleeAttackEnd = false;
    }
    private int CalculateDamage(AttackStats2 attack, CombatantPiece3 target)
    {
        CombatPieceHandler cph = CombatManager.Instance.pieceHandler;
        HeroUnitPiece3 attackerHero = cph.GetHero(piece.pieceOwner.GetOwner());
        HeroUnitPiece3 defenderHero = cph.GetHero(target.pieceOwner.GetOwner());
        return AttackCalculation.FullDamageCalculation(attack, piece, target, attackerHero, defenderHero);
    }
    private void SpawnProjectile(AttackStats2 attack, CombatantPiece3 target)
    {
        Projectile prefab = AllPrefabs.Instance.projectile;
        projectile = Instantiate(prefab, transform);
        projectile.SetupAndGo(attack, piece, target);
    }
    public bool EvaluateMeleeAttack(CombatantPiece3 targetPiece)
    {
        bool condition = piece.offenseStats.attack_melee && piece.currentTile.IsNeighbour(targetPiece.currentTile);
        return condition;
    }
    public bool EvaluateRangedAttack(CombatantPiece3 targetPiece)
    {
        bool condition = piece.offenseStats.attack_ranged && !piece.currentTile.IsNeighbour(targetPiece.currentTile);
        return condition;
    }
    /*
    *   END:        Attack
    */

    /*
    *   BEGIN:      Counter and Retaliation
    */
    public IEnumerator Retaliate(CombatantPiece3 target)
    {
        AttackStats2 attack = piece.offenseStats.attack_melee;

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
