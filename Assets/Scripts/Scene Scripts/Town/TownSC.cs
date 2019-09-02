using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownSC : AbstractSingleton<TownSC>, ISceneController
{
    public IEnumerator ConfirmSceneLoaded()
    {
        yield return
            TownInputs.Instance &&
            TownUI.Instance &&
            TownManager.Instance;
    }

    public void HideObjects()
    {
        TownInputs.Instance.Hide();
        TownUI.Instance.Hide();
        TownManager.Instance.Hide();
    }

    public void ShowObjects()
    {
        TownInputs.Instance.Show();
        TownUI.Instance.Show();
        TownManager.Instance.Show();
    }
}
