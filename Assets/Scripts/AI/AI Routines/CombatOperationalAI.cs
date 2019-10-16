using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatOperationalAI : AbstractAIRoutine
{
    [Header("Readings")]
    public AbstractCombatPiece currentUnit;

    [Header("Skill calculations")]
    public int skill;
    //TODO skill item
    //TODO skill target

    [Header("Attack calculations")]
    public int attack;
    public AbstractCombatPiece attackTarget;

    [Header("Other calculations")]
    public int defend;
    public int wait;
    public int move;

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
        AbstractCombatPiece current = CombatManager.Instance.currentPiece;
        currentUnit = current.owner == aiPersonality.player ? current : null;
    }

    private void CalculateSkillPriority()
    {
        skill = 0;
    }

    private void CalculateAttackPriority()
    {
        attack = 0;
        attackTarget = null;

        CombatPieceHandler cph = CombatManager.Instance.pieceHandler;
        List<CombatUnitPiece> unitList;
        if (!cph.GetPieceList(aiPersonality.player, true, out unitList)) return;
        unitList = cph.GetActivePieces(unitList);

        if (currentUnit.hasRangedAttack)
        {
            Dictionary<CombatUnitPiece, float> mapUnitDistance = new Dictionary<CombatUnitPiece, float>();
            foreach (CombatUnitPiece unit in unitList)
            {
                float distance = Vector3.Distance(currentUnit.transform.position, unit.transform.position);
                mapUnitDistance.Add(unit, distance);
            }
            mapUnitDistance = mapUnitDistance.OrderBy(a => a.Value).ToDictionary(a => a.Key, a => a.Value);
            if (mapUnitDistance.Count > 0) attackTarget = mapUnitDistance.First().Key;
        }
        else
        {
            Dictionary<CombatUnitPiece, PathfindResults> mapUnitPath = new Dictionary<CombatUnitPiece, PathfindResults>();
            foreach (CombatUnitPiece unit in unitList)
            {
                Pathfinder.FindPath(currentUnit.currentTile, unit.currentTile, Pathfinder.HexHeuristic,
                    true, true, true,
                    out PathfindResults path);
                mapUnitPath.Add(unit, path);
            }
            mapUnitPath = mapUnitPath.OrderBy(a => a.Value.pathTotalCost).ToDictionary(a => a.Key, a => a.Value);
            if (mapUnitPath.Count > 0) attackTarget = mapUnitPath.First().Key;
        }

        if (attackTarget) attack = 100;   //for now, we can only attack the enemy
    }

    private void CalculateDefendPriority()
    {
        defend = 0;
    }

    private void CalculateWaitPriority()
    {
        wait = 0;
    }

    private void CalculateMovePriority()
    {
        move = 0;
    }

    private void SelectAction()
    {
        int allValues = skill + attack + defend + wait + move;
        sortedValue = Random.Range(0, allValues);
        decision = CombatOperationalDecision.NONE;

        int bottom = 0;
        int top = skill;
        if (skill > 0 &&
            bottom <= sortedValue && sortedValue < top)
        {
            decision = CombatOperationalDecision.SKILL;
            return;
        }

        bottom += skill;
        top = attack;
        if (attack > 0 &&
            bottom <= sortedValue && sortedValue < top)
        {
            decision = CombatOperationalDecision.ATTACK;
            return;
        }

        bottom += attack;
        top = defend;
        if (defend > 0 &&
            bottom <= sortedValue && sortedValue < top)
        {
            decision = CombatOperationalDecision.DEFEND;
            return;
        }

        bottom += defend;
        top = wait;
        if (wait > 0 &&
            bottom <= sortedValue && sortedValue < top)
        {
            decision = CombatOperationalDecision.WAIT;
            return;
        }

        bottom += wait;
        top = move;
        if (move > 0 &&
            bottom <= sortedValue && sortedValue < top)
        {
            decision = CombatOperationalDecision.MOVE;
            return;
        }
    }

    private void PerformAction()
    {
        CombatManager cm = CombatManager.Instance;

        switch (decision)
        {
            case CombatOperationalDecision.SKILL:
                break;
            case CombatOperationalDecision.ATTACK:
                currentUnit.InteractWithPiece(attackTarget, false);
                break;
            case CombatOperationalDecision.DEFEND:
                break;
            case CombatOperationalDecision.WAIT:
                break;
            case CombatOperationalDecision.MOVE:
                break;
            default:
                cm.NextUnit();
                break;
        }
    }
}
