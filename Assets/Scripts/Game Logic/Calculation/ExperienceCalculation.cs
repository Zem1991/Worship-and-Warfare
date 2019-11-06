using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExperienceCalculation
{
    public static int CalculateExperienceToLevel(int targetLevel)
    {
        int expSum = 0;
        if (targetLevel > 1) expSum += 1000;

        int lvlSum = 1;
        while (lvlSum < targetLevel)
        {
            expSum += (int)(expSum * 1.1F);
            lvlSum++;
        }
        return expSum;
    }

    public static int CalculateLevelUps(int currentLevel, int currentExperience)
    {
        int levelUps = 0;
        int nextLevel = currentLevel + 1;
        while (true)
        {
            int expToNext = CalculateExperienceToLevel(nextLevel);
            if (expToNext <= currentExperience)
            {
                levelUps++;
            }
            else
            {
                break;
            }
        }
        levelUps -= currentLevel;
        return levelUps;
    }

    public static int FullExperienceCalculation(List<AbstractCombatantPiece2> pieces)
    {
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
        if (hero.isDead)
        {
            result = hero.hero.combatPieceStats.hitPoints_maximum;
            result += hero.hero.experienceStats.level * 50;
        }
        return result;
    }

    public static int UnitExperience(CombatantUnitPiece2 unit)
    {
        int result = 0;
        int stackDif = unit.unit.stackStats.stack_maximum - unit.stackStats.stack_current;
        if (stackDif > 0)
        {
            result = unit.unit.combatPieceStats.hitPoints_maximum;
            result *= stackDif;
        }
        return result;
    }
}
