using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSC : Singleton<CombatSC>, ISceneController
{
    public IEnumerator ConfirmSceneLoaded()
    {
        yield return CombatManager.Instance;
    }

    public void HideObjects()
    {
        CombatInputs.Instance.gameObject.SetActive(false);
        CombatUI.Instance.gameObject.SetActive(false);
        CombatManager.Instance.gameObject.SetActive(false);
    }

    public void ShowObjects()
    {
        CombatInputs.Instance.gameObject.SetActive(true);
        CombatUI.Instance.gameObject.SetActive(true);
        CombatManager.Instance.gameObject.SetActive(true);
    }
}
