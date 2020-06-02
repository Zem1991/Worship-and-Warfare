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
    public CombatantPiece3 currentPiece;
    public List<CombatantPiece3> turnSequence = new List<CombatantPiece3>();
    public List<CombatantPiece3> waitSequence = new List<CombatantPiece3>();
    public List<string> combatLog = new List<string>();
    public CombatResult result;

    [Header("Teams")]
    public Player attackerPlayer;
    public IPieceForCombat attackerPiece;
    public Player defenderPlayer;
    public IPieceForCombat defenderPiece;

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

    public void BootCombat(IPieceForCombat attackerPiece, IPieceForCombat defenderPiece, DB_Tileset tileset)
    {
        //TODO: put these 2 lines somewhere else?
        pieceHandler.Remove();
        result = CombatResult.NOT_FINISHED;

        attackerPlayer = attackerPiece.IPFC_GetPlayerForCombat();
        this.attackerPiece = attackerPiece;
        defenderPlayer = defenderPiece.IPFC_GetPlayerForCombat();
        this.defenderPiece = defenderPiece;

        Party attackerParty = attackerPiece.IPFC_GetPartyForCombat();
        Party defenderParty = defenderPiece.IPFC_GetPartyForCombat();
        Town defenderTown = defenderPiece.IPFC_GetPTownForCombat();

        BootMap(tileset, defenderTown);
        BootPieces(attackerPlayer, attackerParty, defenderPlayer, defenderParty, defenderTown);
        NextTurn();
        combatStarted = true;
    }

    private void BootMap(DB_Tileset tileset, Town defenderTown)
    {
        this.tileset = tileset;

        Debug.LogWarning("No tile data for combat map!");
        mapHandler.BuildMap(MAP_SIZE, tileset);
        if (!defenderTown) mapHandler.AddRandomObstacles(tileset);
    }

    private void BootPieces(Player attackerPlayer, Party attackerParty, Player defenderPlayer, Party defenderParty, Town defenderTown)
    {
        pieceHandler.Create(attackerPlayer, attackerParty, defenderPlayer, defenderParty, defenderTown);
        pieceHandler.InitialUnitPositions();
        if (defenderTown) pieceHandler.InitialTownDefensePositions();
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
            List<CombatantPiece3> list = null;
            if (turnSequence.Count > 0) list = turnSequence;
            else if (waitSequence.Count > 0) list = waitSequence;

            currentPiece = list[0];
            Vector3 pos = currentPiece.transform.position;

            CombatSceneInputs csi = CombatSceneInputs.Instance;
            csi.executor.selectionPos = new Vector2Int((int)pos.x, (int)pos.z);
            csi.executor.selectionTile = currentPiece.currentTile as CombatTile;
            csi.executor.selectionPiece = currentPiece;

            //csi.executor.selectionHighlight.transform.position = currentPiece.transform.position;

            csi.executor.canCommandSelectedPiece = currentPiece.pieceOwner.Get() == PlayerManager.Instance.localPlayer;

            list.RemoveAt(0);
            CombatUI.Instance.turnSequence.RemoveFirstFromTurnSequence();
            CombatSceneHighlights.Instance.Refresh();

            Player owner = currentPiece.pieceOwner.Get();
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
            foreach (CombatantPiece3 cup in turnSequence) cup.ISTET_StartTurn();

            NextUnit();
            currentTurn++;
        }
    }

    public bool CalculateFullTurnSequence()
    {
        List<CombatantPiece3> newSequence = new List<CombatantPiece3>();
        newSequence.AddRange(pieceHandler.GetActivePieces(pieceHandler.attackerPieces));
        newSequence.AddRange(pieceHandler.GetActivePieces(pieceHandler.defenderPieces));
        if (newSequence.Count <= 0) return false;

        turnSequence = newSequence;
        waitSequence = new List<CombatantPiece3>();
        UpdateTurnSequence();
        return true;
    }

    public void UpdateTurnSequence()
    {
        turnSequence = turnSequence.OrderBy(a => a.spawnId).ToList();
        turnSequence = turnSequence.OrderByDescending(a => a.movementStats.initiative).ToList();
        CombatUI.Instance.turnSequence.CreateTurnSequence(turnSequence, waitSequence);
    }

    public void AddUnitToTurnSequence(CombatantPiece3 uc)
    {
        turnSequence.Add(uc);
        UpdateTurnSequence();
    }

    public void RemoveUnitFromTurnSequence(CombatantPiece3 uc)
    {
        turnSequence.Remove(uc);
        UpdateTurnSequence();
    }

    public void UpdateWaitSequence()
    {
        waitSequence = waitSequence.OrderBy(a => a.spawnId).ToList();
        waitSequence = waitSequence.OrderBy(a => a.movementStats.initiative).ToList();
        CombatUI.Instance.turnSequence.CreateTurnSequence(turnSequence, waitSequence);
    }

    public void AddUnitToWaitSequence(CombatantPiece3 uc)
    {
        waitSequence.Add(uc);
        UpdateWaitSequence();
    }

    public void RemoveUnitFromWaitSequence(CombatantPiece3 uc)
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
            List<CombatantPiece3> piecesToKill = pieceHandler.defenderPieces;
            pieceHandler.KillEmAll(piecesToKill);
            CombatEnd(CombatResult.ATTACKER_WON);
        }
        else if (defenderPlayer == localPlayer)
        {
            List<CombatantPiece3> piecesToKill = pieceHandler.attackerPieces;
            pieceHandler.KillEmAll(piecesToKill);
            CombatEnd(CombatResult.DEFENDER_WON);
        }
    }

    public void CombatEndForceDefeat()
    {
        Player localPlayer = PlayerManager.Instance.localPlayer;
        if (attackerPlayer == localPlayer)
        {
            List<CombatantPiece3> piecesToKill = pieceHandler.attackerPieces;
            pieceHandler.KillEmAll(piecesToKill);
            CombatEnd(CombatResult.DEFENDER_WON);
        }
        else if (defenderPlayer == localPlayer)
        {
            List<CombatantPiece3> piecesToKill = pieceHandler.defenderPieces;
            pieceHandler.KillEmAll(piecesToKill);
            CombatEnd(CombatResult.ATTACKER_WON);
        }
    }

    public void CombatEndConfirm()
    {
        CombatUI.Instance.ResultPopupHide();
        TerminateCombat();
        ReturnFromCombat(result, attackerPiece, defenderPiece);
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
        BootCombat(attackerPiece, defenderPiece, tileset);

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
        CombatSceneInputs csi = CombatSceneInputs.Instance;
        if (csi.executor.canCommandSelectedPiece) CombatSceneInputs.Instance.executor.MakeSelectedPieceWait();
    }
    public void Selection_Defend()
    {
        CombatSceneInputs csi = CombatSceneInputs.Instance;
        if (csi.executor.canCommandSelectedPiece) CombatSceneInputs.Instance.executor.MakeSelectedPieceDefend();
    }
    public void Selection_Ability(int id)
    {
        CombatSceneInputs csi = CombatSceneInputs.Instance;
        if (csi.executor.canCommandSelectedPiece) CombatSceneInputs.Instance.executor.MakeSelectedPieceUseAbility(id);
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

    private void ReturnFromCombat(CombatResult result, IPieceForCombat attacker, IPieceForCombat defender)
    {
        //CombatSceneInputs.Instance.executor.RemoveMoveAreaHighlights();
        //CombatSceneInputs.Instance.executor.RemoveMovePathHighlights();
        CombatSC.Instance.HideScene();

        //TODO consider an 'redo combat' feature.
        ApplyCombatResults(out int attackerExperience, out int defenderExperience);
        GameManager.Instance.ChangeSchemes(GameScheme.FIELD);

        FieldSC.Instance.ShowScene();

        IPieceForCombat victor = null;
        IPieceForCombat defeated = null;
        int expBounty = 0;

        switch (result)
        {
            case CombatResult.ATTACKER_WON:
                victor = attacker;
                defeated = defender;
                expBounty = attackerExperience;
                break;
            case CombatResult.DEFENDER_WON:
                victor = defender;
                defeated = attacker;
                expBounty = defenderExperience;
                break;
        }
        FieldManager.Instance.RemoveParty(defeated);

        TownPiece3 defeatedAsTown = defeated as TownPiece3;
        if (defeatedAsTown) FieldManager.Instance.ChangeTownOwner(defeatedAsTown, attacker.IPFC_GetPlayerForCombat());

        //TODO move this apply experience/levelup thing to the FieldManager
        bool hasLevelUp = victor.IPFC_GetPartyForCombat().GiveExperiencePoints(expBounty);
        //while (hasLevelUp)
        //{
        //TODO don't use this while
        if (hasLevelUp)
        {
            HeroUnit hero = victor.IPFC_GetPartyForCombat().GetHeroSlot().Get() as HeroUnit;
            if (hero)
            {
                FieldUI fui = FieldUI.Instance;
                fui.levelUp.UpdatePanel(hero);
                fui.LevelUpShow(hero);
                //hasLevelUp = victor.ApplyExperience();
            }
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
            ApplyCombatResults(attackerPiece.IPFC_GetPartyForCombat(), pieceHandler.attackerPieces);
        }
        if (result != CombatResult.ATTACKER_WON)
        {
            defenderExperience = ExperienceCalculation.FullExperienceCalculation(pieceHandler.attackerPieces);
            ApplyCombatResults(defenderPiece.IPFC_GetPartyForCombat(), pieceHandler.defenderPieces);
        }
    }

    private void ApplyCombatResults(Party party, List<CombatantPiece3> pieces)
    {
        foreach (var piece in pieces)
        {
            HeroUnitPiece3 asHero = piece as HeroUnitPiece3;
            CombatUnitPiece3 asUnit = piece as CombatUnitPiece3;

            if (asHero)
            {
                HeroUnit hero = asHero.GetHeroUnit();
                if (asHero.healthStats.hitPoints_current <= 0)
                {
                    party.Remove(hero);
                    Destroy(hero.gameObject);
                }
            }

            if (asUnit)
            {
                CombatUnit unit = asUnit.GetCombatUnit();
                int amount = unit.GetStackHealthStats().GetStackSize(); //TPDP: - asUnit.stackStats.Get();
                if (unit.GetStackHealthStats().SubtractFromStack(amount))
                {
                    party.Remove(unit);
                    Destroy(unit.gameObject);
                }
            }
        }
    }
}
