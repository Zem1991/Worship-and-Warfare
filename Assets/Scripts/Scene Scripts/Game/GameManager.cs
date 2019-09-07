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

public class GameManager : AbstractSingleton<GameManager>
{
    public const string SCENE_DATABASE = "Database";
    public const string SCENE_FIELD = "Game - Field";
    public const string SCENE_TOWN = "Game - Town";
    public const string SCENE_COMBAT = "Game - Combat";

    public const float SPEED_MIN = 0.25F;
    public const float SPEED_MAX = 2F;

    [Header("Settings")]
    public string scenarioFileToLoad = "Test Scenario 01";

    [Header("Scenes")]
    public Scene sceneDatabase;
    public Scene sceneField;
    public Scene sceneTown;
    public Scene sceneCombat;

    [Header("Match initialization")]
    public bool scenarioBooted;
    public bool scenarioStarted;

    [Header("Match flow")]
    public float gameSpeed;
    public bool isPaused;
    public int currentTurn;
    public Player currentPlayer;
    public GameScheme currentGameScheme;
    public string timeElapsedText;

    [Header("Day/Week/Month")]
    public int day;
    public int week;
    public int month;

    private ScenarioFileData scenarioFileData;
    private TimeSpan timeElapsed;

    public Camera mainCamera { get; private set; }

    public override void Awake()
    {
        gameSpeed = Time.timeScale;
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

    public void PerformExchange(FieldPiece sender, FieldPiece receiver)
    {
        Debug.Log("PIECES ARE EXCHANGING STUFF");
    }

    public void GoToTown(FieldPiece piece, Town town)
    {
        Debug.Log("PIECE IS VISITING TOWN");
        ChangeSchemes(GameScheme.TOWN);

        FieldSC.Instance.HideObjects();
        //TownManager.Instance.StartCombat(null, attacker, defender);
        TownSC.Instance.ShowObjects();
    }

    public void ReturnFromTown(FieldPiece piece, Town town)
    {
        Debug.Log("PIECE IS BACK FROM TOWN");
        ChangeSchemes(GameScheme.FIELD);

        TownSC.Instance.HideObjects();
        //TownManager.Instance.StartCombat(null, attacker, defender);
        FieldSC.Instance.ShowObjects();
    }

    public void GoToCombat(FieldPiece attacker, FieldPiece defender)
    {
        Debug.Log("PIECES ARE IN BATTLE");
        ChangeSchemes(GameScheme.COMBAT);

        FieldSC.Instance.HideObjects();
        FieldTile fieldTile = defender.currentTile as FieldTile;
        CombatManager.Instance.BootCombat(attacker, defender, fieldTile.db_tileset_lowerLand);
        CombatSC.Instance.ShowObjects();
    }

    public void ReturnFromCombat(CombatResult result, FieldPiece attacker, FieldPiece defender)
    {
        Debug.Log("PIECES FINISHED BATTLE");
        ChangeSchemes(GameScheme.FIELD);

        CombatSC.Instance.HideObjects();
        CombatManager.Instance.TerminateCombat();

        FieldSC.Instance.ShowObjects();
        switch (result)
        {
            case CombatResult.ATTACKER_WON:
                FieldManager.Instance.RemovePiece(defender);
                break;
            case CombatResult.DEFENDER_WON:
                FieldManager.Instance.RemovePiece(attacker);
                break;
        }
    }

    public void EndTurnForCurrentPlayer()
    {
        Player next = PlayerManager.Instance.EndTurnForPlayer(currentPlayer);
        if (!next) NextTurnForAll();
        else currentPlayer = next;
    }

    private void NextTurnForAll()
    {
        currentTurn++;
        PlayerManager.Instance.RefreshTurnForActivePlayers(currentTurn);
        currentPlayer = PlayerManager.Instance.activePlayers[0];

        NextDayWeekMonth();
    }

    private void NextDayWeekMonth()
    {
        int turnsAdjusted = currentTurn - 1;
        month = (turnsAdjusted / 28) + 1;
        int monthDayDif = turnsAdjusted % 28;
        week = (monthDayDif / 7) + 1;
        int weekDayDif = monthDayDif % 7;
        day = (weekDayDif % 7) + 1;
    }

    public void Restart()
    {
        FieldManager.Instance.TerminateField();
        LoadScenarioFile(null);
    }

    public void LoadScenarioFile(string scenarioFileName)
    {
        Debug.LogWarning("NOT READING ANY SCENARIO FILE BY NAME!");
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
        currentTurn = 0;
        NextTurnForAll();
        timeElapsed = TimeSpan.Zero;

        scenarioStarted = true;
        Debug.Log("Scenario started.");
    }

    private void ChangeSchemes(GameScheme gs)
    {
        currentGameScheme = gs;
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
