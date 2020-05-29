using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : MonoBehaviour
{
    [Header("Town data")]
    public string townName;

    [Header("Object components")]
    [SerializeField] private Party garrison;

    [Header("Object components")]
    public ResourceStats2 dailyIncome;

    [Header("Buildings")]
    public TownBuilding townCenter;
    public TownBuilding tavern;
    public TownBuilding temple;
    public TownBuilding castle;
    public TownBuilding mageGuild;
    public TownBuilding workshop;

    [Header("Defenses")]
    public TownDefense wall;
    public TownDefense gatehouse;
    public TownDefense tower;
    public TownDefense moat;

    [Header("Database reference")]
    public DB_Faction dbFaction;

    public void Initialize(DB_Faction dbFaction, TownData townData)
    {
        string selectedName = townData.townName != null ? townName : dbFaction.townNames[0];     //TODO get random name

        this.dbFaction = dbFaction;
        townName = selectedName;
        name = townName;

        garrison.Initialize(townData.garrison);
    }

    public Party GetGarrison()
    {
        return garrison;
    }

    public List<AbstractTownStructure> GetStructures()
    {
        List<AbstractTownStructure> result = new List<AbstractTownStructure>();
        if (townCenter) result.Add(townCenter);
        if (tavern) result.Add(tavern);
        if (temple) result.Add(temple);
        if (castle) result.Add(castle);
        if (mageGuild) result.Add(mageGuild);
        if (workshop) result.Add(workshop);
        return result;
    }

    public AbstractTownStructure BuildStructure(DB_TownStructure dbTownStructure, Player owner)
    {
        if (owner)
        {
            Dictionary<ResourceStats2, int> costs = dbTownStructure.resourceStats.GetCosts(1);
            owner.currentResources.Subtract(costs);
        }

        DB_TownBuilding dbTownBuilding = dbTownStructure as DB_TownBuilding;
        //TODO BUILD DEFENSE METHOD CALL
        if (dbTownBuilding) return BuildBuilding(dbTownBuilding);
        return null;
    }

    private TownBuilding BuildBuilding(DB_TownBuilding dbTownBuilding)
    {
        TownBuilding prefab = AllPrefabs.Instance.townBuilding;
        TownBuilding newTB = Instantiate(prefab, transform);
        newTB.Initialize(dbTownBuilding);

        switch (dbTownBuilding.buildingType)
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
