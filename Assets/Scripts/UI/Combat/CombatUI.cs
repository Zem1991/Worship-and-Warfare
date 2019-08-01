using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUI : Singleton<CombatUI>, IUIScheme
{
    public CombatUI_TL_CoreButtons coreButtons;
    public CombatUI_TC_PartyCard attackerParty;
    public CombatUI_TC_PartyCard defenderParty;
    public CombatUI_TR_Timers timers;
    public CombatUI_BL_CurrentUnit currentUnit;
    public CombatUI_BC_TurnSequence turnSequence;
    public CombatUI_BR_CombatLogs combatLogs;

    public void UpdatePanels()
    {
        CombatManager cm = CombatManager.Instance;

        coreButtons.UpdatePanel();
        attackerParty.UpdatePanel(cm.pieces.attackerHero);
        defenderParty.UpdatePanel(cm.pieces.defenderHero);
        timers.UpdatePanel();
        currentUnit.UpdatePanel(cm.currentUnit);
        turnSequence.UpdatePanel();
        combatLogs.UpdatePanel();
    }
}
