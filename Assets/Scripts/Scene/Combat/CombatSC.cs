using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSC : AbstractSingleton<CombatSC>, ISceneController
{
    public IEnumerator WaitForSceneLoad()
    {
        yield return
            CombatSceneInputs.Instance &&
            CombatUI.Instance &&
            CombatSceneHighlights.Instance &&
            CombatManager.Instance;
    }

    public void HideScene()
    {
        CombatSceneInputs.Instance.Hide();
        CombatUI.Instance.Hide();
        CombatSceneHighlights.Instance.Hide();
        CombatManager.Instance.Hide();
    }

    public void ShowScene()
    {
        CombatSceneInputs.Instance.Show();
        CombatUI.Instance.Show();
        CombatSceneHighlights.Instance.Show();
        CombatManager.Instance.Show();
    }
}
