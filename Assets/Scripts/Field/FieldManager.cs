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

    public void RemoveParty(PartyPiece2 piece)
    {
        pieceHandler.RemoveParty(piece);
    }

    public void RemovePickup(AbstractPickupPiece2 pickup)
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
            TradeScreen(party, defender);
        }
        else
        {
            StartCoroutine(GoToCombat(party, defender));
        }
    }

    public void PartyInteraction(PartyPiece2 party, AbstractPickupPiece2 pickup)
    {
        switch (pickup.pickupType)
        {
            case PickupType.RESOURCE:
                ResourcePickupPiece2 resourcePickup = pickup as ResourcePickupPiece2;
                Player owner = party.pieceOwner.GetOwner();
                ResourceStats resources = owner.resourceStats;
                long amount = resourcePickup.resourceAmount;
                switch (resourcePickup.dbResource.resourceType)
                {
                    case ResourceType.GOLD:
                        resources.gold += amount;
                        break;
                    case ResourceType.ORE:
                        resources.ore += amount;
                        break;
                    case ResourceType.ALE:
                        resources.ale += amount;
                        break;
                    case ResourceType.CRYSTALS:
                        resources.crystals += amount;
                        break;
                    case ResourceType.SULPHUR:
                        resources.sulphur += amount;
                        break;
                    default:
                        Debug.LogError("Resource not found: " + resourcePickup.dbResource.resourceType);
                        break;
                }
                break;
            case PickupType.ARTIFACT:
                ArtifactPickupPiece2 artifactPickup = pickup as ArtifactPickupPiece2;
                Artifact prefab = AllPrefabs.Instance.artifact;
                Artifact artifact = Instantiate(prefab, transform);
                artifact.Initialize(artifactPickup.dbArtifact);
                Hero hero = party.party.hero.GetSlotObject() as Hero;
                hero.inventory.AddFromPickup(artifact);
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
        AbstractUIPanel currentWindow = FieldUI.Instance.currentWindow;
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
    public void Selection_EnterTown()
    {
        TownPiece2 selectionPiece = FieldSceneInputs.Instance.executor.selectionPiece as TownPiece2;
        bool canCommandSelectedPiece = FieldSceneInputs.Instance.executor.canCommandSelectedPiece;

        if (selectionPiece && canCommandSelectedPiece)
        {
            TownPiece2 town = FieldSceneInputs.Instance.executor.selectionPiece as TownPiece2;
            StartCoroutine(GoToTown(null, town));
        }
    }

    public void Selection_Movement()
    {
        FieldSceneInputs.Instance.executor.MakeSelectedPieceInteract(false);
    }

    public void Selection_Inventory()
    {
        PartyPiece2 selectionPiece = FieldSceneInputs.Instance.executor.selectionPiece as PartyPiece2;
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

    private void TradeScreen(PartyPiece2 sender, PartyPiece2 receiver)
    {
        FieldUI.Instance.TradeScreenShow(sender, receiver);
    }

    private IEnumerator GoToTown(PartyPiece2 visitor, TownPiece2 townPiece)
    {
        List<PartyPiece2> pieces = new List<PartyPiece2>();
        if (visitor)
        {
            pieces.Add(visitor);
            townPiece.visitorPiece = visitor;
        }

        yield return
            StartCoroutine(pieceHandler.YieldForIdlePieces(pieces));

        FieldSC.Instance.HideScene();
        GameManager.Instance.ChangeSchemes(GameScheme.TOWN);
        TownManager.Instance.BootTown(townPiece);
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
