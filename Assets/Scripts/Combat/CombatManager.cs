using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatManager : Singleton<CombatManager>
{
    [Header("Prefabs")]
    public CombatTile prefabTile;

    [Header("Combat Map")]
    public CombatMap map;

    [Header("Battleground")]
    public Sprite background;

    [Header("Attacker")]
    public Player attacker;
    public HeroCombat attackerHero;
    public List<UnitCombat> attackerUnits;

    [Header("Defender")]
    public Player defender;
    public HeroCombat defenderHero;
    public List<UnitCombat> defenderUnits;

    [Header("Combat Flow")]
    public bool combatStarted;
    public int currentTurn;
    public UnitCombat currentUnit;
    public List<UnitCombat> turnSequence;

    public void StartCombat(DB_Battleground battleground, Piece attackerPiece, Piece defenderPiece)
    {
        //background = battleground.image;

        //TODO INSTANTIATE COMBAT PIECES (HeroCombat and UnitCombat) USING DATA FROM RECEIVED PIECES!

        attacker = attackerPiece.owner;
        attackerHero = attackerPiece.hero;
        attackerUnits = attackerPiece.units;

        defender = defenderPiece.owner;
        defenderHero = defenderPiece.hero;
        defenderUnits = defenderPiece.units;

        Debug.LogWarning("No tile data for combat map!");
        map.Create(null);

        combatStarted = true;
        NextTurn();
    }

    public void NextUnit()
    {
        if (turnSequence.Count > 0)
        {
            currentUnit = turnSequence[0];
            turnSequence.RemoveAt(0);
            turnSequence.TrimExcess();
        }
        else
        {
            NextTurn();
        }
    }

    public void NextTurn()
    {
        CalculateFullTurnSequence();
        currentTurn++;
    }

    public void CalculateFullTurnSequence()
    {
        List<UnitCombat> newSequence = new List<UnitCombat>();
        newSequence.AddRange(GetActiveUnits(attackerUnits));
        newSequence.AddRange(GetActiveUnits(defenderUnits));
        turnSequence = newSequence;
        UpdateTurnSequence();
    }

    public void UpdateTurnSequence()
    {
        turnSequence.OrderByDescending(a => a.initiative);
    }

    public void AddUnitToTurnSequence(UnitCombat uc)
    {
        turnSequence.Add(uc);
        UpdateTurnSequence();
    }

    public void RemoveUnitFromTurnSequence(UnitCombat uc)
    {
        turnSequence.Remove(uc);
        turnSequence.TrimExcess();
        UpdateTurnSequence();
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
