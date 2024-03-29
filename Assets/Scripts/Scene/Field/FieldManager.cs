﻿using System;
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

    public void RemoveParty(IPieceForCombat piece)
    {
        PartyPiece3 asParty = piece as PartyPiece3;
        TownPiece3 asTown = piece as TownPiece3;

        if (asParty) pieceHandler.RemovePartyPiece(asParty);
        if (asTown) asTown.IPFC_GetPartyForCombat().ClearParty();
    }

    public void ChangeTownOwner(TownPiece3 town, Player player)
    {
        town.pieceOwner.Set(player);
        town.pieceController.Set(player);
        town.IFP_SetFlagSprite(player.dbColor.imgFlag);
    }

    public void RemovePickup(PickupPiece3 pickup)
    {
        pieceHandler.RemovePickup(pickup);
    }

    public void NextTurnForAll()
    {
        PlayerManager pm = PlayerManager.Instance;

        currentTurn++;
        pm.RefreshTurnForActivePlayers(currentTurn);

        NextDayWeekMonth();
        foreach (var item in pieceHandler.partyPieces)
        {
            item.ISTET_StartTurn();
        }

        currentPlayer = pm.activePlayers[0];
        StartTurnForCurrentPlayer();
    }

    public IEnumerator PartyInteraction(PartyPiece3 party, TownPiece3 town)
    {
        if (party.pieceOwner.Get() != town.pieceOwner.Get())
        {
            if (town.IPFC_GetPartyForCombat().GetMostRelevant())
            {
                yield return
                    StartCoroutine(GoToCombat(party, town));
            }
            else
            {
                Player player = party.pieceOwner.Get();
                ChangeTownOwner(town, player);
            }
        }

        bool canEnter = party.pieceOwner.Get() == town.pieceOwner.Get();
        if (canEnter) StartCoroutine(GoToTown(party, town));
    }

    public IEnumerator PartyInteraction(PartyPiece3 party, PartyPiece3 defender)
    {
        if (party.pieceOwner.Get() == defender.pieceOwner.Get())
        {
            TradeScreen(party, defender);
        }
        else
        {
            yield return StartCoroutine(GoToCombat(party, defender));
        }
    }

    public IEnumerator PartyInteraction(PartyPiece3 party, PickupPiece3 pickup)
    {
        switch (pickup.pickupType)
        {
            case PickupType.RESOURCE:
                Player owner = party.pieceOwner.Get();
                ResourceStats2 resources = owner.currentResources;
                long amount = pickup.resourceAmount;
                switch (pickup.dbResource.resourceType)
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
                    case ResourceType.SULFUR:
                        resources.sulfur += amount;
                        break;
                    default:
                        Debug.LogError("Resource not found: " + pickup.dbResource.resourceType);
                        break;
                }
                break;
            case PickupType.ARTIFACT:
                PickupPiece3 artifactPickup = pickup as PickupPiece3;
                Artifact prefab = AllPrefabs.Instance.artifact;
                Artifact artifact = Instantiate(prefab, transform);
                artifact.Initialize(artifactPickup.dbArtifact);
                HeroUnit hero = party.IPFC_GetPartyForCombat().GetHeroSlot().Get() as HeroUnit;
                hero.inventory.Add(artifact);
                break;
            default:
                break;
        }
        RemovePickup(pickup);
        yield return null;
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
        StartCoroutine(NextPlayerTurn());
    }
    /*
     * End: UI Top Right buttons
     */

    /*
     * Begin: UI Bottom Right buttons
     */
    public void Selection_EnterTown()
    {
        FieldInputExecutor fie = FieldSceneInputs.Instance.executor;
        TownPiece3 selectionPiece = fie.selectionPiece as TownPiece3;
        bool canCommandSelectedPiece = fie.canCommandSelectedPiece;

        if (selectionPiece && canCommandSelectedPiece)
        {
            TownPiece3 town = selectionPiece;
            StartCoroutine(GoToTown(null, town));
        }
    }

    public void Selection_Movement()
    {
        FieldSceneInputs.Instance.executor.MakeSelectedPieceInteract(false);
    }

    public void Selection_Inventory()
    {
        PartyPiece3 selectionPiece = FieldSceneInputs.Instance.executor.selectionPiece as PartyPiece3;
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

    private IEnumerator NextPlayerTurn()
    {
        List<PartyPiece3> playerFieldPieces = pieceHandler.GetPlayerPieces(currentPlayer);
        yield return StartCoroutine(pieceHandler.YieldForIdlePieces(playerFieldPieces));

        PlayerManager pm = PlayerManager.Instance;
        Player next = pm.EndTurnForPlayer(currentPlayer);
        if (next)
        {
            currentPlayer = next;
            StartTurnForCurrentPlayer();
        }
        else
        {
            NextTurnForAll();
        }
    }

    private void StartTurnForCurrentPlayer()
    {
        PlayerManager pm = PlayerManager.Instance;
        if (currentPlayer == pm.localPlayer)
        {
            FieldUI.Instance.timers.UnlockButtons();
            FieldSceneHighlights.Instance.Refresh();

            Vector3 position = currentPlayer.GetStartingPosition();
            FieldSceneInputs.Instance.GetCameraController().PlaceCamera(position);
        }
        else if (currentPlayer.type == PlayerType.COMPUTER)
        {
            currentPlayer.aiPersonality.FieldRoutine();
        }
    }

    private void TradeScreen(PartyPiece3 sender, PartyPiece3 receiver)
    {
        FieldUI.Instance.TradeScreenShow(sender, receiver);
    }

    private IEnumerator GoToTown(PartyPiece3 visitor, TownPiece3 townPiece)
    {
        List<PartyPiece3> pieces = new List<PartyPiece3>();
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

    private IEnumerator GoToCombat(PartyPiece3 attacker, PartyPiece3 defender)
    {
        List<PartyPiece3> pieces = new List<PartyPiece3> { attacker, defender };
        yield return
            StartCoroutine(pieceHandler.YieldForIdlePieces(pieces));

        FieldTile fieldTile = defender.currentTile as FieldTile;

        FieldSC.Instance.HideScene();
        GameManager.Instance.ChangeSchemes(GameScheme.COMBAT);
        CombatManager.Instance.BootCombat(attacker, defender, fieldTile.db_tileset_lowerLand);
        CombatSC.Instance.ShowScene();
    }

    private IEnumerator GoToCombat(PartyPiece3 attacker, TownPiece3 defender)
    {
        List<PartyPiece3> pieces = new List<PartyPiece3> { attacker };
        yield return
            StartCoroutine(pieceHandler.YieldForIdlePieces(pieces));

        FieldTile fieldTile = defender.currentTile as FieldTile;

        FieldSC.Instance.HideScene();
        GameManager.Instance.ChangeSchemes(GameScheme.COMBAT);
        CombatManager.Instance.BootCombat(attacker, defender, fieldTile.db_tileset_lowerLand);
        CombatSC.Instance.ShowScene();
    }
}
