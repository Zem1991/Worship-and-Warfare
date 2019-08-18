using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CombatResult
{
    NOT_FINISHED,
    ATTACKER_WON,
    DEFENDER_WON
}

public class CombatManager : AbstractSingleton<CombatManager>, IShowableHideable
{
    private const string localPlayerVictoryMsg = "You won the battle!";
    private const string localPlayerDefeatMsg = "You lost the battle!";
    public readonly Vector2Int MAP_SIZE = new Vector2Int(15, 9);

    [Header("Prefabs")]
    public CombatTile prefabTile;
    public HeroCombatPiece prefabHero;
    public UnitCombatPiece prefabUnit;

    [Header("Auxiliary Objects")]
    public CombatMapHandler mapHandler;
    public CombatPieceHandler pieceHandler;

    [Header("Teams")]
    public Player attacker;
    public FieldPiece attackerPiece;
    public Player defender;
    public FieldPiece defenderPiece;

    [Header("Combat Flow")]
    public bool combatStarted;
    public int currentTurn;
    public UnitCombatPiece currentUnit;
    public List<UnitCombatPiece> turnSequence;
    public CombatResult result;

    public void BootCombat(FieldPiece attackerPiece, FieldPiece defenderPiece)
    {
        CombatUI.Instance.ResultPopupHide();

        //background = battleground.image;

        attacker = attackerPiece.owner;
        this.attackerPiece = attackerPiece;
        defender = defenderPiece.owner;
        this.defenderPiece = defenderPiece;

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

    public void CombatEnd(CombatResult result)
    {
        this.result = result;
        Player localPlayer = PlayerManager.Instance.localPlayer;
        string resultMsg;
        if ((result == CombatResult.ATTACKER_WON) && (attacker == localPlayer) ||
            (result == CombatResult.DEFENDER_WON) && (defender == localPlayer))
        {
            resultMsg = localPlayerVictoryMsg;
        }
        else
        {
            resultMsg = localPlayerDefeatMsg;
        }

        CombatUI.Instance.ResultPopupShow(resultMsg);
    }

    public void CombatEndConfirm()
    {
        //TODO CHANGE PIECES BEFORE SENDING THEM BACK.
        GameManager.Instance.ReturnFromCombat(result, attackerPiece, defenderPiece);
    }
}
