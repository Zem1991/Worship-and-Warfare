using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
    public static ScenarioManager Singleton;
    public readonly Vector2Int MIN_SIZE = Vector2Int.one * 10;
    public readonly Vector2Int MAX_SIZE = Vector2Int.one * 80;

    [Header("Scenario")]
    public Scenario scenario;

    private Bounds bounds;

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

        Vector3 boundsSize = new Vector3(scenario.scenarioSize.x, 0, scenario.scenarioSize.y);
        bounds = new Bounds(boundsSize / 2F, boundsSize);
    }

    public bool IsWithinBounds(Vector3 pos)
    {
        return bounds.Contains(pos);
    }

    //public Vector3 ClampToBounds(Vector3 pos)
    //{
    //    return bounds.ClosestPoint(pos);
    //}
}
