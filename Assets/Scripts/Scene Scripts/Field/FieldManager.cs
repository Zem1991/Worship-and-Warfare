using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : AbstractSingleton<FieldManager>, IShowableHideable
{
    [Header("Auxiliary Objects")]
    public FieldMapHandler mapHandler;
    public FieldPieceHandler pieceHandler;

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
        foreach (var item in pieceHandler.partyPieces)
        {
            item.ISTET_StartTurn();
        }
    }

    public void PartiesAreInteracting(PartyPiece2 sender, PartyPiece2 receiver)
    {
        if (sender.GetOwner() == receiver.GetOwner())
        {
            GameManager.Instance.PerformExchange(sender, receiver);
        }
        else
        {
            GameManager.Instance.GoToCombat(sender, receiver);
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
        GameManager.Instance.PauseUnpause(false);
        FieldUI.Instance.EscapeMenuHide();

        GameManager.Instance.Restart();
    }
    /*
     * End: UI Top Left buttons
     */

    /*
     * Begin: UI Top Right buttons
     */
    public void EndTurn()
    {
        GameManager.Instance.EndTurn();
    }
    /*
     * End: UI Top Right buttons
     */

    /*
     * Begin: UI Bottom Right buttons
     */
    public void Selection_Movement()
    {
        FieldInputs.Instance.MakeSelectedPieceInteract(false);
    }

    public void Selection_Inventory()
    {
        AbstractFieldPiece2 selectionPiece = FieldInputs.Instance.selectionPiece;
        bool canCommandSelectedPiece = FieldInputs.Instance.canCommandSelectedPiece;

        if (selectionPiece && canCommandSelectedPiece)
        {
            if (FieldUI.Instance.currentWindow == FieldUI.Instance.inventory) FieldUI.Instance.InventoryHide();
            else if (FieldUI.Instance.currentWindow == null) FieldUI.Instance.InventoryShow(selectionPiece);
        }
    }
    /*
    * End: UI Bottom Right buttons
    */
}
