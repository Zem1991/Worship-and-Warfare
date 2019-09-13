using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldUI : AbstractSingleton<FieldUI>, IUIScheme, IShowableHideable
{
    [Header("Static panels")]
    public FieldUI_TL_CoreButtons coreButtons;
    public FieldUI_TC_Resources resources;
    public FieldUI_TR_Timers timers;
    public FieldUI_BL_Minimap minimap;
    public FieldUI_BC_Commands commands;
    public FieldUI_BR_FactionCrest factionCrest;
    public FieldUI_BR_PartyCard partyCard;

    [Header("Dynamic panels")]
    public FieldUI_CC_EscapeMenu escapeMenu;

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void UpdatePanels()
    {
        coreButtons.UpdatePanel();
        resources.UpdatePanel();
        timers.UpdatePanel();
        minimap.UpdatePanel();
        coreButtons.UpdatePanel();

        FieldPiece p = FieldInputs.Instance.selectionPiece;
        if (p) UpdateWithSelection(p);
        else UpdateWithoutSelection();
    }

    private void UpdateWithSelection(FieldPiece p)
    {
        factionCrest.Hide();
        partyCard.UpdatePanel(p);
        commands.UpdatePanel(p);
        partyCard.Show();
        commands.Show();
    }

    private void UpdateWithoutSelection()
    {
        partyCard.Hide();
        commands.Hide();
        factionCrest.Show();
    }

    public void EscapeMenuHide()
    {
        escapeMenu.Hide();
        UIManager.Instance.PointerExit(escapeMenu);
    }

    public void EscapeMenuShow()
    {
        escapeMenu.Show();
    }
}
