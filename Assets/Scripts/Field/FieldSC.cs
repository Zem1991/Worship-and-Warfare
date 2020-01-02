using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldSC : AbstractSingleton<FieldSC>, ISceneController
{
    public IEnumerator WaitForSceneLoad()
    {
        yield return
            FieldSceneInputs.Instance &&
            FieldUI.Instance &&
            FieldSceneHighlights.Instance &&
            FieldManager.Instance;
    }

    public void HideScene()
    {
        FieldSceneInputs.Instance.Hide();
        FieldUI.Instance.Hide();
        FieldSceneHighlights.Instance.Hide();
        FieldManager.Instance.Hide();
    }

    public void ShowScene()
    {
        FieldSceneInputs.Instance.Show();
        FieldUI.Instance.Show();
        FieldSceneHighlights.Instance.Show();
        FieldManager.Instance.Show();
    }
}
