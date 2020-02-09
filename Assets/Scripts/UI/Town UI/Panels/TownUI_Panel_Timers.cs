using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownUI_Panel_Timers : AUIPanel
{
    public Text txtTurns;
    public Text txtTimer;
    public Button btnNextTurn;

    public void UpdatePanel()
    {
        FieldManager fm = FieldManager.Instance;
        GameManager gm = GameManager.Instance;
        txtTurns.text = "Day " + fm.day + ", week " + fm.week + ", month " + fm.month;
        txtTimer.text = gm.timeElapsedText;
    }

    public void LockButtons()
    {
        btnNextTurn.interactable = false;
    }

    public void UnlockButtons()
    {
        btnNextTurn.interactable = true;
    }
}
