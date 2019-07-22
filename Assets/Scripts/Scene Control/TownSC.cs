using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownSC : Singleton<TownSC>, ISceneController
{
    public IEnumerator ConfirmSceneLoaded()
    {
        yield return TownManager.Instance;
    }

    public void HideObjects()
    {
        TownInputs.Instance.gameObject.SetActive(false);
        TownUI.Instance.gameObject.SetActive(false);
        TownManager.Instance.gameObject.SetActive(false);
    }

    public void ShowObjects()
    {
        TownInputs.Instance.gameObject.SetActive(true);
        TownUI.Instance.gameObject.SetActive(true);
        TownManager.Instance.gameObject.SetActive(true);
    }
}
