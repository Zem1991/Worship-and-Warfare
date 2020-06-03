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

    public static int FullExperienceCalculation(List<CombatantPiece3> pieces)
    {
        int result = 0;
        foreach (var item in pieces)
        {
            HeroUnitPiece3 asHero = item as HeroUnitPiece3;
            CombatUnitPiece3 asUnit = item as CombatUnitPiece3;

            if (asHero) result += HeroExperience(asHero);
            else if (asUnit) result += UnitExperience(asUnit);
        }
        return result;
    }

    public static int HeroExperience(HeroUnitPiece3 hero)
    {
        int result = 0;
        if (hero.stateDead)
        {
            //TODO: function to get proper HeroUnit exp value
            result = 150;
            result += hero.GetHeroUnit().levelUpStats.level * 50;
        }
        return result;
    }

    public static int UnitExperience(CombatUnitPiece3 unit)
    {
        //TODO: function to get proper CombatUnit exp value
        CombatUnit combatUnit = unit.GetCombatUnit();
        int stackSize = combatUnit.GetStackHealthStats().GetStackSize();
        int xpPerUnit = combatUnit.GetDBCombatUnit().experienceValue;
        return stackSize * xpPerUnit;
    }
}
