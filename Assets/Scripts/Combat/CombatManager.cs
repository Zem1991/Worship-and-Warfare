using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatManager : Singleton<CombatManager>
{
    [Header("Prefabs")]
    public CombatTile prefabTile;
    public HeroCombat prefabHero;
    public UnitCombat prefabUnit;

    [Header("Auxiliary Objects")]
    public CombatMap map;
    public CombatPieces pieces;

    [Header("Battleground")]
    public Sprite background;

    [Header("Combat Flow")]
    public bool combatStarted;
    public int currentTurn;
    public UnitCombat currentUnit;
    public List<UnitCombat> turnSequence;

    public void StartCombat(DB_Battleground battleground, Piece attackerPiece, Piece defenderPiece)
    {
        //background = battleground.image;

        //TODO INSTANTIATE COMBAT PIECES (HeroCombat and UnitCombat) USING DATA FROM RECEIVED PIECES!

        Debug.LogWarning("No tile data for combat map!");
        map.Create(null);
        pieces.Create(attackerPiece, defenderPiece);
        pieces.InitialHeroPositions(map.attackerHeroTile, map.defenderHeroTile);
        pieces.InitialUnitPositions(map.attackerStartTiles, map.defenderStartTiles);

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
        newSequence.AddRange(pieces.GetActiveUnits(pieces.attackerUnits));
        newSequence.AddRange(pieces.GetActiveUnits(pieces.defenderUnits));
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
}
