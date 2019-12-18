using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldSC : AbstractSingleton<FieldSC>, ISceneController
{
    public IEnumerator WaitForSceneLoad()
    {
        yield return
            FieldInputs.Instance &&
            FieldUI.Instance &&
            FieldManager.Instance;
    }

    public void HideScene()
    {
        FieldInputs.Instance.Hide();
        FieldUI.Instance.Hide();
        FieldManager.Instance.Hide();
    }

    public void ShowScene()
    {
        FieldInputs.Instance.Show();
        FieldUI.Instance.Show();
        FieldManager.Instance.Show();
    }
}
