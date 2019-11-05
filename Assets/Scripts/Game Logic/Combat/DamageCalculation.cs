using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DamageCalculation
{
    public static int FullDamageCalculation(AbstractCombatantPiece2 attacker, AbstractCombatantPiece2 defender, CombatantHeroPiece2 attackerHero, CombatantHeroPiece2 defenderHero)
    {
        float dmgBase = CombatantDamage(attacker);
        float increments = Increments(attacker, defender, attackerHero, defenderHero);
        float reductions = Reductions(attacker, defender, attackerHero, defenderHero);
        Debug.Log("DamageCalculation result: " + dmgBase + " * " + increments + " * " + reductions);

        float fullFormula = dmgBase * increments * reductions;
        int result = Mathf.CeilToInt(fullFormula);
        return Mathf.Max(result, 1);
    }

    public static float CombatantDamage(AbstractCombatantPiece2 attacker)
    {
        float result = 0;
        CombatantHeroPiece2 attackerAsHero = attacker as CombatantHeroPiece2;
        CombatantUnitPiece2 attackerAsUnit = attacker as CombatantUnitPiece2;
        if (attackerAsHero)
        {
            result += Random.Range(attackerAsHero.combatPieceStats.attack_primary.damage_minimum, attackerAsHero.combatPieceStats.attack_primary.damage_maximum + 1);
        }
        else if (attackerAsUnit)
        {
            for (int i = 0; i < attackerAsUnit.stackStats.stack_current; i++)
                result += Random.Range(attackerAsUnit.combatPieceStats.attack_primary.damage_minimum, attackerAsUnit.combatPieceStats.attack_primary.damage_maximum + 1);
        }
        return result;
    }

    public static float Increments(AbstractCombatantPiece2 attacker, AbstractCombatantPiece2 defender, CombatantHeroPiece2 attackerHero, CombatantHeroPiece2 defenderHero)
    {
        float attackerHeroOffense = I1_AttackerHeroOffense(attackerHero, defenderHero);
        return 1 + attackerHeroOffense;
    }

    public static float I1_AttackerHeroOffense(CombatantHeroPiece2 attackerHero, CombatantHeroPiece2 defenderHero)
    {
        float result = 0;
        if (attackerHero == null || attackerHero.isDead) return result;

        float atrDif = attackerHero.attributeStats.atrOffense;
        if (defenderHero != null && !defenderHero.isDead) atrDif -= defenderHero.attributeStats.atrDefense;

        if (atrDif > 0)
        {
            result = 0.05F * atrDif;
        }
        return result;
    }

    public static float Reductions(AbstractCombatantPiece2 attacker, AbstractCombatantPiece2 defender, CombatantHeroPiece2 attackerHero, CombatantHeroPiece2 defenderHero)
    {
        float defenderHeroDefense = 1 - R1_DefenderHeroDefense(attackerHero, defenderHero);
        float defenderIsHero = 1 - RX_DefenderIsHero(attacker, defender);
        float defenderIsDefending = 1 - RX_DefenderIsDefending(defender);
        return defenderHeroDefense * defenderIsHero * defenderIsDefending;
    }

    public static float R1_DefenderHeroDefense(CombatantHeroPiece2 attackerHero, CombatantHeroPiece2 defenderHero)
    {
        float result = 0;
        if (defenderHero == null || defenderHero.isDead) return result;

        float atrDif = defenderHero.attributeStats.atrDefense;
        if (attackerHero != null && !attackerHero.isDead) atrDif -= attackerHero.attributeStats.atrOffense;

        if (atrDif > 0)
        {
            result = 0.025F * atrDif;
        }
        return result;
    }

    public static float RX_DefenderIsHero(AbstractCombatantPiece2 attacker, AbstractCombatantPiece2 defender)
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

    public static float RX_DefenderIsDefending(AbstractCombatantPiece2 defender)
    {
        float result = 0;
        if (defender.stateDefending) result = 0.25F;
        return result;
    }
}
