using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatManager : AbstractSingleton<CombatManager>, IShowableHideable
{
    public readonly Vector2Int MAP_SIZE = new Vector2Int(15, 9);

    [Header("Prefabs")]
    public CombatTile prefabTile;
    public HeroCombatPiece prefabHero;
    public UnitCombatPiece prefabUnit;

    [Header("Auxiliary Objects")]
    public CombatMapHandler mapHandler;
    public CombatPieceHandler pieceHandler;

    [Header("Combat Flow")]
    public bool combatStarted;
    public int currentTurn;
    public UnitCombatPiece currentUnit;
    public List<UnitCombatPiece> turnSequence;

    public void BootCombat(FieldPiece attackerPiece, FieldPiece defenderPiece)
    {
        //background = battleground.image;

        //TODO INSTANTIATE COMBAT PIECES (HeroCombat and UnitCombat) USING DATA FROM RECEIVED PIECES!

        Debug.LogWarning("No tile data for combat map!");
        mapHandler.BuildMap(MAP_SIZE);
        pieceHandler.Create(attackerPiece, defenderPiece);
        pieceHandler.InitialHeroPositions(mapHandler.map);
        pieceHandler.InitialUnitPositions(mapHandler.map);

        combatStarted = true;
        NextTurn();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        mapHandler.gameObject.SetActive(false);
        pieceHandler.gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        mapHandler.gameObject.SetActive(true);
        pieceHandler.gameObject.SetActive(true);
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
        List<UnitCombatPiece> newSequence = new List<UnitCombatPiece>();
        newSequence.AddRange(pieceHandler.GetActiveUnits(pieceHandler.attackerUnits));
        newSequence.AddRange(pieceHandler.GetActiveUnits(pieceHandler.defenderUnits));
        turnSequence = newSequence;
        UpdateTurnSequence();
    }

    public void UpdateTurnSequence()
    {
        turnSequence.OrderByDescending(a => a.initiative);
    }

    public void AddUnitToTurnSequence(UnitCombatPiece uc)
    {
        turnSequence.Add(uc);
        UpdateTurnSequence();
    }

    public void RemoveUnitFromTurnSequence(UnitCombatPiece uc)
    {
        turnSequence.Remove(uc);
        turnSequence.TrimExcess();
        UpdateTurnSequence();
    }
}
