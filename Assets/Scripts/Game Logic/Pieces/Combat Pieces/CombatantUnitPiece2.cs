using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatantUnitPiece2 : AbstractCombatantPiece2
{
    [Header("Unit data")]
    public Unit unit;

    [Header("Prefab references")]
    public StackStats stackStats;

    public void Initialize(Unit unit, Player owner, int spawnId, bool defenderSide)
    {
        Initialize(owner, unit.combatPieceStats, spawnId, defenderSide);

        StackStats prefabSS = AllPrefabs.Instance.stackStats;

        this.unit = unit;
        name = "P" + owner.id + " - Stack of " + unit.GetName();

        stackStats = Instantiate(prefabSS, transform);
        stackStats.Initialize(unit.stackStats);

        IMP_ResetMovementPoints();
        SetAnimatorOverrideController(unit.dbData.animatorCombat);
    }

    public override IEnumerator TakeDamage(int amount)
    {
        bool defeated = combatPieceStats.TakeDamage(amount, stackStats, out int stackLost);
        //string log = unit.GetName() + " took " + amount + " damage. " + stackLost + " units died.";
        //CombatManager.Instance.AddEntryToLog(log);
        if (defeated) yield return StartCoroutine(DamagedDead());
        else yield return StartCoroutine(DamagedHurt());
    }
}
