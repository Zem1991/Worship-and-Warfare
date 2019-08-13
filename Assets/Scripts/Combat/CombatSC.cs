using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSC : AbstractSingleton<CombatSC>, ISceneController
{
    public IEnumerator ConfirmSceneLoaded()
    {
        yield return
            CombatInputs.Instance &&
            CombatUI.Instance &&
            CombatManager.Instance;
    }

    public void HideObjects()
    {
        CombatInputs.Instance.Hide();
        CombatUI.Instance.Hide();
        CombatManager.Instance.Hide();
    }

    public void ShowObjects()
    {
        CombatInputs.Instance.Show();
        CombatUI.Instance.Show();
        CombatManager.Instance.Show();
    }
}
