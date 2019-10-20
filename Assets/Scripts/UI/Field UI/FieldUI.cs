using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldUI : AbstractSingleton<FieldUI>, IUIScheme, IShowableHideable
{
    [Header("Panels")]
    public FieldUI_TL_CoreButtons coreButtons;
    public FieldUI_TC_Resources resources;
    public FieldUI_TR_Timers timers;
    public FieldUI_BL_Minimap minimap;
    public FieldUI_BC_Commands commands;
    public FieldUI_BR_SelectionCard selectionCard;

    [Header("Windows")]
    public FieldUI_CC_EscapeMenu escapeMenu;
    public FieldUI_CC_Inventory inventory;

    [Header("Current Window")]
    public AUIPanel currentWindow;

    public override void Awake()
    {
        base.Awake();
        EscapeMenuHide();
        InventoryHide();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void CloseCurrentWindow()
    {
        if (currentWindow == escapeMenu) EscapeMenuHide();
        if (currentWindow == inventory) InventoryHide();
    }

    public void UpdatePanels()
    {
        FieldInputs fi = FieldInputs.Instance;

        coreButtons.UpdatePanel();
        resources.UpdatePanel();
        timers.UpdatePanel();
        minimap.UpdatePanel();
        coreButtons.UpdatePanel();

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
        selectionCard.UpdatePanel(party);

        if (canCommandSelectedPiece)
        {
            commands.UpdatePanel(party);
            commands.Show();
        }
        else
        {
            commands.Hide();
        }
    }

    private void UpdateWithSelection(PickupPiece2 pickup)
    {
        selectionCard.UpdatePanel(pickup);
        commands.Hide();
    }

    private void UpdateWithoutSelection()
    {
        selectionCard.HidePanel();
        commands.Hide();
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
