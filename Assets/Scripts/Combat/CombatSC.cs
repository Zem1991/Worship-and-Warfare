using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSC : AbstractSingleton<CombatSC>, ISceneController
{
    public IEnumerator WaitForSceneLoad()
    {
        yield return
            CombatInputs.Instance &&
            CombatUI.Instance &&
            CombatManager.Instance;
    }

    public void HideScene()
    {
        CombatInputs.Instance.Hide();
        CombatUI.Instance.Hide();
        CombatManager.Instance.Hide();
    }

    public void ShowScene()
    {
        CombatInputs.Instance.Show();
        CombatUI.Instance.Show();
        CombatManager.Instance.Show();
    }
}
