using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
    public static ScenarioManager Singleton;

    [Header("Scenario")]
    public Scenario scenario;

    void Awake()
    {
        if (Singleton != null)
        {
            Debug.LogWarning("Only one instance of ScenarioManager may exist! Deleting this extra one.");
            Destroy(this);
        }
        else
        {
            Singleton = this;
        }
    }

    public void BootScenario(ScenarioFileData data)
    {
        scenario.scenarioName = data.name;
        scenario.scenarioAuthor = data.author;
        scenario.scenarioSize = new Vector2Int(data.width, data.height);

        PlayerManager.Singleton.InstantiatePlayers(data.players);

        MapManager.Singleton.BuildMap(scenario.scenarioSize, data.map);
        //if (data.extraMap != null)
        //{
        //    usesExtraMap = true;
        //    MapManager.Singleton.BuildExtraMap(scenarioSize, data.extraMap);
        //}

        PieceManager.Singleton.CreatePieces(data.pieces);
    }
}
