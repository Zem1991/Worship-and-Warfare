using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : AbstractSingleton<MainMenuManager>
{
    public string TEST_SCENARIO_NAME;

    public void BootTestScenario01()
    {
        StartCoroutine(Main.Instance.LoadGameScene(TEST_SCENARIO_NAME));
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
