using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExperienceCalculation
{
    public static int CalculateExperienceToLevel(int targetLevel)
    {
        //TODO base exp value per level, will it be 100 or 1000?
        int expSum = 100;
        int lvlSum = 2;
        while (lvlSum < targetLevel)
        {
            expSum = (int)(expSum * 1.2F);
            lvlSum++;
        }
        return expSum;
    }

    public static int CalculateLevelUps(int currentLevel, int currentExperience)
    {
        int levelUps = 0;
        int expToNext = 0;
        int nextLevel = currentLevel + 1;
        while (true)
        {
            expToNext += CalculateExperienceToLevel(nextLevel);
            if (expToNext <= currentExperience)
            {
                levelUps++;
                nextLevel++;
            }
            else
            {
                break;
            }
        }
        levelUps -= (currentLevel - 1);
        return levelUps;
    }

    public static int FullExperienceCalculation(List<AbstractCombatActorPiece2> pieces)
    {
        //TODO allow experience gain from forcibly winning or losing combat

        int result = 0;
        foreach (var item in pieces)
        {
            CombatantHeroPiece2 asHero = item as CombatantHeroPiece2;
            CombatantUnitPiece2 asUnit = item as CombatantUnitPiece2;

            if (asHero) result += HeroExperience(asHero);
            else if (asUnit) result += UnitExperience(asUnit);
        }
        return result;
    }

    public static int HeroExperience(CombatantHeroPiece2 hero)
    {
        int result = 0;
        if (hero.stateDead)
        {
            result = 150;
            result += hero.GetHero().experienceStats.level * 50;
        }
        return result;
    }

    public static int UnitExperience(CombatantUnitPiece2 unit)
    {
        //int result = 0;
        //TODO combat stats instead of regular stats
        //int stackDif = unit.unit.stackStats.stack_maximum - unit.stackStats.stack;
        //if (stackDif > 0)
        //{
        //    result = unit.unit.dbData.experienceValue;
        //    result *= stackDif;
        //}
        //return result;
        return unit.GetUnit().stackStats.Get() * unit.GetUnit().dbData.experienceValue;
    }
}
