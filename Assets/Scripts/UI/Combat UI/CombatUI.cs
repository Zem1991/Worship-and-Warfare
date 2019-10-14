using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUI : AbstractSingleton<CombatUI>, IUIScheme, IShowableHideable
{
    [Header("Panels")]
    public CombatUI_TL_CoreButtons coreButtons;
    public CombatUI_TC_PartyCard attackerParty;
    public CombatUI_TC_PartyCard defenderParty;
    public CombatUI_TR_Timers timers;
    public CombatUI_BL_CurrentUnit currentUnit;
    public CombatUI_BC_TurnSequence turnSequence;
    public CombatUI_BR_CombatLogs combatLogs;

    [Header("Windows")]
    public CombatUI_CC_EscapeMenu escapeMenu;
    public CombatUI_CC_ResultPopup resultPopup;

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

        AbstractCombatPiece ucm = cm.currentPiece;
        bool canCommandSelectedPiece = ci.canCommandSelectedPiece;

        if (ucm) UpdateWithSelection(ucm, canCommandSelectedPiece);
        else UpdateWithoutSelection();
    }

    private void UpdateWithSelection(AbstractCombatPiece acp, bool canCommandSelectedPiece)
    {
        CombatUnitPiece cup = acp as CombatUnitPiece;
        currentUnit.UpdatePanel(cup.unit);
    }

    private void UpdateWithoutSelection()
    {
        currentUnit.UpdatePanel(null);
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
