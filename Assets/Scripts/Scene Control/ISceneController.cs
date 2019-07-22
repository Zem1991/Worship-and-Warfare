using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISceneController
{
    IEnumerator ConfirmSceneLoaded();

    void HideObjects();

    void ShowObjects();
}
