using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DamageCalculation
{
    public static int FullDamageCalculation(AttackStats attack, AbstractCombatPiece2 attacker, AbstractCombatPiece2 defender, CombatantHeroPiece2 attackerHero, CombatantHeroPiece2 defenderHero)
    {
        float dmgBase = CombatantDamage(attack, attacker);
        float increments = Increments(attacker, defender, attackerHero, defenderHero);
        float reductions = Reductions(attacker, defender, attackerHero, defenderHero);
        Debug.Log("DamageCalculation result: " + dmgBase + " * " + increments + " * " + reductions);

        float fullFormula = dmgBase * increments * reductions;
        int result = Mathf.CeilToInt(fullFormula);
        return Mathf.Max(result, 1);
    }

    public static float CombatantDamage(AttackStats attack, AbstractCombatPiece2 attacker)
    {
        float result = 0;
        CombatantUnitPiece2 attackerAsUnit = attacker as CombatantUnitPiece2;
        if (attackerAsUnit)
        {
            for (int i = 0; i < attackerAsUnit.stackStats.stack_current; i++)
                result += Random.Range(attack.damage_minimum, attack.damage_maximum + 1);
        }
        else
        {
            result += Random.Range(attack.damage_minimum, attack.damage_maximum + 1);
        }
        return result;
    }

    public static float Increments(AbstractCombatPiece2 attacker, AbstractCombatPiece2 defender, CombatantHeroPiece2 attackerHero, CombatantHeroPiece2 defenderHero)
    {
        float attackerHeroOffense = I1_AttackerHeroOffense(attackerHero, defenderHero);
        return 1 + attackerHeroOffense;
    }

    public static float I1_AttackerHeroOffense(CombatantHeroPiece2 attackerHero, CombatantHeroPiece2 defenderHero)
    {
        float result = 0;
        if (attackerHero == null || attackerHero.stateDead) return result;

        float atrDif = attackerHero.attributeStats.atrOffense;
        if (defenderHero != null && !defenderHero.stateDead) atrDif -= defenderHero.attributeStats.atrDefense;

        if (atrDif > 0)
        {
            result = 0.05F * atrDif;
        }
        return result;
    }

    public static float Reductions(AbstractCombatPiece2 attacker, AbstractCombatPiece2 defender, CombatantHeroPiece2 attackerHero, CombatantHeroPiece2 defenderHero)
    {
        float defenderHeroDefense = 1 - R1_DefenderHeroDefense(attackerHero, defenderHero);
        float defenderIsHero = 1 - RX_DefenderIsHero(attacker, defender);
        float defenderIsDefending = 1 - RX_DefenderIsDefending(defender);
        return defenderHeroDefense * defenderIsHero * defenderIsDefending;
    }

    public static float R1_DefenderHeroDefense(CombatantHeroPiece2 attackerHero, CombatantHeroPiece2 defenderHero)
    {
        float result = 0;
        if (defenderHero == null || defenderHero.stateDead) return result;

        float atrDif = defenderHero.attributeStats.atrDefense;
        if (attackerHero != null && !attackerHero.stateDead) atrDif -= attackerHero.attributeStats.atrOffense;

        if (atrDif > 0)
        {
            result = 0.025F * atrDif;
        }
        return result;
    }

    public static float RX_DefenderIsHero(AbstractCombatPiece2 attacker, AbstractCombatPiece2 defender)
    {
        float result = 0;
        CombatantUnitPiece2 attackerAsUnit = attacker as CombatantUnitPiece2;
        CombatantHeroPiece2 defenderAsHero = defender as CombatantHeroPiece2;
        if (attackerAsUnit && defenderAsHero)
        {
            int tierMath = 6 - Mathf.Clamp(attackerAsUnit.unit.dbData.tier, 1, 5);
            result = 1F / tierMath;
            result = 1 - Mathf.Clamp01(result);
        }
        return result;
    }

    public static float RX_DefenderIsDefending(AbstractCombatPiece2 defender)
    {
        float result = 0;
        if (defender.pieceCombatActions.stateDefend) result = 0.25F;
        return result;
    }
}
