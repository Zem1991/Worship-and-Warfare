using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownSC : AbstractSingleton<TownSC>, ISceneController
{
    public IEnumerator WaitForSceneLoad()
    {
        yield return
            TownSceneInputs.Instance &&
            TownUI.Instance &&
            TownManager.Instance;
    }

    public void HideScene()
    {
        TownSceneInputs.Instance.Hide();
        TownUI.Instance.Hide();
        TownManager.Instance.Hide();
    }

    public void ShowScene()
    {
        TownSceneInputs.Instance.Show();
        TownUI.Instance.Show();
        TownManager.Instance.Show();
    }
}
