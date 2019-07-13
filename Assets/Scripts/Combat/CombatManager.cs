using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Singleton;

    [Header("Prefabs")]
    public CombatTile prefabTile;

    [Header("Diorama")]
    public Diorama diorama;

    [Header("Battleground")]
    public Sprite background;

    [Header("Attacker")]
    public Player attacker;
    public CombatHero attackerHero;
    public List<CombatUnit> attackerUnits;

    [Header("Defender")]
    public Player defender;
    public CombatHero defenderHero;
    public List<CombatUnit> defenderUnits;

    [Header("Combat Flow")]
    public bool combatStarted;
    public int currentTurn;
    public CombatUnit currentUnit;
    public List<CombatUnit> turnSequence;

    void Awake()
    {
        if (Singleton != null)
        {
            Debug.LogWarning("Only one instance of CombatManager may exist! Deleting this extra one.");
            Destroy(this);
        }
        else
        {
            Singleton = this;
        }
    }

    public void StartCombat(DB_Battleground battleground, Piece attackerPiece, Piece defenderPiece)
    {
        background = battleground.image;

        attacker = attackerPiece.owner;
        //attackerHero = attackerPiece.hero;
        //attackerUnits = attackerPiece.units;

        defender = defenderPiece.owner;
        //defenderHero = defenderPiece.hero;
        //defenderUnits = defenderPiece.units;

        combatStarted = true;
        NextTurn();

        //TODO THIS IS JUST FOR TESTING!
        diorama.Create(null);
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
        List<CombatUnit> newSequence = new List<CombatUnit>();
        newSequence.AddRange(GetActiveUnits(attackerUnits));
        newSequence.AddRange(GetActiveUnits(defenderUnits));
        turnSequence = newSequence;
        UpdateTurnSequence();
    }

    public void UpdateTurnSequence()
    {
        turnSequence.OrderByDescending(a => a.initiative);
    }

    public void AddUnitToTurnSequence(CombatUnit cu)
    {
        turnSequence.Add(cu);
        UpdateTurnSequence();
    }

    public void RemoveUnitFromTurnSequence(CombatUnit cu)
    {
        turnSequence.Remove(cu);
        turnSequence.TrimExcess();
        UpdateTurnSequence();
    }

    public List<CombatUnit> GetActiveUnits(List<CombatUnit> units)
    {
        List<CombatUnit> result = new List<CombatUnit>();
        foreach (var item in units)
        {
            if (item.hp > 0) result.Add(item);
        }
        return result;
    }
}
