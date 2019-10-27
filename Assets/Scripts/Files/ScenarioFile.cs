using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScenarioFile
{
    public ScenarioData scenarioData;
    public List<PlayerData> players;

    public MapData map;
    //public MapData extraMap;
    public List<PartyData> parties;
    public List<PickupData> pickups;
}
