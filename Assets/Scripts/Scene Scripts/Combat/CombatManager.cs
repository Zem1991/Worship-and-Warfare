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
    public List<UnitCombatPiece> turnSequence = new List<UnitCombatPiece>();
    public List<string> combatLog = new List<string>();
    public CombatResult result;

    public void TerminateCombat()
    {
        mapHandler.ClearMap();
        pieceHandler.Remove();

        combatStarted = false;
        currentTurn = 0;
        currentUnit = null;
        turnSequence.Clear();
        result = CombatResult.NOT_FINISHED;
    }

    public void BootCombat(FieldPiece attackerPiece, FieldPiece defenderPiece, DB_Tileset tileset)
    {
        CombatUI.Instance.EscapeMenuHide();
        CombatUI.Instance.ResultPopupHide();

        //background = battleground.image;

        attacker = attackerPiece.owner;
        this.attackerPiece = attackerPiece;
        defender = defenderPiece.owner;
        this.defenderPiece = defenderPiece;

        Debug.LogWarning("No tile data for combat map!");
        mapHandler.BuildMap(MAP_SIZE, tileset);
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

    public bool IsCombatRunning()
    {
        return result == CombatResult.NOT_FINISHED;
    }

    public void NextUnit()
    {
        if (turnSequence.Count > 0)
        {
            currentUnit = turnSequence[0];
            turnSequence.RemoveAt(0);
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
        UpdateTurnSequence();
    }

    public void EscapeMenu()
    {
        bool pauseStatus = GameManager.Instance.PauseUnpause();
        if (pauseStatus) CombatUI.Instance.EscapeMenuShow();
        else CombatUI.Instance.EscapeMenuHide();
    }

    public void CombatEnd(CombatResult result)
    {
        GameManager.Instance.PauseUnpause(false);
        CombatUI.Instance.EscapeMenuHide();

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

    public void CombatEndForceDefeat()
    {
        Player localPlayer = PlayerManager.Instance.localPlayer;
        if (attacker == localPlayer) CombatEnd(CombatResult.DEFENDER_WON);
        else if (defender == localPlayer) CombatEnd(CombatResult.ATTACKER_WON);
    }

    public void CombatEndConfirm()
    {
        //TODO CHANGE PIECES BEFORE SENDING THEM BACK.
        CombatUI.Instance.ResultPopupHide();
        GameManager.Instance.ReturnFromCombat(result, attackerPiece, defenderPiece);

        ClearLogs();
    }

    public List<string> GetLastLogs(int entries)
    {
        List<string> result = new List<string>();
        entries = Mathf.Min(entries, combatLog.Count);
        for (int i = 0; i < entries; i++)
        {
            int index = combatLog.Count - 1 - i;
            result.Add(combatLog[index]);
        }
        return result;
    }

    public void AddEntryToLog(string entry)
    {
        string currentDT = GameManager.Instance.currentDateTime;
        string fullEntry = "[Turn #" + currentTurn + "]" + "[" + currentDT + "]" + " " + entry;
        combatLog.Add(fullEntry);
    }

    public void ClearLogs()
    {
        combatLog.Clear();
    }
}
