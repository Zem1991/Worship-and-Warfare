using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : AbstractSingleton<Main>, ISceneController
{
    [Header("Local singletons")]
    [SerializeField] private MainMenuManager mainMenuManager;
    [SerializeField] private MainMenuUI mainMenuUI;
    [SerializeField] private SceneLoader sceneLoader;

    public override void Awake()
    {
        mainMenuManager = FindObjectOfType<MainMenuManager>();
        mainMenuUI = FindObjectOfType<MainMenuUI>();
        sceneLoader = FindObjectOfType<SceneLoader>();

        base.Awake();
    }

    public IEnumerator Start()
    {
        yield return
            StartCoroutine(sceneLoader.LoadScene_Main());

        yield return
            StartCoroutine(sceneLoader.LoadScene_Database());

        ShowScene();
    }

    public IEnumerator WaitForSceneLoad()
    {
        yield return
            MainMenuManager.CheckReference(mainMenuManager) &&
            MainMenuUI.CheckReference(mainMenuUI);
            SceneLoader.CheckReference(sceneLoader);
    }

    public void HideScene()
    {
        mainMenuUI.HideUI();
    }

    public void ShowScene()
    {
        mainMenuUI.ShowUI();
    }

    public IEnumerator LoadGameScene(string scenario)
    {
        HideScene();

        yield return
            StartCoroutine(sceneLoader.LoadScene_Game());

        yield return
            StartCoroutine(GameManager.Instance.LoadScenarioFile(scenario));
    }

    public void ReturnToMain()
    {
        GameManager.Instance.PauseUnpause(false);

        //yield return
        //    StartCoroutine(sceneLoader.LoadScene_Main());

        ShowScene();
    }
}
