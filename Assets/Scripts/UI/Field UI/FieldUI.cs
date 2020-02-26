using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldUI : AbstractSingleton<FieldUI>, IUIScheme, IShowableHideable
{
    [Header("Panels")]
    public FieldUI_Panel_CoreButtons coreButtons;
    public FieldUI_Panel_Resources resources;
    public FieldUI_Panel_Timers timers;
    public FieldUI_Panel_Minimap minimap;
    public FieldUI_Panel_Selection selection;
    public FieldUI_Panel_Commands commands;

    [Header("Windows")]
    public FieldUI_Panel_EscapeMenu escapeMenu;
    public FieldUI_Panel_Inventory inventory;

    [Header("Current Window")]
    public AbstractUIPanel currentWindow;

    public void Hide()
    {
        gameObject.SetActive(false);

        coreButtons.Hide();
        resources.Hide();
        timers.Hide();
        minimap.Hide();
        selection.Hide();
        commands.Hide();

        EscapeMenuHide();
        InventoryHide();
    }

    public void Show()
    {
        gameObject.SetActive(true);

        coreButtons.Show();
        resources.Show();
        timers.Show();
        minimap.Show();
        selection.Show();
        commands.Show();
    }

    public void CloseCurrentWindow()
    {
        if (currentWindow == escapeMenu) EscapeMenuHide();
        if (currentWindow == inventory) InventoryHide();
    }

    public void UpdatePanels()
    {
        Player player = PlayerManager.Instance.localPlayer;
        FieldInputExecutor fi = FieldSceneInputs.Instance.executor;

        coreButtons.UpdatePanel();
        resources.UpdatePanel(player);
        timers.UpdatePanel();
        minimap.UpdatePanel();

        TownPiece2 town = fi.selectionPiece as TownPiece2;
        PartyPiece2 party = fi.selectionPiece as PartyPiece2;
        AbstractPickupPiece2 pickup = fi.selectionPiece as AbstractPickupPiece2;
        bool canCommandSelectedPiece = fi.canCommandSelectedPiece;

        if (town) UpdateWithSelection(town, canCommandSelectedPiece);
        else if (party) UpdateWithSelection(party, canCommandSelectedPiece);
        else if (pickup) UpdateWithSelection(pickup);
        else UpdateWithoutSelection();

        if (currentWindow == inventory) inventory.UpdatePanel(party);
    }

    private void UpdateWithSelection(TownPiece2 town, bool canCommandSelectedPiece)
    {
        selection.UpdatePanel(town);
        selection.Show();

        if (canCommandSelectedPiece) commands.UpdatePanel(town);
        else commands.UpdatePanel();
    }

    private void UpdateWithSelection(PartyPiece2 party, bool canCommandSelectedPiece)
    {
        selection.UpdatePanel(party);
        selection.Show();

        if (canCommandSelectedPiece) commands.UpdatePanel(party);
        else commands.UpdatePanel();
    }

    private void UpdateWithSelection(AbstractPickupPiece2 pickup)
    {
        selection.UpdatePanel(pickup);
        selection.Show();

        commands.UpdatePanel();
    }

    private void UpdateWithoutSelection()
    {
        selection.HideInformations();
        selection.Hide();

        commands.UpdatePanel();
    }

    public void EscapeMenuHide()
    {
        escapeMenu.Hide();
        currentWindow = null;
        UIManager.Instance.PointerExit(escapeMenu);
    }

    public void EscapeMenuShow()
    {
        escapeMenu.Show();
        currentWindow = escapeMenu;
    }

    public void InventoryHide()
    {
        inventory.DNDEndDrag();
        inventory.Hide();
        currentWindow = null;
        UIManager.Instance.PointerExit(inventory);
    }

    public void InventoryShow(AbstractFieldPiece2 selectionPiece)
    {
        inventory.Show();
        currentWindow = inventory;
    }
}
