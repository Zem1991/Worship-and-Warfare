using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public DBCM_Faction factions;

    public DBCM_Class classes;
    public DBCM_Hero heroes;
    public DBCM_Skill skills;

    public DBCM_Animation animations;
    public DBCM_Item items;
    public DBCM_Spell spells;

    public DBCM_Unit units;
    public DBCM_Trait traits;

    public DBCM_Element elements;
    public DBCM_Status statuses;

    public DBCM_Tileset tilesets;
    public DBCM_Battleground battlegrounds;

    void Awake()
    {
        factions = GetComponentInChildren<DBCM_Faction>();
        classes = GetComponentInChildren<DBCM_Class>();
        heroes = GetComponentInChildren<DBCM_Hero>();
        skills = GetComponentInChildren<DBCM_Skill>();
        animations = GetComponentInChildren<DBCM_Animation>();
        items = GetComponentInChildren<DBCM_Item>();
        spells = GetComponentInChildren<DBCM_Spell>();
        units = GetComponentInChildren<DBCM_Unit>();
        traits = GetComponentInChildren<DBCM_Trait>();
        elements = GetComponentInChildren<DBCM_Element>();
        statuses = GetComponentInChildren<DBCM_Status>();
        tilesets = GetComponentInChildren<DBCM_Tileset>();
        battlegrounds = GetComponentInChildren<DBCM_Battleground>();
    }
}
