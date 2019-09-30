using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : AbstractSingleton<DatabaseManager>
{
    [Header("Database Contents")]
    public DBHandler_Faction factions;
    public DBHandler_Class classes;
    public DBHandler_Hero heroes;
    public DBHandler_Skill skills;
    public DBHandler_Animation animations;
    public DBHandler_Artifact artifacts;
    public DBHandler_Spell spells;
    public DBHandler_Unit units;
    public DBHandler_Ability traits;
    public DBHandler_Element elements;
    public DBHandler_Status statuses;
    public DBHandler_Tileset tilesets;
    public DBHandler_Battleground battlegrounds;

    public override void Awake()
    {
        factions = GetComponentInChildren<DBHandler_Faction>();
        classes = GetComponentInChildren<DBHandler_Class>();
        heroes = GetComponentInChildren<DBHandler_Hero>();
        skills = GetComponentInChildren<DBHandler_Skill>();
        animations = GetComponentInChildren<DBHandler_Animation>();
        artifacts = GetComponentInChildren<DBHandler_Artifact>();
        spells = GetComponentInChildren<DBHandler_Spell>();
        units = GetComponentInChildren<DBHandler_Unit>();
        traits = GetComponentInChildren<DBHandler_Ability>();
        elements = GetComponentInChildren<DBHandler_Element>();
        statuses = GetComponentInChildren<DBHandler_Status>();
        tilesets = GetComponentInChildren<DBHandler_Tileset>();
        battlegrounds = GetComponentInChildren<DBHandler_Battleground>();

        base.Awake();
    }
}
