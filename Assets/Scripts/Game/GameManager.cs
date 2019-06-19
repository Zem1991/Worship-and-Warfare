using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton;

    private TimeSpan timeElapsed = TimeSpan.Zero;

    [Header("Settings")]
    public string scenarioToBoot = "Test Scenario 01";

    [Header("Variables")]
    public bool scenarioBooted;
    public string timeElapsedText;
    public int currentDay;
    public Player currentPlayer;

    void Awake()
    {
        if (Singleton != null)
        {
            Debug.LogWarning("Only one instance of GameManager may exist! Deleting this extra one.");
            Destroy(this);
        }
        else
        {
            Singleton = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        BootScenario();
        StartMatch();
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

    private void BootScenario()
    {
        Debug.Log("Booting scenario: " + scenarioToBoot);
        ScenarioFileData data = ScenarioFileHandler.Load(scenarioToBoot);
        if (data != null)
        {
            ScenarioManager.Singleton.BootScenario(data);
            Debug.Log("Scenario booted.");
        }
        else{
            Debug.LogError("Could not boot the scenario");
        }
    }

    private void StartMatch()
    {
        scenarioBooted = true;
        currentDay = 1;
    }

    //public GameData GetGameData()
    //{
    //    return new GameData(timeElapsed, currentDay, currentPlayer.id);
    //}

    //public void SaveGame()
    //{
    //    ScenarioData scenario = ScenarioManager.Singleton.GetScenarioData();
    //    GameData game = GetGameData();
    //    SaveData data = new SaveData(scenario, game);
    //    SaveLoadHandler.SaveGame(saveFileName, data);
    //}

    //public void LoadGame()
    //{
    //    SaveData data = SaveLoadHandler.LoadGame(saveFileName);
    //    UnloadData();
    //    LoadData(data);
    //}

    //private void UnloadData()
    //{
    //    Debug.Log("Pretend level data was unloaded");
    //}

    //private void LoadData(SaveData data)
    //{
    //    Debug.Log("Pretend level data was loaded");
    //}
}
