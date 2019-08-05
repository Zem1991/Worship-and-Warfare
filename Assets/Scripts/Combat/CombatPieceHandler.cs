using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPieceHandler : MonoBehaviour
{
    [Header("Attacker")]
    public HeroCombatPiece attackerHero;
    public List<UnitCombatPiece> attackerUnits;

    [Header("Defender")]
    public HeroCombatPiece defenderHero;
    public List<UnitCombatPiece> defenderUnits;

    public void Remove()
    {
        if (attackerUnits != null)
        {
            foreach (var item in attackerUnits)
            {
                Destroy(item);
            }
            attackerUnits = null;
        }

        if (defenderUnits != null)
        {
            foreach (var item in defenderUnits)
            {
                Destroy(item);
            }
            defenderUnits = null;
        }
    }

    public void Create(FieldPiece attackerPiece, FieldPiece defenderPiece)
    {
        Remove();

        CombatManager cm = CombatManager.Instance;
        attackerUnits = new List<UnitCombatPiece>();
        defenderUnits = new List<UnitCombatPiece>();

        if (attackerPiece.hero != null)
        {
            attackerHero = Instantiate(cm.prefabHero, transform);
            attackerHero.Initialize(attackerPiece.hero);
        }

        if (defenderPiece.hero != null)
        {
            defenderHero = Instantiate(cm.prefabHero, transform);
            defenderHero.Initialize(defenderPiece.hero);
        }

        if (attackerPiece.units != null)
        {
            foreach (var item in attackerPiece.units)
            {
                UnitCombatPiece uc = Instantiate(cm.prefabUnit, transform);
                uc.Initialize(item);
                attackerUnits.Add(uc);
            }
        }

        if (defenderPiece.units != null)
        {
            foreach (var item in defenderPiece.units)
            {
                UnitCombatPiece uc = Instantiate(cm.prefabUnit, transform);
                uc.Initialize(item);
                defenderUnits.Add(uc);
            }
        }
    }

    public void InitialHeroPositions(CombatMap map)
    {
        CombatTile attackerHeroTile = map.attackerHeroTile;
        CombatTile defenderHeroTile = map.defenderHeroTile;

        if (attackerHero)
        {
            attackerHero.transform.position = attackerHeroTile.transform.position;
            attackerHero.currentTile = attackerHeroTile;
            attackerHeroTile.occupantPiece = attackerHero;
        }

        if (defenderHero)
        {
            defenderHero.transform.position = defenderHeroTile.transform.position;
            defenderHero.currentTile = defenderHeroTile;
            defenderHeroTile.occupantPiece = defenderHero;
        }
    }

    public void InitialUnitPositions(CombatMap map)
    {
        List<CombatTile> attackerHeroTiles = map.attackerStartTiles;
        List<CombatTile> defenderHeroTiles = map.defenderStartTiles;

        if (attackerUnits != null)
        {
            for (int i = 0; i < attackerUnits.Count; i++)
            {
                attackerUnits[i].transform.position = attackerHeroTiles[i].transform.position;
                attackerUnits[i].currentTile = attackerHeroTiles[i];
                attackerHeroTiles[i].occupantPiece = attackerUnits[i];
            }
        }

        if (defenderUnits != null)
        {
            for (int i = 0; i < attackerUnits.Count; i++)
            {
                defenderUnits[i].transform.position = defenderHeroTiles[i].transform.position;
                defenderUnits[i].currentTile = defenderHeroTiles[i];
                defenderHeroTiles[i].occupantPiece = defenderUnits[i];
            }
        }
    }

    public List<UnitCombatPiece> GetActiveUnits(List<UnitCombatPiece> units)
    {
        List<UnitCombatPiece> result = new List<UnitCombatPiece>();
        foreach (var item in units)
        {
            if (item.hitPointsCurrent > 0) result.Add(item);
        }
        return result;
    }
}
