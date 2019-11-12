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

    //public override bool ACP_TakeDamage(int amount)
    //{
    //    bool result = combatPieceStats.TakeDamage(amount, stackStats, out int stackLost);
    //    string log = unit.GetName() + " took " + amount + " damage. " + stackLost + " units died.";
    //    CombatManager.Instance.AddEntryToLog(log);

    //    if (result)
    //    {
    //        ACP_Die();
    //        return true;
    //    }
    //    else
    //    {
    //        isHurt = true;
    //        return false;
    //    }
    //}

    //public override void ACP_Die()
    //{
    //    stackStats.stack_current = 0;
    //    base.ACP_Die();
    //}
}
