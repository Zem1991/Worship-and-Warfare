using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioManager : AbstractSingleton<ScenarioManager>
{
    public readonly Vector2Int MIN_SIZE = Vector2Int.one * 10;
    public readonly Vector2Int MAX_SIZE = Vector2Int.one * 80;

    [Header("Scenario")]
    public Scenario scenario;

    private Bounds scenarioBounds;

    public void StartScenario(ScenarioFile data)
    {
        scenario.scenarioName = data.scenarioData.name;
        scenario.scenarioAuthor = data.scenarioData.author;
        scenario.scenarioSize = new Vector2Int(data.scenarioData.width, data.scenarioData.height);

        PlayerManager.Instance.InstantiatePlayers(data.players);
        PlayerManager.Instance.RunAIPlayers();

        FieldManager.Instance.BootField(scenario.scenarioSize, data.map, data.towns, data.parties, data.pickups);
        //if (data.extraMap != null)
        //{
        //    usesExtraMap = true;
        //    MapManager.Singleton.BuildExtraMap(scenarioSize, data.extraMap);
        //}

        Vector3 boundsSize = new Vector3(scenario.scenarioSize.x, 0, scenario.scenarioSize.y);
        scenarioBounds = new Bounds(boundsSize / 2F, boundsSize);
    }

    public bool IsWithinBounds(Vector3 pos)
    {
        return scenarioBounds.Contains(pos);
    }

    //public Vector3 ClampToBounds(Vector3 pos)
    //{
    //    return bounds.ClosestPoint(pos);
    //}
}
