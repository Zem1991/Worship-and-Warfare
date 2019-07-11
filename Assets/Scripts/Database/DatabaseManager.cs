using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Singleton;

    [Header("Database Contents")]
    public DBHandler_Faction factions;
    public DBHandler_Class classes;
    public DBHandler_Hero heroes;
    public DBHandler_Skill skills;
    public DBHandler_Animation animations;
    public DBHandler_Item items;
    public DBHandler_Spell spells;
    public DBHandler_Unit units;
    public DBHandler_Trait traits;
    public DBHandler_Element elements;
    public DBHandler_Status statuses;
    public DBHandler_Tileset tilesets;
    public DBHandler_Battleground battlegrounds;

    [Header("Database status")]
    public bool isLoaded;

    void Awake()
    {
        if (Singleton != null)
        {
            Debug.LogWarning("Only one instance of DatabaseManager may exist! Deleting this extra one.");
            Destroy(this);
        }
        else
        {
            Singleton = this;
        }

        factions = GetComponentInChildren<DBHandler_Faction>();
        classes = GetComponentInChildren<DBHandler_Class>();
        heroes = GetComponentInChildren<DBHandler_Hero>();
        skills = GetComponentInChildren<DBHandler_Skill>();
        animations = GetComponentInChildren<DBHandler_Animation>();
        items = GetComponentInChildren<DBHandler_Item>();
        spells = GetComponentInChildren<DBHandler_Spell>();
        units = GetComponentInChildren<DBHandler_Unit>();
        traits = GetComponentInChildren<DBHandler_Trait>();
        elements = GetComponentInChildren<DBHandler_Element>();
        statuses = GetComponentInChildren<DBHandler_Status>();
        tilesets = GetComponentInChildren<DBHandler_Tileset>();
        battlegrounds = GetComponentInChildren<DBHandler_Battleground>();

        isLoaded = true;
        Debug.Log("DatabaseManager loaded!");
    }
}
