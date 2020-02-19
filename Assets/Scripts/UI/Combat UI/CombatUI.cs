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
    public UI_CombatActorInspector combatActorInspector;

    [Header("Current Window")]
    public AbstractUIPanel currentWindow;

    public void Hide()
    {
        gameObject.SetActive(false);

        coreButtons.Hide();
        attackerParty.Hide();
        defenderParty.Hide();
        timers.Hide();
        currentPiece.Hide();
        turnSequence.Hide();
        //combatLogs.Hide();

        EscapeMenuHide();
        ResultPopupHide();
    }

    public void Show()
    {
        gameObject.SetActive(true);

        coreButtons.Show();
        attackerParty.Show();
        defenderParty.Show();
        timers.Show();
        currentPiece.Show();
        turnSequence.Show();
        //combatLogs.Show();
    }

    public void CloseCurrentWindow()
    {
        if (currentWindow == escapeMenu) EscapeMenuHide();
        if (currentWindow == combatActorInspector) CombatActorInspectorHide();
    }

    public void UpdatePanels()
    {
        CombatManager cm = CombatManager.Instance;
        CombatInputExecutor ci = CombatSceneInputs.Instance.executor;

        coreButtons.UpdatePanel();
        attackerParty.UpdatePanel(cm.pieceHandler.attackerHero);
        defenderParty.UpdatePanel(cm.pieceHandler.defenderHero);
        timers.UpdatePanel();
        turnSequence.UpdatePanel();
        combatLogs.UpdatePanel(cm.GetLastLogs(5));

        AbstractCombatantPiece2 actp = cm.currentPiece as AbstractCombatantPiece2;
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

    public void CombatActorInspectorHide()
    {
        combatActorInspector.Hide();
        currentWindow = null;
        UIManager.Instance.PointerExit(combatActorInspector);
    }

    public void CombatActorInspectorShow(AbstractCombatActorPiece2 acap)
    {
        combatActorInspector.RefreshInfo(acap);
        combatActorInspector.Show();
        currentWindow = combatActorInspector;
    }
}
