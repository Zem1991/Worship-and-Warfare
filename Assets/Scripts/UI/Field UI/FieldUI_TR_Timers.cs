using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldUI_TR_Timers : AUIPanel
{
    public Text txtTurns;
    public Text txtTimer;
    public Button btnNextTurn;

    public void UpdatePanel()
    {
        GameManager gm = GameManager.Instance;
        txtTurns.text = "Day " + gm.day + ", week " + gm.week + ", month " + gm.month;
        txtTimer.text = gm.timeElapsedText;
    }
}
