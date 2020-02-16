using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionTree : MonoBehaviour
{
    [Header("Buildings")]
    public DB_TownBuilding villageHall;
    public DB_TownBuilding tavern;
    public DB_TownBuilding temple;
    public DB_TownBuilding castle;
    public DB_TownBuilding mageGuild;
    public DB_TownBuilding workshop;

    [Header("Hero classes")]
    public DB_Class heroClassMight;
    public DB_Class heroClassMagic;
    public DB_Class heroClassTech;

    [Header("Units")]
    public DB_Unit unitTier1;
    public DB_Unit unitTier2;
    public DB_Unit unitTier3;
    public DB_Unit unitTier4;
    public DB_Unit unitTier5;
    public DB_Unit unitTier6;
    public DB_Unit unitTier7;

    public List<DB_TownBuilding> GetBuildings()
    {
        List<DB_TownBuilding> result = new List<DB_TownBuilding>();
        if (villageHall) result.Add(villageHall);
        if (tavern) result.Add(tavern);
        if (temple) result.Add(temple);
        if (castle) result.Add(castle);
        if (mageGuild) result.Add(mageGuild);
        if (workshop) result.Add(workshop);
        return result;
    }

    public List<DB_Class> GetHeroClasses()
    {
        List<DB_Class> result = new List<DB_Class>();
        if (heroClassMight) result.Add(heroClassMight);
        if (heroClassMagic) result.Add(heroClassMagic);
        if (heroClassTech) result.Add(heroClassTech);
        return result;
    }

    public List<DB_Hero> GetHeroes(DB_Class heroClass)
    {
        List<DB_Hero> result = new List<DB_Hero>();
        List<DB_Hero> heroes = DBHandler_Hero.Instance.SelectAll();
        foreach (DB_Hero hero in heroes)
        {
            if (hero.classs == heroClass) result.Add(hero);
        }
        return result;
    }

    public List<DB_Unit> GetUnits()
    {
        List<DB_Unit> result = new List<DB_Unit>();
        if (unitTier1) result.Add(unitTier1);
        if (unitTier2) result.Add(unitTier2);
        if (unitTier3) result.Add(unitTier3);
        if (unitTier4) result.Add(unitTier4);
        if (unitTier5) result.Add(unitTier5);
        if (unitTier6) result.Add(unitTier6);
        if (unitTier7) result.Add(unitTier7);
        return result;
    }
}
