using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : AbstractSingleton<FieldManager>, IShowableHideable
{
    [Header("Auxiliary objects")]
    public FieldMapHandler mapHandler;
    public FieldPieceHandler pieceHandler;

    [Header("Field flow")]
    public int currentTurn;
    public Player currentPlayer;

    [Header("Day/Week/Month")]
    public int day;
    public int week;
    public int month;

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

    public void TerminateField()
    {
        mapHandler.ClearMap();
        pieceHandler.RemoveAll();
    }

    public void BootField(Vector2Int scenarioSize, MapData map, List<PartyData> parties, List<PickupData> pickups)
    {
        mapHandler.BuildMap(scenarioSize, map);
        pieceHandler.CreateAll(parties, pickups);
    }

    public void RemovePiece(PartyPiece2 piece)
    {
        pieceHandler.RemovePiece(piece);
    }

    public void RemovePickup(PickupPiece2 pickup)
    {
        pieceHandler.RemovePickup(pickup);
    }

    public void NextTurnForAll()
    {
        currentTurn++;

        PlayerManager.Instance.RefreshTurnForActivePlayers(currentTurn);
        currentPlayer = PlayerManager.Instance.activePlayers[0];

        NextDayWeekMonth();

        foreach (var item in pieceHandler.partyPieces)
        {
            item.ISTET_StartTurn();
        }
    }

    public void PartiesAreInteracting(PartyPiece2 sender, PartyPiece2 receiver)
    {
        if (sender.pieceOwner.GetOwner() == receiver.pieceOwner.GetOwner())
        {
            PerformExchange(sender, receiver);
        }
        else
        {
            StartCoroutine(GoToCombat(sender, receiver));
        }
    }

    public void PartyFoundPickup(PartyPiece2 partyPiece2, PickupPiece2 targetPickup)
    {
        switch (targetPickup.pickupType)
        {
            case PickupType.RESOURCE:
                Debug.LogWarning("Resource pickup is not supported");
                break;
            case PickupType.ARTIFACT:
                Artifact prefab = AllPrefabs.Instance.artifact;
                Artifact artifact = Instantiate(prefab, transform);
                artifact.Initialize(targetPickup.dbArtifact);
                partyPiece2.partyHero.inventory.AddArtifact(artifact);
                break;
            case PickupType.UNIT:
                Debug.LogWarning("Unit pickup is not supported");
                break;
            default:
                break;
        }
        RemovePickup(targetPickup);
        //yield return null;
    }

    /*
     * Begin: UI Top Left buttons
     */
    public void EscapeMenu()
    {
        bool isPaused = GameManager.Instance.isPaused;
        AUIPanel currentWindow = FieldUI.Instance.currentWindow;
        if (!isPaused && currentWindow)
        {
            FieldUI.Instance.CloseCurrentWindow();
            return;
        }

        bool pauseStatus = GameManager.Instance.PauseUnpause();
        if (pauseStatus) FieldUI.Instance.EscapeMenuShow();
        else FieldUI.Instance.EscapeMenuHide();
    }

    public void Restart()
    {
        FieldSC.Instance.HideScene();
        TerminateField();
        StartCoroutine(GameManager.Instance.LoadScenarioFile());
    }

    public void QuitToMaimMenu()
    {
        FieldSC.Instance.HideScene();
        TerminateField();
        Main.Instance.ReturnToMain();
    }
    /*
     * End: UI Top Left buttons
     */

    /*
     * Begin: UI Top Right buttons
     */
    public void EndTurn()
    {
        FieldUI.Instance.timers.LockButtons();
        StartCoroutine(EndTurnForCurrentPlayer());
    }
    /*
     * End: UI Top Right buttons
     */

    /*
     * Begin: UI Bottom Right buttons
     */
    public void Selection_Movement()
    {
        FieldSceneInputs.Instance.executor.MakeSelectedPieceInteract(false);
    }

    public void Selection_Inventory()
    {
        AbstractFieldPiece2 selectionPiece = FieldSceneInputs.Instance.executor.selectionPiece;
        bool canCommandSelectedPiece = FieldSceneInputs.Instance.executor.canCommandSelectedPiece;

        if (selectionPiece && canCommandSelectedPiece)
        {
            if (FieldUI.Instance.currentWindow == FieldUI.Instance.inventory) FieldUI.Instance.InventoryHide();
            else if (FieldUI.Instance.currentWindow == null) FieldUI.Instance.InventoryShow(selectionPiece);
        }
    }
    /*
    * End: UI Bottom Right buttons
    */

    private void NextDayWeekMonth()
    {
        int turnsAdjusted = currentTurn - 1;
        month = (turnsAdjusted / 28) + 1;
        int monthDayDif = turnsAdjusted % 28;
        week = (monthDayDif / 7) + 1;
        int weekDayDif = monthDayDif % 7;
        day = (weekDayDif % 7) + 1;
    }

    private IEnumerator EndTurnForCurrentPlayer()
    {
        PlayerManager pm = PlayerManager.Instance;
        FieldPieceHandler fPH = FieldManager.Instance.pieceHandler;
        //FieldInputExecutor fInputs = FieldSceneInputs.Instance.executor;

        List<PartyPiece2> playerFieldPieces = fPH.GetPlayerPieces(currentPlayer);
        yield return StartCoroutine(fPH.YieldForIdlePieces(playerFieldPieces));

        Player next = pm.EndTurnForPlayer(currentPlayer);
        if (!next) NextTurnForAll();
        else currentPlayer = next;

        if (currentPlayer == pm.localPlayer)
        {
            FieldUI.Instance.timers.UnlockButtons();
            //fInputs.ResetHighlights();
        }
        else if (currentPlayer.type == PlayerType.COMPUTER)
        {
            currentPlayer.aiPersonality.FieldRoutine();
        }
    }

    private void PerformExchange(AbstractFieldPiece2 sender, AbstractFieldPiece2 receiver)
    {
        Debug.Log("PIECES ARE EXCHANGING STUFF");
        //yield return null;
    }

    private IEnumerator GoToCombat(PartyPiece2 attacker, PartyPiece2 defender)
    {
        List<PartyPiece2> pieces = new List<PartyPiece2> { attacker, defender };
        yield return
            StartCoroutine(pieceHandler.YieldForIdlePieces(pieces));

        //FieldSceneInputs.Instance.executor.RemoveMoveAreaHighlights();
        //FieldSceneInputs.Instance.executor.RemoveMovePathHighlights();
        FieldSC.Instance.HideScene();

        Debug.Log("PIECES ARE IN BATTLE");
        GameManager.Instance.ChangeSchemes(GameScheme.COMBAT);

        FieldTile fieldTile = defender.currentTile as FieldTile;
        CombatManager.Instance.BootCombat(attacker, defender, fieldTile.db_tileset_lowerLand);

        CombatSC.Instance.ShowScene();
    }
}
