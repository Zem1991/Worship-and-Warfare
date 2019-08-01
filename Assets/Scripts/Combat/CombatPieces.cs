using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPieces : MonoBehaviour
{
    [Header("Attacker")]
    public HeroCombat attackerHero;
    public List<UnitCombat> attackerUnits;

    [Header("Defender")]
    public HeroCombat defenderHero;
    public List<UnitCombat> defenderUnits;

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

    public void Create(Piece attackerPiece, Piece defenderPiece)
    {
        Remove();

        CombatManager cm = CombatManager.Instance;
        attackerUnits = new List<UnitCombat>();
        defenderUnits = new List<UnitCombat>();

        if (attackerPiece.hero != null)
        {
            attackerHero = Instantiate(cm.prefabHero);
            attackerHero.Initialize(attackerPiece.hero);
        }

        if (defenderPiece.hero != null)
        {
            attackerHero = Instantiate(cm.prefabHero);
            attackerHero.Initialize(defenderPiece.hero);
        }

        if (attackerPiece.units != null)
        {
            foreach (var item in attackerPiece.units)
            {
                UnitCombat uc = Instantiate(cm.prefabUnit);
                uc.Initialize(item);
                attackerUnits.Add(uc);
            }
        }

        if (defenderPiece.units != null)
        {
            foreach (var item in defenderPiece.units)
            {
                UnitCombat uc = Instantiate(cm.prefabUnit);
                uc.Initialize(item);
                defenderUnits.Add(uc);
            }
        }
    }

    public void InitialHeroPositions(CombatTile attackerHeroTile, CombatTile defenderHeroTile)
    {
        if (attackerHero)
        {
            attackerHero.transform.position = attackerHeroTile.transform.position;
            attackerHero.tile = attackerHeroTile;
            attackerHeroTile.piece = attackerHero;
        }

        if (defenderHero)
        {
            defenderHero.transform.position = defenderHeroTile.transform.position;
            defenderHero.tile = defenderHeroTile;
            defenderHeroTile.piece = defenderHero;
        }
    }

    public void InitialUnitPositions(List<CombatTile> attackerHeroTiles, List<CombatTile> defenderHeroTiles)
    {
        if (attackerUnits != null)
        {
            for (int i = 0; i < attackerUnits.Count; i++)
            {
                attackerUnits[i].transform.position = attackerHeroTiles[i].transform.position;
                attackerUnits[i].tile = attackerHeroTiles[i];
                attackerHeroTiles[i].piece = attackerUnits[i];
            }
        }

        if (defenderUnits != null)
        {
            for (int i = 0; i < attackerUnits.Count; i++)
            {
                defenderUnits[i].transform.position = defenderHeroTiles[i].transform.position;
                defenderUnits[i].tile = defenderHeroTiles[i];
                defenderHeroTiles[i].piece = defenderUnits[i];
            }
        }
    }

    public List<UnitCombat> GetActiveUnits(List<UnitCombat> units)
    {
        List<UnitCombat> result = new List<UnitCombat>();
        foreach (var item in units)
        {
            if (item.hitPointsCurrent > 0) result.Add(item);
        }
        return result;
    }
}
