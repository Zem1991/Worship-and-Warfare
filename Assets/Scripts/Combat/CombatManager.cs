using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatManager : AbstractSingleton<CombatManager>, IShowableHideable
{
    private const string localPlayerVictoryMsg = "You won the battle!";
    private const string localPlayerDefeatMsg = "You lost the battle!";
    public readonly Vector2Int MAP_SIZE = new Vector2Int(15, 9);

    [Header("Auxiliary objects")]
    public CombatMapHandler mapHandler;
    public CombatPieceHandler pieceHandler;

    [Header("Combat flow")]
    public bool combatStarted;
    public int currentTurn;
    public AbstractCombatActorPiece2 currentPiece;
    public List<AbstractCombatActorPiece2> turnSequence = new List<AbstractCombatActorPiece2>();
    public List<AbstractCombatActorPiece2> waitSequence = new List<AbstractCombatActorPiece2>();
    public List<string> combatLog = new List<string>();
    public CombatResult result;

    [Header("Teams")]
    public Player attackerPlayer;
    public PartyPiece2 attackerParty;
    public Player defenderPlayer;
    public PartyPiece2 defenderParty;

    [Header("Battlefield")]
    public DB_Tileset tileset;

    public void Hide()
    {
        //gameObject.SetActive(false);
        mapHandler.gameObject.SetActive(false);
        pieceHandler.gameObject.SetActive(false);
    }

    public void Show()
    {
        //gameObject.SetActive(true);
        mapHandler.gameObject.SetActive(true);
        pieceHandler.gameObject.SetActive(true);
    }

    public void TerminateCombat()
    {
        mapHandler.ClearMap();
        //pieceHandler.Remove();

        combatStarted = false;
        currentTurn = 0;
        currentPiece = null;
        turnSequence.Clear();
        combatLog.Clear();
    }

    public void BootCombat(PartyPiece2 attackerParty, PartyPiece2 defenderParty, DB_Tileset tileset)
    {
        pieceHandler.Remove();
        result = CombatResult.NOT_FINISHED;

        attackerPlayer = attackerParty.pieceOwner.GetOwner();
        this.attackerParty = attackerParty;
        defenderPlayer = defenderParty.pieceOwner.GetOwner();
        this.defenderParty = defenderParty;
        this.tileset = tileset;

        Debug.LogWarning("No tile data for combat map!");
        mapHandler.BuildMap(MAP_SIZE, tileset);
        mapHandler.AddRandomObstacles(tileset);
        pieceHandler.Create(attackerParty, defenderParty);
        pieceHandler.InitialPositions(mapHandler.map);

        combatStarted = true;
        NextTurn();
    }

    public bool IsCombatRunning()
    {
        return result == CombatResult.NOT_FINISHED;
    }

    public void NextUnit()
    {
        if (CheckBattleEnd()) return;

        if (turnSequence.Count > 0 || waitSequence.Count > 0)
        {
            List<AbstractCombatActorPiece2> list = null;
            if (turnSequence.Count > 0) list = turnSequence;
            else if (waitSequence.Count > 0) list = waitSequence;

            currentPiece = list[0];
            Vector3 pos = currentPiece.transform.position;

            CombatSceneInputs csi = CombatSceneInputs.Instance;
            csi.executor.selectionPos = new Vector2Int((int)pos.x, (int)pos.z);
            csi.executor.selectionTile = currentPiece.currentTile as CombatTile;
            csi.executor.selectionPiece = currentPiece;

            //csi.executor.selectionHighlight.transform.position = currentPiece.transform.position;

            csi.executor.canCommandSelectedPiece = currentPiece.pieceOwner.GetOwner() == PlayerManager.Instance.localPlayer;

            list.RemoveAt(0);
            CombatUI.Instance.turnSequence.RemoveFirstFromTurnSequence();
            CombatSceneHighlights.Instance.Refresh();

            Player owner = currentPiece.pieceOwner.GetOwner();
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
            foreach (AbstractCombatActorPiece2 cup in turnSequence) cup.ISTET_StartTurn();

            NextUnit();
            currentTurn++;
        }
    }

    public bool CalculateFullTurnSequence()
    {
        List<AbstractCombatActorPiece2> newSequence = new List<AbstractCombatActorPiece2>();
        newSequence.AddRange(pieceHandler.GetActivePieces(pieceHandler.attackerPieces));
        newSequence.AddRange(pieceHandler.GetActivePieces(pieceHandler.defenderPieces));
        if (newSequence.Count <= 0) return false;

        turnSequence = newSequence;
        waitSequence = new List<AbstractCombatActorPiece2>();
        UpdateTurnSequence();
        return true;
    }

    public void UpdateTurnSequence()
    {
        turnSequence = turnSequence.OrderBy(a => a.spawnId).ToList();
        turnSequence = turnSequence.OrderByDescending(a => a.combatPieceStats.initiative).ToList();
        CombatUI.Instance.turnSequence.CreateTurnSequence(turnSequence, waitSequence);
    }

    public void AddUnitToTurnSequence(AbstractCombatActorPiece2 uc)
    {
        turnSequence.Add(uc);
        UpdateTurnSequence();
    }

    public void RemoveUnitFromTurnSequence(AbstractCombatActorPiece2 uc)
    {
        turnSequence.Remove(uc);
        UpdateTurnSequence();
    }

    public void UpdateWaitSequence()
    {
        waitSequence = waitSequence.OrderBy(a => a.spawnId).ToList();
        waitSequence = waitSequence.OrderBy(a => a.combatPieceStats.initiative).ToList();
        CombatUI.Instance.turnSequence.CreateTurnSequence(turnSequence, waitSequence);
    }

    public void AddUnitToWaitSequence(AbstractCombatActorPiece2 uc)
    {
        waitSequence.Add(uc);
        UpdateWaitSequence();
    }

    public void RemoveUnitFromWaitSequence(AbstractCombatActorPiece2 uc)
    {
        waitSequence.Remove(uc);
        UpdateTurnSequence();
    }

    public void CombatEnd(CombatResult result)
    {
        GameManager.Instance.PauseUnpause(false);
        CombatUI.Instance.EscapeMenuHide();

        this.result = result;
        Player localPlayer = PlayerManager.Instance.localPlayer;

        string resultMsg;
        if ((result == CombatResult.ATTACKER_WON) && (attackerPlayer == localPlayer) ||
            (result == CombatResult.DEFENDER_WON) && (defenderPlayer == localPlayer))
        {
            resultMsg = localPlayerVictoryMsg;
        }
        else
        {
            resultMsg = localPlayerDefeatMsg;
        }
        CombatUI.Instance.ResultPopupShow(resultMsg);
    }

    public void CombatEndForceVictory()
    {
        Player localPlayer = PlayerManager.Instance.localPlayer;
        if (attackerPlayer == localPlayer)
        {
            List<AbstractCombatActorPiece2> piecesToKill = pieceHandler.defenderPieces;
            pieceHandler.KillEmAll(piecesToKill);
            CombatEnd(CombatResult.ATTACKER_WON);
        }
        else if (defenderPlayer == localPlayer)
        {
            List<AbstractCombatActorPiece2> piecesToKill = pieceHandler.attackerPieces;
            pieceHandler.KillEmAll(piecesToKill);
            CombatEnd(CombatResult.DEFENDER_WON);
        }
    }

    public void CombatEndForceDefeat()
    {
        Player localPlayer = PlayerManager.Instance.localPlayer;
        if (attackerPlayer == localPlayer)
        {
            List<AbstractCombatActorPiece2> piecesToKill = pieceHandler.attackerPieces;
            pieceHandler.KillEmAll(piecesToKill);
            CombatEnd(CombatResult.DEFENDER_WON);
        }
        else if (defenderPlayer == localPlayer)
        {
            List<AbstractCombatActorPiece2> piecesToKill = pieceHandler.defenderPieces;
            pieceHandler.KillEmAll(piecesToKill);
            CombatEnd(CombatResult.ATTACKER_WON);
        }
    }

    public void CombatEndConfirm()
    {
        CombatUI.Instance.ResultPopupHide();
        TerminateCombat();
        ReturnFromCombat(result, attackerParty, defenderParty);
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

    /*
     * Begin: UI Top Left buttons
     */
    public void EscapeMenu()
    {
        bool isPaused = GameManager.Instance.isPaused;
        AbstractUIPanel currentWindow = CombatUI.Instance.currentWindow;
        if (!isPaused && currentWindow)
        {
            CombatUI.Instance.CloseCurrentWindow();
            return;
        }

        bool pauseStatus = GameManager.Instance.PauseUnpause();
        if (pauseStatus) CombatUI.Instance.EscapeMenuShow();
        else CombatUI.Instance.EscapeMenuHide();
    }

    public void Restart()
    {
        CombatUI.Instance.EscapeMenuHide();

        TerminateCombat();
        BootCombat(attackerParty, defenderParty, tileset);

        GameManager.Instance.PauseUnpause();
    }
    /*
     * End: UI Top Left buttons
     */

    /*
     * Begin: UI Bottom Center buttons
     */
    public void Selection_Wait()
    {
        CombatSceneInputs.Instance.executor.MakeSelectedPieceWait();
    }

    public void Selection_Defend()
    {
        CombatSceneInputs.Instance.executor.MakeSelectedPieceDefend();
    }
    /*
    * End: UI Bottom Center buttons
    */

    /*
     * Begin: UI Windows
     */
    public void CombatActorInspector()
    {
        if (CombatUI.Instance.currentWindow == CombatUI.Instance.combatActorInspector) CombatUI.Instance.CombatActorInspectorHide();
        else if (CombatUI.Instance.currentWindow == null) CombatUI.Instance.CombatActorInspectorShow(currentPiece);
    }
    /*
     * End: UI Windows
     */

    private void ReturnFromCombat(CombatResult result, PartyPiece2 attacker, PartyPiece2 defender)
    {
        //CombatSceneInputs.Instance.executor.RemoveMoveAreaHighlights();
        //CombatSceneInputs.Instance.executor.RemoveMovePathHighlights();
        CombatSC.Instance.HideScene();

        //TODO consider an 'redo combat' feature.
        ApplyCombatResults(out int attackerExperience, out int defenderExperience);
        GameManager.Instance.ChangeSchemes(GameScheme.FIELD);

        FieldSC.Instance.ShowScene();

        PartyPiece2 victor = null;
        int expBounty = 0;
        PartyPiece2 defeated = null;

        switch (result)
        {
            case CombatResult.ATTACKER_WON:
                victor = attacker;
                expBounty = attackerExperience;
                defeated = defender;
                break;
            case CombatResult.DEFENDER_WON:
                victor = defender;
                expBounty = defenderExperience;
                defeated = attacker;
                break;
        }
        FieldManager.Instance.RemoveParty(defeated);

        //TODO move this apply experience/levelup thing to the FieldManager
        bool hasLevelUp = victor.ApplyExperience(expBounty);
        //while (hasLevelUp)
        //{
        //TODO don't use this while
        if (hasLevelUp)
        {
            Hero hero = victor.party.GetHeroSlot().Get() as Hero;
            FieldUI fui = FieldUI.Instance;
            fui.levelUp.UpdatePanel(hero);
            fui.LevelUpShow(hero);
            //hasLevelUp = victor.ApplyExperience();
        }
        //}

        //Combat scene cleanups
        pieceHandler.Remove();

        FieldSceneInputs.Instance.executor.MakeSelectedPieceInteract(false);
        //FieldInputs.Instance.ResetHighlights();
    }

    private void ApplyCombatResults(out int attackerExperience, out int defenderExperience)
    {
        attackerExperience = 0;
        defenderExperience = 0;

        if (result != CombatResult.DEFENDER_WON)
        {
            attackerExperience = ExperienceCalculation.FullExperienceCalculation(pieceHandler.defenderPieces);
            ApplyCombatResults(attackerParty, pieceHandler.attackerPieces);
        }
        if (result != CombatResult.ATTACKER_WON)
        {
            defenderExperience = ExperienceCalculation.FullExperienceCalculation(pieceHandler.attackerPieces);
            ApplyCombatResults(defenderParty, pieceHandler.defenderPieces);
        }
    }

    private void ApplyCombatResults(PartyPiece2 party, List<AbstractCombatActorPiece2> pieces)
    {
        foreach (var piece in pieces)
        {
            CombatantHeroPiece2 asHero = piece as CombatantHeroPiece2;
            CombatantUnitPiece2 asUnit = piece as CombatantUnitPiece2;

            if (asHero)
            {
                Hero hero = asHero.hero;
                if (asHero.combatPieceStats.hitPoints_current <= 0)
                {
                    party.party.Remove(hero);
                    Destroy(hero.gameObject);
                }
            }

            if (asUnit)
            {
                Unit unit = asUnit.unit;
                unit.stackStats.stack_maximum = asUnit.stackStats.stack_current;
                if (unit.stackStats.stack_maximum <= 0)
                {
                    party.party.Remove(unit);
                    Destroy(unit.gameObject);
                }
            }
        }
    }
}
