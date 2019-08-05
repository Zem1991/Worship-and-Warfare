using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldSC : AbstractSingleton<FieldSC>, ISceneController
{
    public IEnumerator ConfirmSceneLoaded()
    {
        yield return FieldManager.Instance;
    }

    public void HideObjects()
    {
        FieldInputs.Instance.Hide();
        FieldUI.Instance.Hide();
        FieldManager.Instance.Hide();
    }

    public void ShowObjects()
    {
        FieldInputs.Instance.Show();
        FieldUI.Instance.Show();
        FieldManager.Instance.Show();
    }
}
