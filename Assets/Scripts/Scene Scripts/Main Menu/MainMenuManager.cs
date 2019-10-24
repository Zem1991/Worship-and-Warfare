using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public const string SCENE_GAME = "Game";
    public const string TEST_SCENARIO_01 = "Test Scenario 01";

    public void BootTestScenario01()
    {
        StartCoroutine(LoadGameScene(TEST_SCENARIO_01));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator LoadGameScene(string scenario)
    {
        Scene scene = SceneManager.GetSceneByName(SCENE_GAME);
        if (scene.handle == 0)
        {
            yield return SceneManager.LoadSceneAsync(SCENE_GAME, LoadSceneMode.Additive);
            Debug.Log("Scene " + SCENE_GAME + " created.");
        }
        else
        {
            Debug.Log("Scene " + SCENE_GAME + " being reused.");
        }

        yield return GameSC.Instance.ConfirmSceneLoaded();
        GameManager.Instance.LoadScenarioFile(scenario);
    }
}
