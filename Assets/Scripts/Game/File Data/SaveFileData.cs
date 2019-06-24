using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveFileData
{
    public ScenarioFileData scenario;
    public GameData game;

    public SaveFileData(ScenarioFileData scenario, GameData game)
    {
        this.scenario = scenario;
        this.game = game;
    }
}
