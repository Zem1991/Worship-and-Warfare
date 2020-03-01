﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : MonoBehaviour
{
    public string townName;
    public TownPiece2 townPiece;

    [Header("Garrison")]
    public Party garrison;
    public Party visitor;

    [Header("Buildings")]
    public TownBuilding townCenter;
    public TownBuilding tavern;
    public TownBuilding temple;
    public TownBuilding castle;
    public TownBuilding mageGuild;
    public TownBuilding workshop;
    //public List<TownBuilding> buildings;

    [Header("Database reference")]
    public DB_Faction dbFaction;

    public void Initialize(DB_Faction dbFaction, TownPiece2 townPiece, string townName = null)
    {
        string selectedName = townName != null ? townName : dbFaction.townNames[0];     //TODO get random name

        this.townPiece = townPiece;

        this.dbFaction = dbFaction;
        this.townName = selectedName;
        name = townName;
    }

    public List<TownBuilding> GetBuildings()
    {
        List<TownBuilding> result = new List<TownBuilding>();
        if (townCenter) result.Add(townCenter);
        if (tavern) result.Add(tavern);
        if (temple) result.Add(temple);
        if (castle) result.Add(castle);
        if (mageGuild) result.Add(mageGuild);
        if (workshop) result.Add(workshop);
        return result;
    }

    public TownBuilding BuildStructure(DB_TownBuilding dbTownBuilding, bool forFree = false)
    {
        if (!forFree)
        {
            Player owner = townPiece.pieceOwner.GetOwner();
            Dictionary<ResourceStats, int> costs = dbTownBuilding.resourceStats.GetCosts(1);
            owner.resourceStats.Subtract(costs);
        }

        TownBuilding prefab = AllPrefabs.Instance.townBuilding;
        TownBuilding newTB = Instantiate(prefab, transform);
        newTB.Initialize(dbTownBuilding);

        switch (dbTownBuilding.townBuildingType)
        {
            case TownBuildingType.TOWN_CENTER:
                townCenter = newTB;
                break;
            case TownBuildingType.TAVERN:
                tavern = newTB;
                break;
            case TownBuildingType.TEMPLE:
                temple = newTB;
                break;
            case TownBuildingType.CASTLE:
                castle = newTB;
                break;
            case TownBuildingType.MAGE_GUILD:
                mageGuild = newTB;
                break;
            case TownBuildingType.WORKSHOP:
                workshop = newTB;
                break;
            default:
                Debug.LogWarning("Unknown building type!");
                break;
        }
        return newTB;
    }
}
