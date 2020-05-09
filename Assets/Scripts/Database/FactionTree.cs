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
    public DB_HeroUnit heroClassMight;
    public DB_HeroUnit heroClassMagic;
    public DB_HeroUnit heroClassTech;

    [Header("Units")]
    public DB_CombatUnit unitTier1;
    public DB_CombatUnit unitTier2;
    public DB_CombatUnit unitTier3;
    public DB_CombatUnit unitTier4;
    public DB_CombatUnit unitTier5;
    public DB_CombatUnit unitTier6;
    public DB_CombatUnit unitTier7;

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

    public List<DB_HeroUnit> GetHeroClasses()
    {
        List<DB_HeroUnit> result = new List<DB_HeroUnit>();
        if (heroClassMight) result.Add(heroClassMight);
        if (heroClassMagic) result.Add(heroClassMagic);
        if (heroClassTech) result.Add(heroClassTech);
        return result;
    }

    public List<DB_HeroPerson> GetHeroes(DB_HeroUnit heroClass)
    {
        List<DB_HeroPerson> result = new List<DB_HeroPerson>();
        List<DB_HeroPerson> heroes = DBHandler_HeroPerson.Instance.SelectAll();
        foreach (DB_HeroPerson hero in heroes)
        {
            if (hero.heroClass == heroClass) result.Add(hero);
        }
        return result;
    }

    public List<DB_CombatUnit> GetUnits()
    {
        List<DB_CombatUnit> result = new List<DB_CombatUnit>();
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
