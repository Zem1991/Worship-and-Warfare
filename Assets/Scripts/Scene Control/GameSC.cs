using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSC : Singleton<GameSC>, ISceneController
{
    public IEnumerator ConfirmSceneLoaded()
    {
        yield return
            GameManager.Instance &&
            ScenarioManager.Instance &&
            PlayerManager.Instance &&
            InputManager.Instance &&
            UIManager.Instance;
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
