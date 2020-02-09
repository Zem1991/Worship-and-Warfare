using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : MonoBehaviour
{
    public string townName;
    public List<TownBuilding> buildings;

    [Header("Database reference")]
    public DB_Faction dbFaction;

    public void Initialize(DB_Faction dbFaction, string townName = null)
    {
        string selectedName = townName != null ? townName : dbFaction.townNames[0];     //TODO get random name

        this.dbFaction = dbFaction;
        this.townName = selectedName;
        name = townName;
    }
}
