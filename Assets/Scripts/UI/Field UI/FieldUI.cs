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
    public AUIPanel currentWindow;

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
        FieldInputExecutor fi = FieldSceneInputs.Instance.executor;

        coreButtons.UpdatePanel();
        resources.UpdatePanel();
        timers.UpdatePanel();
        minimap.UpdatePanel();

        PartyPiece2 party = fi.selectionPiece as PartyPiece2;
        bool canCommandSelectedPiece = fi.canCommandSelectedPiece;

        PickupPiece2 pickup = fi.selectionPiece as PickupPiece2;

        if (party) UpdateWithSelection(party, canCommandSelectedPiece);
        else if (pickup) UpdateWithSelection(pickup);
        else UpdateWithoutSelection();

        if (currentWindow == inventory) inventory.UpdatePanel(party);
    }

    private void UpdateWithSelection(PartyPiece2 party, bool canCommandSelectedPiece)
    {
        if (party)
        {
            selection.UpdatePanel(party);
            selection.Show();
        }
        else
        {
            selection.HideInformations();
            selection.Hide();
        }

        if (canCommandSelectedPiece)
        {
            commands.UpdatePanel(party);
        }
        else
        {
            commands.UpdatePanel();
        }
    }

    private void UpdateWithSelection(PickupPiece2 pickup)
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
        inventory.InvSlotEndDrag(null);
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
