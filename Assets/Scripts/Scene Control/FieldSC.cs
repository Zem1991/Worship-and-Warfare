using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldSC : Singleton<FieldSC>, ISceneController
{
    public IEnumerator ConfirmSceneLoaded()
    {
        yield return
            MapManager.Instance &&
            PieceManager.Instance;
    }

    public void HideObjects()
    {
        FieldInputs.Instance.gameObject.SetActive(false);
        FieldUI.Instance.gameObject.SetActive(false);
        MapManager.Instance.gameObject.SetActive(false);
        PieceManager.Instance.gameObject.SetActive(false);
    }

    public void ShowObjects()
    {
        FieldInputs.Instance.gameObject.SetActive(true);
        FieldUI.Instance.gameObject.SetActive(true);
        MapManager.Instance.gameObject.SetActive(true);
        PieceManager.Instance.gameObject.SetActive(true);
    }
}
