using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Prefab references")]
    public CombatPieceStats combatPieceStats;
    public StackStats stackStats;

    [Header("Database reference")]
    public DB_Unit dbData;

    public void Initialize(DB_Unit dbData, StackData stackData)
    {
        CombatPieceStats prefabCPS = AllPrefabs.Instance.combatPieceStats;
        StackStats prefabSS = AllPrefabs.Instance.stackStats;

        this.dbData = dbData;
        name = dbData.nameSingular;

        combatPieceStats = Instantiate(prefabCPS, transform);
        combatPieceStats.Initialize(dbData.combatPieceStats);

        stackStats = Instantiate(prefabSS, transform);
        stackStats.Initialize(stackData);
    }

    public string GetName()
    {
        if (stackStats.stack_current <= 1) return dbData.nameSingular;
        return dbData.namePlural;
    }
}
