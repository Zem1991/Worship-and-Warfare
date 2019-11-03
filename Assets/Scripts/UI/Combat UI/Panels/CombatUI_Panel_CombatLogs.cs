using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI_Panel_CombatLogs : AUIPanel
{
    public Text logs;

    public void UpdatePanel(List<string> entries)
    {
        logs.text = "";
        for (int i = entries.Count; i > 0; i--)
        {
            string line = entries[i - 1];
            if (i > 1) line += "\n";
            logs.text += line;
        }
    }
}
