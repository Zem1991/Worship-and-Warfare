using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUI : AbstractSingleton<CombatUI>, IUIScheme, IShowableHideable
{
    [Header("Panels")]
    public CombatUI_Panel_CoreButtons coreButtons;
    public CombatUI_Panel_PartyCard attackerParty;
    public CombatUI_Panel_PartyCard defenderParty;
    public CombatUI_Panel_Timers timers;
    public CombatUI_Panel_CurrentPiece currentPiece;
    public CombatUI_Panel_TurnSequence turnSequence;
    public CombatUI_Panel_CombatLogs combatLogs;

    [Header("Windows")]
    public CombatUI_Panel_EscapeMenu escapeMenu;
    public CombatUI_Panel_ResultPopup resultPopup;

    [Header("Current Window")]
    public AUIPanel currentWindow;

    public override void Awake()
    {
        base.Awake();
        EscapeMenuHide();
        ResultPopupHide();
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
        //if (currentWindow == inventory) InventoryHide();
    }

    public void UpdatePanels()
    {
        CombatManager cm = CombatManager.Instance;
        CombatInputs ci = CombatInputs.Instance;

        coreButtons.UpdatePanel();
        attackerParty.UpdatePanel(cm.pieceHandler.attackerHero);
        defenderParty.UpdatePanel(cm.pieceHandler.defenderHero);
        timers.UpdatePanel();
        turnSequence.UpdatePanel();
        combatLogs.UpdatePanel(cm.GetLastLogs(5));

        AbstractCombatantPiece2 actp = cm.currentPiece;
        bool canCommandSelectedPiece = ci.canCommandSelectedPiece;

        if (actp) UpdateWithSelection(actp, canCommandSelectedPiece);
        else UpdateWithoutSelection();
    }

    private void UpdateWithSelection(AbstractCombatantPiece2 actp, bool canCommandSelectedPiece)
    {
        currentPiece.UpdatePanel(actp, canCommandSelectedPiece);
    }

    private void UpdateWithoutSelection()
    {
        currentPiece.UpdatePanel(null, false);
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

    public void ResultPopupHide()
    {
        resultPopup.Hide();
        currentWindow = null;
        UIManager.Instance.PointerExit(resultPopup);
    }

    public void ResultPopupShow(string message)
    {
        resultPopup.txtMessage.text = message;
        resultPopup.Show();
        currentWindow = resultPopup;
    }
}
