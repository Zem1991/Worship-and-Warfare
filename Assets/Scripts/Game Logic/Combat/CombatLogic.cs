using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CombatLogic
{
    public static int DamageCalculation(AbstractCombatantPiece2 attacker, AbstractCombatantPiece2 defender, CombatantHeroPiece2 attackerHero, CombatantHeroPiece2 defenderHero)
    {
        float dmgBase = UnitDamage(attacker);
        float increments = Increments(attackerHero, defenderHero);
        float reductions = Reductions(attackerHero, defenderHero);
        Debug.Log("DamageCalculation result: " + dmgBase + " * " + increments + " * " + reductions);
        float fullFormula = dmgBase * increments * reductions;
        int result = Mathf.CeilToInt(fullFormula);
        return Mathf.Max(result, 1);
    }

    public static float UnitDamage(AbstractCombatantPiece2 unit)
    {
        float result = 0;
        CombatantUnitPiece2 asUnit = unit as CombatantUnitPiece2;
        if (asUnit)
        {
            for (int i = 0; i < asUnit.stackStats.stack_current; i++)
                result += Random.Range(asUnit.combatPieceStats.attack_primary.damage_minimum, asUnit.combatPieceStats.attack_primary.damage_maximum + 1);
        }
        else
        {
            //TODO
        }
        return result;
    }

    public static float Increments(CombatantHeroPiece2 attackerHero, CombatantHeroPiece2 defenderHero)
    {
        return 1
            + I1_AttackerOffense(attackerHero, defenderHero);
    }

    public static float I1_AttackerOffense(CombatantHeroPiece2 attackerHero, CombatantHeroPiece2 defenderHero)
    {
        float result = 0;
        if (attackerHero == null) return result;

        float atrDif = attackerHero.hero.attributeStats.atrOffense;
        if (defenderHero != null) atrDif -= defenderHero.hero.attributeStats.atrDefense;

        if (atrDif > 0)
        {
            float perDif = 0.05F * atrDif;
            result += perDif;
        }
        return result;
    }

    public static float Reductions(CombatantHeroPiece2 attackerHero, CombatantHeroPiece2 defenderHero)
    {
        return 1
            - R1_DefenderDefense(attackerHero, defenderHero);
    }

    public static float R1_DefenderDefense(CombatantHeroPiece2 attackerHero, CombatantHeroPiece2 defenderHero)
    {
        float result = 0;
        if (defenderHero == null) return result;

        float atrDif = defenderHero.hero.attributeStats.atrDefense;
        if (attackerHero != null) atrDif -= attackerHero.hero.attributeStats.atrOffense;

        if (atrDif > 0)
        {
            float perDif = 0.025F * atrDif;
            result += perDif;
        }
        return result;
    }
}
