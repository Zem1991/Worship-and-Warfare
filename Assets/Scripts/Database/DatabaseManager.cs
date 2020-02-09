using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : AbstractSingleton<DatabaseManager>
{
    [Header("Database Contents")]
    [SerializeField] private DBHandler_Color colors;
    [SerializeField] private DBHandler_Artifact artifacts;
    [SerializeField] private DBHandler_Spell spells;
    [SerializeField] private DBHandler_Tech techs;
    [SerializeField] private DBHandler_Skill skills;
    [SerializeField] private DBHandler_Ability abilities;
    [SerializeField] private DBHandler_Faction factions;
    [SerializeField] private DBHandler_TownBuilding townBuildings;
    [SerializeField] private DBHandler_Class classes;
    [SerializeField] private DBHandler_Hero heroes;
    [SerializeField] private DBHandler_Unit units;
    [SerializeField] private DBHandler_Element elements;
    [SerializeField] private DBHandler_Status statuses;
    [SerializeField] private DBHandler_Animation animations;
    [SerializeField] private DBHandler_Tileset tilesets;
    [SerializeField] private DBHandler_Battleground battlegrounds;
    [SerializeField] private DBHandler_CombatObstacle combatObstacles;

    public override void Awake()
    {
        colors = GetComponentInChildren<DBHandler_Color>();
        artifacts = GetComponentInChildren<DBHandler_Artifact>();
        spells = GetComponentInChildren<DBHandler_Spell>();
        techs = GetComponentInChildren<DBHandler_Tech>();
        skills = GetComponentInChildren<DBHandler_Skill>();
        abilities = GetComponentInChildren<DBHandler_Ability>();
        factions = GetComponentInChildren<DBHandler_Faction>();
        townBuildings = GetComponentInChildren<DBHandler_TownBuilding>();
        classes = GetComponentInChildren<DBHandler_Class>();
        heroes = GetComponentInChildren<DBHandler_Hero>();
        units = GetComponentInChildren<DBHandler_Unit>();
        elements = GetComponentInChildren<DBHandler_Element>();
        statuses = GetComponentInChildren<DBHandler_Status>();
        animations = GetComponentInChildren<DBHandler_Animation>();
        tilesets = GetComponentInChildren<DBHandler_Tileset>();
        battlegrounds = GetComponentInChildren<DBHandler_Battleground>();
        combatObstacles = GetComponentInChildren<DBHandler_CombatObstacle>();

        base.Awake();
    }
}
