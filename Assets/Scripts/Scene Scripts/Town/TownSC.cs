using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownSC : AbstractSingleton<TownSC>, ISceneController
{
    public IEnumerator WaitForSceneLoad()
    {
        yield return
            TownInputs.Instance &&
            TownUI.Instance &&
            TownManager.Instance;
    }

    public void HideScene()
    {
        TownInputs.Instance.Hide();
        TownUI.Instance.Hide();
        TownManager.Instance.Hide();
    }

    public void ShowScene()
    {
        TownInputs.Instance.Show();
        TownUI.Instance.Show();
        TownManager.Instance.Show();
    }
}
