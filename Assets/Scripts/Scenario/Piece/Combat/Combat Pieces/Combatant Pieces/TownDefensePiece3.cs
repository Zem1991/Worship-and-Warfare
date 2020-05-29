using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownDefensePiece3 : CombatantPiece3
{
    [Header("Database references")]
    [SerializeField] protected DB_TownDefense dbTownDefense;

    public void Initialize(Player owner, int spawnId, DB_TownDefense dbTownDefense)
    {
        Initialize(owner, spawnId, true);

        this.dbTownDefense = dbTownDefense;
        name = dbTownDefense.structureName + " [" + spawnId + "]";

        settingsStats.CopyFrom(dbTownDefense.settingsStats);
        healthStats.CopyFrom(dbTownDefense.healthStats);
        offenseStats.CopyFrom(dbTownDefense.offenseStats);
        defenseStats.CopyFrom(dbTownDefense.defenseStats);

        SetAnimatorOverrideController(dbTownDefense.animatorCombat);
    }
}
