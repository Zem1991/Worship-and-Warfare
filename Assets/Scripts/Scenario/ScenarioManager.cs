using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
    public static ScenarioManager Singleton;

    [Header("Scenario data")]
    public string scenarioName = "Unnamed Scenario";
    public string scenarioAuthor = "Anonymous Author";

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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BootScenario(ScenarioFileData data)
    {
        scenarioName = data.name;
        scenarioAuthor = data.author;
        MapManager.Singleton.CreateMap(data.map);
        PieceManager.Singleton.CreatePieces(data.pieces);
    }
}
