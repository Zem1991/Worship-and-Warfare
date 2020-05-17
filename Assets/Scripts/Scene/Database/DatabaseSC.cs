using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseSC : AbstractSingleton<DatabaseSC>, ISceneController
{
    [Header("Local singletons")]
    [SerializeField] private DatabaseManager databaseManager;
    [SerializeField] private AllPrefabs allPrefabs;

    public override void Awake()
    {
        databaseManager = FindObjectOfType<DatabaseManager>();
        allPrefabs = FindObjectOfType<AllPrefabs>();

        base.Awake();
    }

    public IEnumerator WaitForSceneLoad()
    {
        yield return
            DatabaseManager.CheckReference(databaseManager) &&
            AllPrefabs.CheckReference(allPrefabs);
    }

    public void HideScene()
    {
        Debug.LogWarning(GetType() + " doesn't have objects to hide");
    }

    public void ShowScene()
    {
        Debug.LogWarning(GetType() + " doesn't have objects to show");
    }
}
