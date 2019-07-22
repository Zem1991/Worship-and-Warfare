using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager1 : Singleton<GameManager1>
{
    public const string SCENE_DATABASE = "Database";
    public const string SCENE_FIELD = "Game - Field";
    public const string SCENE_COMBAT = "Game - Combat";
    public const string SCENE_TOWN = "Game - Town";

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

    private WaitForEndOfFrame waitEOF;
    private ScenarioFileData scenarioFileData;
    private TimeSpan timeElapsed;

    public override void Awake()
    {
        waitEOF = new WaitForEndOfFrame();
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

    public void PerformExchange(Piece sender, Piece receiver)
    {
        Debug.Log("PIECES ARE EXCHANGING STUFF");
    }

    public void GoToCombat(Piece attacker, Piece defender)
    {
        Debug.Log("PIECES ARE IN BATTLE");
        StartCoroutine(LoadCombatScene(attacker, defender));
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

    private IEnumerator InitializeScenario()
    {
        yield return StartCoroutine(VerifyDatabaseScene());
        yield return StartCoroutine(VerifyFieldScene());
        Debug.Log("All required scenes are loaded. Can now boot the scenario.");

        yield return StartCoroutine(BootScenario());
        StartScenario();
    }

    private IEnumerator VerifyDatabaseScene()
    {
        sceneDatabase = SceneManager.GetSceneByName(SCENE_DATABASE);
        if (sceneDatabase.handle == 0)
        {
            yield return SceneManager.LoadSceneAsync(SCENE_DATABASE, LoadSceneMode.Additive);
            sceneDatabase = SceneManager.GetSceneByName(SCENE_DATABASE);
            Debug.Log("Scene " + SCENE_DATABASE + " created.");
        }
        else
        {
            Debug.Log("Scene " + SCENE_DATABASE + " being reused.");
        }
    }

    private IEnumerator VerifyFieldScene()
    {
        sceneField = SceneManager.GetSceneByName(SCENE_FIELD);
        if (sceneField.handle == 0)
        {
            yield return SceneManager.LoadSceneAsync(SCENE_FIELD, LoadSceneMode.Additive);
            sceneField = SceneManager.GetSceneByName(SCENE_FIELD);
            Debug.Log("Scene " + SCENE_FIELD + " created.");
        }
        else
        {
            Debug.Log("Scene " + SCENE_FIELD + " being reused.");
        }
    }

    private IEnumerator BootScenario()
    {
        ScenarioManager.Instance.BootScenario(scenarioFileData);
        scenarioBooted = true;
        Debug.Log("Scenario booted.");
        yield return waitEOF;
    }

    private void StartScenario()
    {
        currentDay = 1;
        currentPlayer = PlayerManager.Instance.allPlayers[0];
        timeElapsed = TimeSpan.Zero;

        scenarioStarted = true;
        Debug.Log("Scenario started.");
    }

    //          TODO:   DO THIS ONE LATER
    //private IEnumerator VerifyTownScene()
    //{
    //    sceneTown = SceneManager.GetSceneByName(SCENE_TOWN);
    //    if (sceneTown.handle == 0)
    //    {
    //        yield return SceneManager.LoadSceneAsync(SCENE_TOWN, LoadSceneMode.Additive);
    //        sceneTown = SceneManager.GetSceneByName(SCENE_TOWN);
    //        Debug.Log("Scene " + SCENE_TOWN + " created.");
    //    }
    //    else
    //    {
    //        Debug.Log("Scene " + SCENE_TOWN + " being reused.");
    //    }
    //}

    private IEnumerator LoadCombatScene(Piece attacker, Piece defender)
    {
        yield return SceneManager.LoadSceneAsync(SCENE_COMBAT, LoadSceneMode.Additive);
        sceneCombat = SceneManager.GetSceneByName(SCENE_COMBAT);
        Debug.Log("Scene " + SCENE_COMBAT + " created.");

        MapManager.Instance.gameObject.SetActive(false);
        PieceManager.Instance.gameObject.SetActive(false);
        InputManager.Instance.scheme = CombatInputs.Instance;

        DB_Battleground battleground = DatabaseManager.Instance.battlegrounds.defaultContent as DB_Battleground;
        CombatManager.Instance.StartCombat(battleground, attacker, defender);
    }
}
