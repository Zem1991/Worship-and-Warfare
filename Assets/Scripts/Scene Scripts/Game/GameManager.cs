using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : AbstractSingleton<GameManager>
{
    public const float SPEED_MIN = 0.25F;
    public const float SPEED_MAX = 2F;

    [Header("Match initialization")]
    public string scenarioName;
    public bool scenarioBooted;
    public bool scenarioStarted;

    [Header("Match flow")]
    public float gameSpeed;
    public bool isPaused;
    public GameScheme currentGameScheme;
    public string timeElapsedText;
    public string currentDateTime;

    private ScenarioFile scenarioFileData;
    private TimeSpan timeElapsed;

    public Camera mainCamera { get; private set; }

    public override void Awake()
    {
        gameSpeed = Time.timeScale;
        mainCamera = GetComponentInChildren<Camera>();

        base.Awake();
    }

    public IEnumerator Start()
    {
        //if (scenarioFileData == null)
        //{
        //    yield return
        //        StartCoroutine(SceneLoader.Instance.LoadScene_Database());

        //    yield return
        //        StartCoroutine(SceneLoader.Instance.LoadScene_Game());

        //    yield return
        //        LoadScenarioFile();
        //}
        yield return null;
    }

    public void Update()
    {
        if (scenarioBooted && Time.timeScale != 0)
        {
            timeElapsed += TimeSpan.FromSeconds(Time.deltaTime);
            timeElapsedText = timeElapsed.ToString();
        }

        DateTime now = DateTime.Now;
        currentDateTime = now.ToShortDateString() + " " + now.ToShortTimeString();
    }

    public float SetGameSpeed(float speed)
    {
        gameSpeed = Mathf.Clamp(speed, SPEED_MIN, SPEED_MAX);
        if (!isPaused) Time.timeScale = gameSpeed;
        return gameSpeed;
    }

    public bool PauseUnpause()
    {
        if (!isPaused)
        {
            Time.timeScale = 0;
            isPaused = true;
        }
        else
        {
            Time.timeScale = gameSpeed;
            isPaused = false;
        }
        return isPaused;
    }

    public bool PauseUnpause(bool pause)
    {
        isPaused = !pause;
        return PauseUnpause();
    }

    public void ChangeSchemes(GameScheme gs)
    {
        currentGameScheme = gs;
        InputManager.Instance.ChangeScheme(gs);
        UIManager.Instance.ChangeScheme(gs);

        IInputScheme inputScheme = InputManager.Instance.scheme;
        mainCamera.transform.parent = inputScheme != null ? inputScheme.CameraController().holder.transform : transform;
        mainCamera.transform.localPosition = Vector3.zero;
        mainCamera.transform.localRotation = Quaternion.identity;
    }

    public IEnumerator LoadScenarioFile()
    {
        yield return StartCoroutine(LoadScenarioFile(scenarioName));
        PauseUnpause(false);
    }

    public IEnumerator LoadScenarioFile(string scenarioFileName)
    {
        Debug.Log("Loading scenario file: " + scenarioFileName);
        scenarioName = scenarioFileName;
        scenarioFileData = ScenarioFileHandler.Load(scenarioFileName);

        if (scenarioFileData != null)
        {
            GameSC.Instance.ShowScene();

            Debug.Log("Scenario file loaded successfully. Initializing...");
            scenarioBooted = false;
            scenarioStarted = false;

            yield return
                StartCoroutine(StartScenario());

            FieldSC.Instance.ShowScene();

        }
        else
        {
            Debug.LogError("Scenario file could not be loaded.");
        }
    }

    private IEnumerator StartScenario()
    {
        yield return
                StartCoroutine(SceneLoader.Instance.LoadScene_Field());

        yield return
                StartCoroutine(SceneLoader.Instance.LoadScene_Combat());

        yield return
                StartCoroutine(SceneLoader.Instance.LoadScene_Town());

        timeElapsed = TimeSpan.Zero;
        ChangeSchemes(GameScheme.FIELD);

        ScenarioManager.Instance.StartScenario(scenarioFileData);
        scenarioBooted = true;

        FieldManager.Instance.currentTurn = 0;
        FieldManager.Instance.NextTurnForAll();
        scenarioStarted = true;
    }
}
