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

    public void BootField(Vector2Int scenarioSize, MapData map, List<TownData> towns, List<PartyData> parties, List<PickupData> pickups)
    {
        mapHandler.BuildMap(scenarioSize, map);
        pieceHandler.CreateAll(towns, parties, pickups);
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

    public void PartyInteraction(PartyPiece2 party, TownPiece2 town)
    {
        if (party.pieceOwner.GetOwner() == town.pieceOwner.GetOwner())
        {
            StartCoroutine(GoToTown(party, town));
        }
        else
        {
            //StartCoroutine(GoToCombat(party, target));    //TODO TOWN SIEGE
        }
    }

    public void PartyInteraction(PartyPiece2 party, PartyPiece2 defender)
    {
        if (party.pieceOwner.GetOwner() == defender.pieceOwner.GetOwner())
        {
            PerformExchange(party, defender);
        }
        else
        {
            StartCoroutine(GoToCombat(party, defender));
        }
    }

    public void PartyInteraction(PartyPiece2 party, PickupPiece2 pickup)
    {
        switch (pickup.pickupType)
        {
            case PickupType.RESOURCE:
                Debug.LogWarning("Resource pickup is not supported");
                break;
            case PickupType.ARTIFACT:
                Artifact prefab = AllPrefabs.Instance.artifact;
                Artifact artifact = Instantiate(prefab, transform);
                artifact.Initialize(pickup.dbArtifact);
                party.party.hero.inventory.AddArtifact(artifact);
                break;
            case PickupType.UNIT:
                Debug.LogWarning("Unit pickup is not supported");
                break;
            default:
                break;
        }
        RemovePickup(pickup);
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

        List<PartyPiece2> playerFieldPieces = pieceHandler.GetPlayerPieces(currentPlayer);
        yield return StartCoroutine(pieceHandler.YieldForIdlePieces(playerFieldPieces));

        Player next = pm.EndTurnForPlayer(currentPlayer);
        if (!next) NextTurnForAll();
        else currentPlayer = next;

        if (currentPlayer == pm.localPlayer)
        {
            FieldUI.Instance.timers.UnlockButtons();
            FieldSceneHighlights.Instance.Refresh();
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

    private IEnumerator GoToTown(PartyPiece2 visitor, TownPiece2 town)
    {
        List<PartyPiece2> pieces = new List<PartyPiece2> { visitor };
        yield return
            StartCoroutine(pieceHandler.YieldForIdlePieces(pieces));

        town.visitor = visitor;

        FieldSC.Instance.HideScene();
        GameManager.Instance.ChangeSchemes(GameScheme.TOWN);
        TownManager.Instance.BootTown(town);
        TownSC.Instance.ShowScene();
    }

    private IEnumerator GoToCombat(PartyPiece2 attacker, PartyPiece2 defender)
    {
        List<PartyPiece2> pieces = new List<PartyPiece2> { attacker, defender };
        yield return
            StartCoroutine(pieceHandler.YieldForIdlePieces(pieces));

        FieldTile fieldTile = defender.currentTile as FieldTile;

        FieldSC.Instance.HideScene();
        GameManager.Instance.ChangeSchemes(GameScheme.COMBAT);
        CombatManager.Instance.BootCombat(attacker, defender, fieldTile.db_tileset_lowerLand);
        CombatSC.Instance.ShowScene();
    }
}
