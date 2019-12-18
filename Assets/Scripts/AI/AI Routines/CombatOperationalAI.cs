using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatOperationalAI : AbstractAIRoutine
{
    [Header("Readings")]
    public AbstractCombatActorPiece2 currentUnit;

    [Header("Skill calculations")]
    public int skillChance;
    //TODO skill item
    //TODO skill target

    [Header("Attack calculations")]
    public int attackChance;
    public AbstractCombatActorPiece2 attackTarget;
    public PathfindResults attackTargetPathfind;

    [Header("Other calculations")]
    public int defendChance;
    public int waitChance;
    public int moveChance;

    [Header("Decision made")]
    public int sortedValue;
    public CombatOperationalDecision decision;

    public override void ReadContext()
    {
        ReadCurrentUnit();
    }

    public override void MakeCalculations()
    {
        CalculateSkillPriority();
        CalculateAttackPriority();
        CalculateDefendPriority();
        CalculateWaitPriority();
        CalculateMovePriority();
    }

    public override void TakeDecision()
    {
        SelectAction();
        PerformAction();
    }

    private void ReadCurrentUnit()
    {
        AbstractCombatActorPiece2 current = CombatManager.Instance.currentPiece;
        currentUnit = current.pieceOwner.GetOwner() == aiPersonality.player ? current : null;
    }

    private void CalculateSkillPriority()
    {
        skillChance = 0;
    }

    private void CalculateAttackPriority()
    {
        attackChance = 0;
        attackTarget = null;

        Dictionary<AbstractCombatActorPiece2, PathfindResults> meleeTargets = new Dictionary<AbstractCombatActorPiece2, PathfindResults>();
        Dictionary<AbstractCombatActorPiece2, PathfindResults> rangedTargets = new Dictionary<AbstractCombatActorPiece2, PathfindResults>();
        bool isRangedViable = currentUnit.combatPieceStats.attack_ranged;

        CombatPieceHandler cph = CombatManager.Instance.pieceHandler;
        if (!cph.GetPieceList(aiPersonality.player, true, out List<AbstractCombatActorPiece2> unitList)) return;

        //Sort enemy pieces based on how accessible they are to be approached.
        Dictionary<AbstractCombatActorPiece2, PathfindResults> mapUnitPath = new Dictionary<AbstractCombatActorPiece2, PathfindResults>();
        foreach (AbstractCombatActorPiece2 unit in unitList)
        {
            if (!unit.currentTile)
            {
                Debug.Log("!!!");
            }

            Pathfinder.FindPath(currentUnit.currentTile, unit.currentTile,
                Pathfinder.HexHeuristic, true, false, false,
                out PathfindResults path);
            mapUnitPath.Add(unit, path);

            //If any enemy unit is adjacent to this unit, then it will prefer to not do ranged attacks.
            //TODO actually make something of this.
            if (currentUnit.currentTile.IsNeighbour(unit.currentTile)) isRangedViable = false;
        }
        meleeTargets = mapUnitPath.OrderBy(a => a.Value.pathTotalCost).ToDictionary(a => a.Key, a => a.Value);

        attackTarget = meleeTargets.Keys.First();
        attackTargetPathfind = meleeTargets.Values.First();
        attackChance = 100;     //for now, we can only attack the enemy
    }

    private void CalculateDefendPriority()
    {
        defendChance = 0;
    }

    private void CalculateWaitPriority()
    {
        waitChance = 0;
    }

    private void CalculateMovePriority()
    {
        moveChance = 0;
    }

    private void SelectAction()
    {
        int allValues = skillChance + attackChance + defendChance + waitChance + moveChance;
        sortedValue = Random.Range(0, allValues);
        decision = CombatOperationalDecision.NONE;

        int bottom = 0;
        int top = skillChance;
        if (skillChance > 0 &&
            bottom <= sortedValue && sortedValue < top)
        {
            decision = CombatOperationalDecision.SKILL;
            return;
        }

        bottom += skillChance;
        top = attackChance;
        if (attackChance > 0 &&
            bottom <= sortedValue && sortedValue < top)
        {
            decision = CombatOperationalDecision.ATTACK;
            return;
        }

        bottom += attackChance;
        top = defendChance;
        if (defendChance > 0 &&
            bottom <= sortedValue && sortedValue < top)
        {
            decision = CombatOperationalDecision.DEFEND;
            return;
        }

        bottom += defendChance;
        top = waitChance;
        if (waitChance > 0 &&
            bottom <= sortedValue && sortedValue < top)
        {
            decision = CombatOperationalDecision.WAIT;
            return;
        }

        bottom += waitChance;
        top = moveChance;
        if (moveChance > 0 &&
            bottom <= sortedValue && sortedValue < top)
        {
            decision = CombatOperationalDecision.MOVE;
            return;
        }
    }

    private void PerformAction()
    {
        IEnumerator coroutine = null;
        switch (decision)
        {
            case CombatOperationalDecision.SKILL:
                break;
            case CombatOperationalDecision.ATTACK:
                currentUnit.targetTile = attackTarget.currentTile;
                currentUnit.targetPiece = attackTarget;
                coroutine = currentUnit.pieceCombatActions.Attack(attackTarget, attackTargetPathfind);
                break;
            case CombatOperationalDecision.DEFEND:
                coroutine = currentUnit.pieceCombatActions.Defend();
                break;
            case CombatOperationalDecision.WAIT:
                coroutine = currentUnit.pieceCombatActions.Wait();
                break;
            case CombatOperationalDecision.MOVE:
                break;
        }

        StartCoroutine(coroutine);
    }
}
