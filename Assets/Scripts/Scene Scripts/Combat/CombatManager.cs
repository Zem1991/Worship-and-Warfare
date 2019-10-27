using System.Collections;
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
    public PartyPiece2 attackerParty;
    public Player defender;
    public PartyPiece2 defenderParty;

    [Header("Combat Flow")]
    public bool combatStarted;
    public int currentTurn;
    public AbstractCombatantPiece2 currentPiece;
    public AbstractCombatantPiece2 retaliatorPiece;
    public List<AbstractCombatantPiece2> turnSequence = new List<AbstractCombatantPiece2>();
    public List<string> combatLog = new List<string>();
    public CombatResult result;

    void Update()
    {
        if (combatStarted)
        {
            CheckBattleEnd();
        }
    }

    public void TerminateCombat()
    {
        mapHandler.ClearMap();
        pieceHandler.Remove();

        combatStarted = false;
        currentTurn = 0;
        currentPiece = null;
        turnSequence.Clear();
        result = CombatResult.NOT_FINISHED;
    }

    public void BootCombat(PartyPiece2 attackerPiece, PartyPiece2 defenderPiece, DB_Tileset tileset)
    {
        //background = battleground.image;

        attacker = attackerPiece.IPO_GetOwner();
        attackerParty = attackerPiece;
        defender = defenderPiece.IPO_GetOwner();
        defenderParty = defenderPiece;

        Debug.LogWarning("No tile data for combat map!");
        mapHandler.BuildMap(MAP_SIZE, tileset);
        pieceHandler.Create(attackerPiece, defenderPiece);
        pieceHandler.InitialPositions(mapHandler.map);
        //pieceHandler.InitialHeroPositions(mapHandler.map);
        //pieceHandler.InitialUnitPositions(mapHandler.map);

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
        retaliatorPiece = null;

        if (CheckBattleEnd()) return;

        if (turnSequence.Count > 0)
        {
            currentPiece = turnSequence[0];
            Vector3 pos = currentPiece.transform.position;

            CombatInputs ci = CombatInputs.Instance;
            ci.selectionPos = new Vector2Int((int)pos.x, (int)pos.z);
            ci.selectionTile = currentPiece.currentTile as CombatTile;
            ci.selectionPiece = currentPiece;

            ci.selectionHighlight.transform.position = currentPiece.transform.position;
            ci.canCommandSelectedPiece = currentPiece.IPO_GetOwner() == PlayerManager.Instance.localPlayer;

            turnSequence.RemoveAt(0);
            CombatUI.Instance.turnSequence.RemoveFirstFromTurnSequence();

            Player owner = currentPiece.IPO_GetOwner();
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
            foreach (AbstractCombatantPiece2 cup in turnSequence) cup.ICP_StartTurn();

            NextUnit();
            currentTurn++;
        }
    }

    public bool CalculateFullTurnSequence()
    {
        List<AbstractCombatantPiece2> newSequence = new List<AbstractCombatantPiece2>();
        newSequence.AddRange(pieceHandler.GetActivePieces(pieceHandler.attackerPieces));
        newSequence.AddRange(pieceHandler.GetActivePieces(pieceHandler.defenderPieces));
        if (newSequence.Count <= 0) return false;

        turnSequence = newSequence;
        UpdateTurnSequence();
        return true;
    }

    public void UpdateTurnSequence()
    {
        turnSequence = turnSequence.OrderBy(a => a.spawnId).ToList();
        turnSequence = turnSequence.OrderByDescending(a => a.combatPieceStats.initiative).ToList();
        CombatUI.Instance.turnSequence.CreateTurnSequence(turnSequence);
    }

    public void AddUnitToTurnSequence(AbstractCombatantPiece2 uc)
    {
        turnSequence.Add(uc);
        UpdateTurnSequence();
    }

    public void RemoveUnitFromTurnSequence(AbstractCombatantPiece2 uc)
    {
        turnSequence.Remove(uc);
        UpdateTurnSequence();
    }

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
        CombatUI.Instance.ResultPopupHide();

        if (result == CombatResult.ATTACKER_WON) pieceHandler.ApplyCombatChanges(attackerParty, pieceHandler.attackerPieces);
        else if (result == CombatResult.DEFENDER_WON) pieceHandler.ApplyCombatChanges(defenderParty, pieceHandler.defenderPieces);

        GameManager.Instance.ReturnFromCombat(result, attackerParty, defenderParty);

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

    private bool CheckBattleEnd()
    {
        int attackerActive = pieceHandler.GetActivePieces(pieceHandler.attackerPieces).Count;
        int defenderActive = pieceHandler.GetActivePieces(pieceHandler.defenderPieces).Count;

        if (attackerActive > 0 && defenderActive <= 0)
        {
            int attackerIdle = pieceHandler.GetIdlePieces(pieceHandler.attackerPieces).Count;
            if (attackerIdle >= attackerActive) CombatEnd(CombatResult.ATTACKER_WON);
            return true;
        }
        else if (defenderActive > 0 && attackerActive <= 0)
        {
            int defenderIdle = pieceHandler.GetIdlePieces(pieceHandler.defenderPieces).Count;
            if (defenderIdle >= defenderActive) CombatEnd(CombatResult.DEFENDER_WON);
            return true;
        }
        return false;
    }
}
