using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameScheme
{
    FIELD,
    TOWN,
    COMBAT
}

public class GameManager : Singleton<GameManager>
{
    public const string SCENE_DATABASE = "Database";
    public const string SCENE_FIELD = "Game - Field";
    public const string SCENE_TOWN = "Game - Town";
    public const string SCENE_COMBAT = "Game - Combat";

    [Header("Settings")]
    public string scenarioFileToLoad = "Test Scenario 01";

    [Header("Scenes")]
    public Scene sceneDatabase;
    public Scene sceneField;
    public Scene sceneTown;
    public Scene sceneCombat;

    [Header("Variables")]
    public bool scenarioBooted;
    public bool scenarioStarted;
    public int currentDay;
    public Player currentPlayer;
    public string timeElapsedText;
    public GameScheme currentGameScheme;

    private ScenarioFileData scenarioFileData;
    private TimeSpan timeElapsed;

    public Camera mainCamera { get; private set; }

    public override void Awake()
    {
        mainCamera = GetComponentInChildren<Camera>();
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadScenarioFile(null);
    }

    // Update is called once per frame
    void Update()
    {
        if (scenarioBooted && Time.timeScale != 0)
        {
            timeElapsed += TimeSpan.FromSeconds(Time.deltaTime);
            timeElapsedText = timeElapsed.ToString();
        }
    }

    public void LoadScenarioFile(string scenarioFileName)
    {
        Debug.Log("Loading scenario file: " + scenarioFileToLoad);
        scenarioFileData = ScenarioFileHandler.Load(scenarioFileToLoad);

        if (scenarioFileData != null)
        {
            Debug.Log("Scenario file loaded successfully. Initializing...");
            scenarioBooted = false;
            scenarioStarted = false;
            StartCoroutine(InitializeScenario());
        }
        else
        {
            Debug.LogError("Scenario file could not be loaded.");
        }
    }

    public void PerformExchange(Piece sender, Piece receiver)
    {
        Debug.Log("PIECES ARE EXCHANGING STUFF");
    }

    public void GoToTown(Piece piece, Town town)
    {
        Debug.Log("PIECE IS VISITING TOWN");
        ChangeSchemes(GameScheme.TOWN);

        FieldSC.Instance.HideObjects();
        //TownManager.Instance.StartCombat(null, attacker, defender);
        TownSC.Instance.ShowObjects();
    }

    public void ReturnFromTown(Piece piece, Town town)
    {
        Debug.Log("PIECE IS BACK FROM TOWN");
        ChangeSchemes(GameScheme.FIELD);

        TownSC.Instance.HideObjects();
        //TownManager.Instance.StartCombat(null, attacker, defender);
        FieldSC.Instance.ShowObjects();
    }

    public void GoToCombat(Piece attacker, Piece defender)
    {
        Debug.Log("PIECES ARE IN BATTLE");
        ChangeSchemes(GameScheme.COMBAT);

        FieldSC.Instance.HideObjects();
        CombatManager.Instance.StartCombat(null, attacker, defender);
        CombatSC.Instance.ShowObjects();
    }

    public void ReturnFromCombat(Piece attacker, Piece defender)
    {
        Debug.Log("PIECES FINISHED BATTLE");
        ChangeSchemes(GameScheme.FIELD);

        CombatSC.Instance.HideObjects();
        //CombatManager.Instance.StartCombat(null, attacker, defender);
        FieldSC.Instance.ShowObjects();
    }

    private IEnumerator InitializeScenario()
    {
        yield return StartCoroutine(GameSC.Instance.ConfirmSceneLoaded());
        Debug.Log("The Game scene is fully loaded. Loading extra scenes...");

        StartCoroutine(LoadExtraScene(SCENE_DATABASE));
        StartCoroutine(LoadExtraScene(SCENE_FIELD));
        StartCoroutine(LoadExtraScene(SCENE_TOWN));
        StartCoroutine(LoadExtraScene(SCENE_COMBAT));

        yield return StartCoroutine(ConfirmAllScenesLoaded());
        Debug.Log("All required scenes are loaded. Can now boot the scenario.");

        TownSC.Instance.HideObjects();
        CombatSC.Instance.HideObjects();

        yield return StartCoroutine(BootScenario());
        ChangeSchemes(GameScheme.FIELD);
        StartScenario();
    }

    private IEnumerator LoadExtraScene(string name)
    {
        Scene scene = SceneManager.GetSceneByName(name);
        if (scene.handle == 0)
        {
            yield return SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
            Debug.Log("Scene " + name + " created.");
        }
        else
        {
            Debug.Log("Scene " + name + " being reused.");
        }
    }

    private IEnumerator ConfirmAllScenesLoaded()
    {
        while (!(DatabaseSC.Instance && FieldSC.Instance && TownSC.Instance && CombatSC.Instance))
        {
            yield return null;
        }

        yield return DatabaseSC.Instance.ConfirmSceneLoaded();
        yield return FieldSC.Instance.ConfirmSceneLoaded();
        yield return TownSC.Instance.ConfirmSceneLoaded();
        yield return CombatSC.Instance.ConfirmSceneLoaded();
    }

    private IEnumerator BootScenario()
    {
        ScenarioManager.Instance.BootScenario(scenarioFileData);
        scenarioBooted = true;
        Debug.Log("Scenario booted.");
        yield return null;
    }

    private void StartScenario()
    {
        currentDay = 1;
        currentPlayer = PlayerManager.Instance.allPlayers[0];
        timeElapsed = TimeSpan.Zero;

        scenarioStarted = true;
        Debug.Log("Scenario started.");
    }

    private void ChangeSchemes(GameScheme gs)
    {
        InputManager.Instance.ChangeScheme(gs);
        UIManager.Instance.ChangeScheme(gs);

        IInputScheme inputScheme = InputManager.Instance.scheme;
        if (inputScheme != null)
            mainCamera.transform.parent = inputScheme.CameraController().holder.transform;
        else
            mainCamera.transform.parent = transform;
        mainCamera.transform.localPosition = Vector3.zero;
        mainCamera.transform.localRotation = Quaternion.identity;
    }
}
