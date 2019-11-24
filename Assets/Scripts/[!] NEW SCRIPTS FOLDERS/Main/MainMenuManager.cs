using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : AbstractSingleton<MainMenuManager>
{
    public const string TEST_SCENARIO_01 = "Test Scenario 01";

    public void BootTestScenario01()
    {
        StartCoroutine(Main.Instance.LoadGameScene(TEST_SCENARIO_01));
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
