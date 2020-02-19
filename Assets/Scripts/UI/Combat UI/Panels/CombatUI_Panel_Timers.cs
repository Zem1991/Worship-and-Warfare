using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI_Panel_Timers : AbstractUIPanel
{
    public Text txtTurns;
    public Text txtTimer;
    public Button btnNextTurn;

    public void UpdatePanel()
    {
        CombatManager cm = CombatManager.Instance;
        GameManager gm = GameManager.Instance;
        txtTurns.text = "Turn " + cm.currentTurn;
        txtTimer.text = gm.timeElapsedText;
    }
}
