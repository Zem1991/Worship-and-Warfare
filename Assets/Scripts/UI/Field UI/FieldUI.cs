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

    [Header("Main windows")]
    public FieldUI_Panel_EscapeMenu escapeMenu;
    public FieldUI_Panel_Inventory inventory;

    [Header("Other windows")]
    public FieldUI_Panel_LevelUp levelUp;
    public FieldUI_Panel_TradeScreen tradeScreen;

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
        TradeScreenHide();
        InventoryHide();

        LevelUpHide();
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
        if (currentWindow == tradeScreen) TradeScreenHide();
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

        //Don't update windows, only the HUD panels!
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

    //BEGIN:    Main windows options
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
    public void TradeScreenHide()
    {
        tradeScreen.fuiLeftParty.inventoryInfo.DNDForceDrop();
        tradeScreen.fuiRightParty.inventoryInfo.DNDForceDrop();
        tradeScreen.Hide();
        currentWindow = null;
        UIManager.Instance.PointerExit(tradeScreen);
    }
    public void TradeScreenShow(PartyPiece2 left, PartyPiece2 right)
    {
        tradeScreen.UpdatePanel(left, right);
        tradeScreen.Show();
        currentWindow = tradeScreen;
    }
    public void InventoryHide()
    {
        inventory.inventoryInfo.DNDForceDrop();
        inventory.Hide();
        currentWindow = null;
        UIManager.Instance.PointerExit(inventory);
    }
    public void InventoryShow(PartyPiece2 selectionPiece)
    {
        inventory.UpdatePanel(selectionPiece as PartyPiece2, true);
        inventory.Show();
        currentWindow = inventory;
    }
    //END:      Main windows options

    //BEGIN:    Other windows options
    public void LevelUpHide()
    {
        levelUp.Hide();
        currentWindow = null;
        UIManager.Instance.PointerExit(levelUp);
    }
    public void LevelUpShow(Hero hero)
    {
        levelUp.Show();
        currentWindow = levelUp;
    }
    //END:      Other windows options
}
