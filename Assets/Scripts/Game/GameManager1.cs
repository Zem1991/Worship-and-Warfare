//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class GameManager : AbstractSingleton<GameManager>
//{
//    public const float SPEED_MIN = 0.25F;
//    public const float SPEED_MAX = 2F;

//    [Header("Match initialization")]
//    public string scenarioName;
//    public bool scenarioBooted;
//    public bool scenarioStarted;

//    [Header("Match flow")]
//    public float gameSpeed;
//    public bool isPaused;
//    public int currentTurn;
//    public Player currentPlayer;
//    public GameScheme currentGameScheme;
//    public string timeElapsedText;
//    public string currentDateTime;

//    [Header("Day/Week/Month")]
//    public int day;
//    public int week;
//    public int month;

//    private ScenarioFile scenarioFileData;
//    private TimeSpan timeElapsed;

//    public Camera mainCamera { get; private set; }

//    public override void Awake()
//    {
//        gameSpeed = Time.timeScale;
//        mainCamera = GetComponentInChildren<Camera>();

//        base.Awake();
//    }

//    public IEnumerator Start()
//    {
//        if (scenarioFileData == null)
//        {
//            yield return
//                StartCoroutine(SceneLoader.Instance.LoadScene_Database());

//            yield return
//                StartCoroutine(SceneLoader.Instance.LoadScene_Game());

//            LoadScenarioFile(scenarioName);
//        }
//    }

//    public void Update()
//    {
//        if (scenarioBooted && Time.timeScale != 0)
//        {
//            timeElapsed += TimeSpan.FromSeconds(Time.deltaTime);
//            timeElapsedText = timeElapsed.ToString();
//        }

//        DateTime now = DateTime.Now;
//        currentDateTime = now.ToShortDateString() + " " + now.ToShortTimeString();
//    }

//    public float SetGameSpeed(float speed)
//    {
//        gameSpeed = Mathf.Clamp(speed, SPEED_MIN, SPEED_MAX);
//        if (!isPaused) Time.timeScale = gameSpeed;
//        return gameSpeed;
//    }

//    public bool PauseUnpause()
//    {
//        if (!isPaused)
//        {
//            Time.timeScale = 0;
//            isPaused = true;
//        }
//        else
//        {
//            Time.timeScale = gameSpeed;
//            isPaused = false;
//        }
//        return isPaused;
//    }

//    public bool PauseUnpause(bool pause)
//    {
//        isPaused = !pause;
//        return PauseUnpause();
//    }

//    public void PerformExchange(AbstractFieldPiece2 sender, AbstractFieldPiece2 receiver)
//    {
//        Debug.Log("PIECES ARE EXCHANGING STUFF");
//        //yield return null;
//    }

//    public void GoToTown(AbstractFieldPiece2 piece, Town town)
//    {
//        Debug.Log("PIECE IS VISITING TOWN");
//        ChangeSchemes(GameScheme.TOWN);

//        FieldSC.Instance.HideScene();
//        //TownManager.Instance.StartCombat(null, attacker, defender);
//        TownSC.Instance.ShowScene();
//    }

//    public void ReturnFromTown(AbstractFieldPiece2 piece, Town town)
//    {
//        Debug.Log("PIECE IS BACK FROM TOWN");
//        ChangeSchemes(GameScheme.FIELD);

//        TownSC.Instance.HideScene();
//        //TownManager.Instance.StartCombat(null, attacker, defender);
//        FieldSC.Instance.ShowScene();
//    }

//    public void GoToCombat(PartyPiece2 attacker, PartyPiece2 defender)
//    {
//        Debug.Log("PIECES ARE IN BATTLE");
//        ChangeSchemes(GameScheme.COMBAT);

//        FieldSC.Instance.HideScene();
//        FieldTile fieldTile = defender.currentTile as FieldTile;

//        CombatManager.Instance.BootCombat(attacker, defender, fieldTile.db_tileset_lowerLand);

//        CombatSC.Instance.ShowScene();
//    }

//    public void ReturnFromCombat(CombatResult result, PartyPiece2 attacker, PartyPiece2 defender)
//    {
//        CombatSC.Instance.HideScene();

//        //Doing this here, because later I could add an 'redo combat' feature.
//        CombatManager.Instance.ApplyCombatResults(out int attackerExperience, out int defenderExperience);

//        ChangeSchemes(GameScheme.FIELD);

