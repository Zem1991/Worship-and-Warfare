﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CombatLogic
{
    public static int DamageCalculation(UnitCombatPiece attackerUnit, UnitCombatPiece defenderUnit, HeroCombatPiece attackerHero, HeroCombatPiece defenderHero)
    {
        float dmgBase = UnitDamage(attackerUnit);
        float increments = Increments(attackerHero, defenderHero);
        float reductions = Reductions(attackerHero, defenderHero);
        Debug.Log("DamageCalculation result: " + dmgBase + " * " + increments + " * " + reductions);
        float fullFormula = dmgBase * increments * reductions;
        int result = Mathf.CeilToInt(fullFormula);
        return Mathf.Max(result, 1);
    }

    public static float UnitDamage(UnitCombatPiece unit)
    {
        float result = 0;
        for (int i = 0; i < unit.stackSize; i++)
        {
            result += Random.Range(unit.damageMin, unit.damageMax + 1);
        }
        return result;
    }

    public static float Increments(HeroCombatPiece attackerHero, HeroCombatPiece defenderHero)
    {
        return 1
            + I1_AttackerOffense(attackerHero, defenderHero);
    }

    public static float I1_AttackerOffense(HeroCombatPiece attackerHero, HeroCombatPiece defenderHero)
    {
        float result = 0;
        if (attackerHero == null) return result;

        float atrDif = attackerHero.atrOffense;
        if (defenderHero != null) atrDif -= defenderHero.atrDefense;

        if (atrDif > 0)
        {
            float perDif = 0.05F * atrDif;
            result += perDif;
        }
        return result;
    }

    public static float Reductions(HeroCombatPiece attackerHero, HeroCombatPiece defenderHero)
    {
        return 1
            - R1_DefenderDefense(attackerHero, defenderHero);
    }

    public static float R1_DefenderDefense(HeroCombatPiece attackerHero, HeroCombatPiece defenderHero)
    {
        float result = 0;
        if (defenderHero == null) return result;

        float atrDif = defenderHero.atrDefense;
        if (attackerHero != null) atrDif -= attackerHero.atrOffense;

        if (atrDif > 0)
        {
            float perDif = 0.025F * atrDif;
            result -= perDif;
        }
        return result;
    }
}