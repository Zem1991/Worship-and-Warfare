using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI_TR_Timers : AUIPanel
{
    public Text txtTurns;
    public Text txtTimer;
    public Button btnNextTurn;

    public void UpdatePanel()
    {
        GameManager gm = GameManager.Instance;

        int day = gm.currentDay % 7;
        if (day == 0) day = 7;
        int week = (gm.currentDay / 7) + 1;

        txtTurns.text = "Day " + day + ", week " + week;
        txtTimer.text = gm.timeElapsedText;
    }
}