//        FieldSC.Instance.ShowScene();
//        switch (result)
//        {
//            case CombatResult.ATTACKER_WON:
//                attacker.ApplyExperience(attackerExperience);
//                FieldManager.Instance.RemovePiece(defender);
//                break;
//            case CombatResult.DEFENDER_WON:
//                defender.ApplyExperience(defenderExperience);
//                FieldManager.Instance.RemovePiece(attacker);
//                break;
//        }
//    }

//    public void EndTurn()
//    {
//        FieldUI.Instance.timers.LockButtons();
//        StartCoroutine(EndTurnForCurrentPlayer());
//    }

//    private IEnumerator EndTurnForCurrentPlayer()
//    {
//        PlayerManager pm = PlayerManager.Instance;
//        FieldPieceHandler fPH = FieldManager.Instance.pieceHandler;
//        FieldInputs fInputs = FieldInputs.Instance;

//        List<PartyPiece2> playerFieldPieces = fPH.GetPlayerPieces(currentPlayer);
//        yield return StartCoroutine(fPH.YieldForIdlePieces(playerFieldPieces));

//        Player next = pm.EndTurnForPlayer(currentPlayer);
//        if (!next) NextTurnForAll();
//        else currentPlayer = next;

//        if (currentPlayer == pm.localPlayer)
//        {
//            FieldUI.Instance.timers.UnlockButtons();
//            fInputs.ResetHighlights();
//        }
//        else if (currentPlayer.type == PlayerType.COMPUTER)
//        {
//            currentPlayer.aiPersonality.FieldRoutine();
//        }
//    }

//    private void NextTurnForAll()
//    {
//        currentTurn++;
//        PlayerManager.Instance.RefreshTurnForActivePlayers(currentTurn);
//        currentPlayer = PlayerManager.Instance.activePlayers[0];

//        FieldManager.Instance.NextTurnForAll();

//        NextDayWeekMonth();
//    }

//    private void NextDayWeekMonth()
//    {
//        int turnsAdjusted = currentTurn - 1;
//        month = (turnsAdjusted / 28) + 1;
//        int monthDayDif = turnsAdjusted % 28;
//        week = (monthDayDif / 7) + 1;
//        int weekDayDif = monthDayDif % 7;
//        day = (weekDayDif % 7) + 1;
//    }

//    public void Restart()
//    {
//        FieldManager.Instance.TerminateField();
//        LoadScenarioFile(scenarioName);
//    }

//    public void LoadScenarioFile(string scenarioFileName)
//    {
//        Debug.Log("Loading scenario file: " + scenarioFileName);
//        scenarioName = scenarioFileName;
//        scenarioFileData = ScenarioFileHandler.Load(scenarioFileName);

//        if (scenarioFileData != null)
//        {
//            GameSC.Instance.ShowScene();

//            Debug.Log("Scenario file loaded successfully. Initializing...");
//            scenarioBooted = false;
//            scenarioStarted = false;
//            StartCoroutine(InitializeScenario());
//        }
//        else
//        {
//            Debug.LogError("Scenario file could not be loaded.");
//        }
//    }

//    private IEnumerator InitializeScenario()
//    {
//        yield return
//                StartCoroutine(SceneLoader.Instance.LoadScene_Field());

//        yield return
//                StartCoroutine(SceneLoader.Instance.LoadScene_Combat());

//        yield return
//                StartCoroutine(SceneLoader.Instance.LoadScene_Town());

//        FieldSC.Instance.ShowScene();
//        ChangeSchemes(GameScheme.FIELD);

//        BootScenario();
//        StartScenario();
//    }

//    private void BootScenario()
//    {
//        ScenarioManager.Instance.BootScenario(scenarioFileData);
//        scenarioBooted = true;
//    }

//    private void StartScenario()
//    {
//        currentTurn = 0;
//        NextTurnForAll();
//        timeElapsed = TimeSpan.Zero;

//        PlayerManager.Instance.RunAIPlayers();
//        scenarioStarted = true;
//    }

//    private void ChangeSchemes(GameScheme gs)
//    {
//        currentGameScheme = gs;
//        InputManager.Instance.ChangeScheme(gs);
//        UIManager.Instance.ChangeScheme(gs);

//        IInputScheme inputScheme = InputManager.Instance.scheme;
//        if (inputScheme != null)
//            mainCamera.transform.parent = inputScheme.CameraController().holder.transform;
//        else
//            mainCamera.transform.parent = transform;
//        mainCamera.transform.localPosition = Vector3.zero;
//        mainCamera.transform.localRotation = Quaternion.identity;
//    }
//}
