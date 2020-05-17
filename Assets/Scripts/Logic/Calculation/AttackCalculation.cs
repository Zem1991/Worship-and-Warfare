using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AttackCalculation
{
    public static int FullDamageCalculation(AttackStats2 attack, CombatantPiece3 attacker, CombatantPiece3 defender, CombatantPiece3 attackerHero, CombatantPiece3 defenderHero)
    {
        float dmgBase = CombatantDamage(attack, attacker);
        float increments = Increments(attack, attacker, defender, attackerHero, defenderHero);
        float reductions = Reductions(attack, attacker, defender, attackerHero, defenderHero);
        float terrain = Terrain(attack, attacker, defender, attackerHero, defenderHero);
        Debug.Log("DamageCalculation result: " + dmgBase + " * " + increments + " * " + reductions + " * " + terrain);

        float fullFormula = dmgBase * increments * reductions * terrain;
        int result = Mathf.CeilToInt(fullFormula);
        return Mathf.Max(result, 1);
    }

    public static float CombatantDamage(AttackStats2 attack, CombatantPiece3 attacker)
    {
        float result = 0;
        CombatUnitPiece3 attackerAsUnitPiece = attacker as CombatUnitPiece3;
        if (attackerAsUnitPiece)
        {
            CombatUnit attackerAsCombatUnit = attackerAsUnitPiece.GetCombatUnit();
            for (int i = 0; i < attackerAsCombatUnit.GetStackHealthStats().GetStackSize(); i++)
                result += Random.Range(attack.damage_minimum, attack.damage_maximum + 1);
        }
        else
        {
            result += Random.Range(attack.damage_minimum, attack.damage_maximum + 1);
        }
        return result;
    }

    public static float Increments(AttackStats2 attack, CombatantPiece3 attacker, CombatantPiece3 defender, CombatantPiece3 attackerHero, CombatantPiece3 defenderHero)
    {
        float attackerHeroOffense = I1_AttackerHeroOffense(attackerHero, defenderHero);
        return 1 + attackerHeroOffense;
    }

    public static float I1_AttackerHeroOffense(CombatantPiece3 attackerHero, CombatantPiece3 defenderHero)
    {
        float result = 0;
        if (attackerHero == null || attackerHero.stateDead) return result;

        float atrDif = attackerHero.attributeStats.attributes.offense;
        if (defenderHero != null && !defenderHero.stateDead) atrDif -= defenderHero.attributeStats.attributes.defense;

        if (atrDif > 0)
        {
            result = 0.05F * atrDif;
        }
        return result;
    }

    public static float Reductions(AttackStats2 attack, CombatantPiece3 attacker, CombatantPiece3 defender, CombatantPiece3 attackerHero, CombatantPiece3 defenderHero)
    {
        float defenderHeroDefense = 1 - R1_DefenderHeroDefense(attackerHero, defenderHero);
        float defenderIsHero = 1 - RX_DefenderIsHero(attacker, defender);
        float defenderIsDefending = 1 - RX_DefenderIsDefending(defender);
        return defenderHeroDefense * defenderIsHero * defenderIsDefending;
    }

    public static float R1_DefenderHeroDefense(CombatantPiece3 attackerHero, CombatantPiece3 defenderHero)
    {
        float result = 0;
        if (defenderHero == null || defenderHero.stateDead) return result;

        float atrDif = defenderHero.attributeStats.attributes.defense;
        if (attackerHero != null && !attackerHero.stateDead) atrDif -= attackerHero.attributeStats.attributes.offense;

        if (atrDif > 0)
        {
            result = 0.025F * atrDif;
        }
        return result;
    }

    public static float RX_DefenderIsHero(CombatantPiece3 attacker, CombatantPiece3 defender)
    {
        float result = 0;
        CombatUnitPiece3 attackerUnitPiece = attacker as CombatUnitPiece3;
        HeroUnitPiece3 defenderUnitPiece = defender as HeroUnitPiece3;

        if (attackerUnitPiece && defenderUnitPiece)
        {
            CombatUnit attackerAsUnit = attackerUnitPiece.GetCombatUnit();
            HeroUnit defenderAsHero = defenderUnitPiece.GetHeroUnit();

            if (attackerAsUnit && defenderAsHero)
            {
                int tierMath = 6 - Mathf.Clamp(attackerAsUnit.GetDBCombatUnit().tier, 1, 5);
                result = 1F / tierMath;
                result = 1 - Mathf.Clamp01(result);
            }
        }
        return result;
    }

    public static float RX_DefenderIsDefending(CombatantPiece3 defender)
    {
        float result = 0;
        if (defender.pieceCombatActions.stateDefend) result = 0.25F;
        return result;
    }

    public static float Terrain(AttackStats2 attack, CombatantPiece3 attacker, CombatantPiece3 defender, CombatantPiece3 attackerHero, CombatantPiece3 defenderHero)
    {
        float rangeMod = TX_RangeModifier(attack, attacker, defender);
        float obstacleMod = TX_ObstacleModifier(attack, attacker, defender);
        return rangeMod * obstacleMod;
    }

    public static float TX_RangeModifier(AttackStats2 attack, CombatantPiece3 attacker, CombatantPiece3 defender)
    {
        float result = 1;
        if (attack.IsRanged())
        {
            Vector3 attackerPos = attacker.currentTile.transform.position;
            Vector3 defenderPos = defender.currentTile.transform.position;
            float distance = Vector3.Distance(attackerPos, defenderPos);
            if (distance > attack.range_maximum) result = 0.5F;
        }
        return result;
    }

    public static float TX_ObstacleModifier(AttackStats2 attack, CombatantPiece3 attacker, CombatantPiece3 defender)
    {
        float result = 1;
        if (attack.IsRanged())
        {
            CombatMap map = CombatManager.Instance.mapHandler.map;
            List<CombatTile> tiles = map.AreaLine(attacker.currentTile as CombatTile, defender.currentTile as CombatTile);

            int obstacles = 0;
            foreach (var item in tiles)
            {
                if (item.obstaclePiece) obstacles++;
            }
            if (obstacles >= 0) result = 0.5F;
            if (obstacles >= 1) result = 0;
        }
        return result;
    }
}
