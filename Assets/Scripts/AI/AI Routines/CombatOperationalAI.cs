using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatOperationalAI : AbstractAIRoutine
{
    [Header("Readings")]
    public AbstractCombatPiece2 currentUnit;

    [Header("Skill calculations")]
    public int skillChance;
    //TODO skill item
    //TODO skill target

    [Header("Attack calculations")]
    public int attackChance;
    public AbstractCombatPiece2 attackTarget;

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
        AbstractCombatPiece2 current = CombatManager.Instance.currentPiece;
        currentUnit = current.GetOwner() == aiPersonality.player ? current : null;
    }

    private void CalculateSkillPriority()
    {
        skillChance = 0;
    }

    private void CalculateAttackPriority()
    {
        attackChance = 0;
        attackTarget = null;

        CombatPieceHandler cph = CombatManager.Instance.pieceHandler;
        List<AbstractCombatPiece2> unitList;
        if (!cph.GetPieceList(aiPersonality.player, true, out unitList)) return;
        unitList = cph.GetActivePieces(unitList);

        if (currentUnit.combatPieceStats.attack_ranged && currentUnit.pieceCombatActions.EvaluateRangedAttack())
        {
            Dictionary<AbstractCombatPiece2, float> mapUnitDistance = new Dictionary<AbstractCombatPiece2, float>();
            foreach (AbstractCombatPiece2 unit in unitList)
            {
                float distance = Vector3.Distance(currentUnit.transform.position, unit.transform.position);
                mapUnitDistance.Add(unit, distance);
            }
            mapUnitDistance = mapUnitDistance.OrderBy(a => a.Value).ToDictionary(a => a.Key, a => a.Value);
            if (mapUnitDistance.Count > 0) attackTarget = mapUnitDistance.First().Key;
        }
        else
        {
            Dictionary<AbstractCombatPiece2, PathfindResults> mapUnitPath = new Dictionary<AbstractCombatPiece2, PathfindResults>();
            foreach (AbstractCombatPiece2 unit in unitList)
            {
                Pathfinder.FindPath(currentUnit.currentTile, unit.currentTile, Pathfinder.HexHeuristic,
                    true, true, true,
                    out PathfindResults path);
                mapUnitPath.Add(unit, path);
            }
            mapUnitPath = mapUnitPath.OrderBy(a => a.Value.pathTotalCost).ToDictionary(a => a.Key, a => a.Value);
            if (mapUnitPath.Count > 0) attackTarget = mapUnitPath.First().Key;
        }

        if (attackTarget) attackChance = 100;   //for now, we can only attack the enemy
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
                currentUnit.targetPiece = attackTarget;
                coroutine = currentUnit.pieceCombatActions.Attack();
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
