using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : AbstractSingleton<DatabaseManager>
{
    [Header("Database Contents")]
    [SerializeField] private DBHandler_Color colors;
    [SerializeField] private DBHandler_Faction factions;
    [SerializeField] private DBHandler_Class classes;
    [SerializeField] private DBHandler_Hero heroes;
    [SerializeField] private DBHandler_Skill skills;
    [SerializeField] private DBHandler_Animation animations;
    [SerializeField] private DBHandler_Artifact artifacts;
    [SerializeField] private DBHandler_Spell spells;
    [SerializeField] private DBHandler_Unit units;
    [SerializeField] private DBHandler_Ability traits;
    [SerializeField] private DBHandler_Element elements;
    [SerializeField] private DBHandler_Status statuses;
    [SerializeField] private DBHandler_Tileset tilesets;
    [SerializeField] private DBHandler_Battleground battlegrounds;

    public override void Awake()
    {
        colors = GetComponentInChildren<DBHandler_Color>();
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
