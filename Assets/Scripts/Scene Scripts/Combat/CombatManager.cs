using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatManager : AbstractSingleton<CombatManager>, IShowableHideable
{
    private const string localPlayerVictoryMsg = "You won the battle!";
    private const string localPlayerDefeatMsg = "You lost the battle!";
    public readonly Vector2Int MAP_SIZE = new Vector2Int(15, 9);

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
    public CombatUnitPiece currentUnit;
    public List<CombatUnitPiece> turnSequence = new List<CombatUnitPiece>();
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
            Vector3 pos = currentUnit.transform.position;

            CombatInputs ci = CombatInputs.Instance;
            ci.selectionPos = new Vector2Int((int)pos.x, (int)pos.z);
            ci.selectionTile = currentUnit.currentTile as CombatTile;
            ci.selectionPiece = currentUnit;

            ci.selectionHighlight.transform.position = currentUnit.transform.position;
            ci.canCommandSelectedPiece = currentUnit.owner == PlayerManager.Instance.localPlayer;

            turnSequence.RemoveAt(0);
            CombatUI.Instance.turnSequence.RemoveFirstFromTurnSequence();

            Player owner = currentUnit.owner;
            if (owner.type == PlayerType.COMPUTER)
            {
                owner.aiPersonality.CombatRoutine();
            }
        }
        else
        {
            NextTurn();
        }
    }

    public void NextTurn()
    {
        if (CalculateFullTurnSequence())
        {
            NextUnit();
            currentTurn++;
        }
    }

    public bool CalculateFullTurnSequence()
    {
        List<CombatUnitPiece> newSequence = new List<CombatUnitPiece>();
        newSequence.AddRange(pieceHandler.GetActiveUnits(pieceHandler.attackerUnits));
        newSequence.AddRange(pieceHandler.GetActiveUnits(pieceHandler.defenderUnits));
        if (newSequence.Count <= 0) return false;

        turnSequence = newSequence;
        UpdateTurnSequence();
        return true;
    }

    public void UpdateTurnSequence()
    {
        turnSequence = turnSequence.OrderBy(a => a.spawnId).ToList();
        turnSequence = turnSequence.OrderByDescending(a => a.unit.initiative).ToList();
        CombatUI.Instance.turnSequence.CreateTurnSequence(turnSequence);
    }

    //public void AddUnitToTurnSequence(CombatUnitPiece uc)
    //{
    //    turnSequence.Add(uc);
    //    UpdateTurnSequence();
    //}

    //public void RemoveUnitFromTurnSequence(CombatUnitPiece uc)
    //{
    //    turnSequence.Remove(uc);
    //    UpdateTurnSequence();
    //}

    public void EscapeMenu()
    {
        bool isPaused = GameManager.Instance.isPaused;
        AUIPanel currentWindow = CombatUI.Instance.currentWindow;
        if (!isPaused && currentWindow)
        {
            CombatUI.Instance.CloseCurrentWindow();
            return;
        }

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
