using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseSC : AbstractSingleton<DatabaseSC>, ISceneController
{
    public IEnumerator ConfirmSceneLoaded()
    {
        yield return 
            DatabaseManager.Instance &&
            AllPrefabs.Instance;
    }

    public void HideObjects()
    {
        Debug.LogWarning(GetType() + " doesn't have objects to hide");
    }

    public void ShowObjects()
    {
        Debug.LogWarning(GetType() + " doesn't have objects to hide");
    }
}
