using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUI : AbstractSingleton<CombatUI>, IUIScheme, IShowableHideable
{
    [Header("Static panels")]
    public CombatUI_TL_CoreButtons coreButtons;
    public CombatUI_TC_PartyCard attackerParty;
    public CombatUI_TC_PartyCard defenderParty;
    public CombatUI_TR_Timers timers;
    public CombatUI_BL_CurrentUnit currentUnit;
    public CombatUI_BC_TurnSequence turnSequence;
    public CombatUI_BR_CombatLogs combatLogs;

    [Header("Dynamic panels")]
    public CombatUI_CC_EscapeMenu escapeMenu;
    public CombatUI_CC_ResultPopup resultPopup;

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
        CombatManager cm = CombatManager.Instance;

        coreButtons.UpdatePanel();
        attackerParty.UpdatePanel(cm.pieceHandler.attackerHero);
        defenderParty.UpdatePanel(cm.pieceHandler.defenderHero);
        timers.UpdatePanel();
        currentUnit.UpdatePanel(cm.currentUnit);
        turnSequence.UpdatePanel();
        combatLogs.UpdatePanel(cm.GetLastLogs(5));
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

    public void ResultPopupHide()
    {
        resultPopup.Hide();
        UIManager.Instance.PointerExit(resultPopup);
    }

    public void ResultPopupShow(string message)
    {
        resultPopup.txtMessage.text = message;
        resultPopup.Show();
    }
}
