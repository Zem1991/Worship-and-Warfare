using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISceneController
{
    IEnumerator WaitForSceneLoad();
    void HideScene();
    void ShowScene();
}
