using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldUI_Panel_Commands : AbstractUIPanel
{
    public Image crest;
    public FieldUI_Panel_Commands_Town townCommands;
    public FieldUI_Panel_Commands_Party partyCommands;

    public void UpdatePanel()
    {
        townCommands.Hide();
        partyCommands.Hide();
    }

    public void UpdatePanel(TownPiece3 tp)
    {
        partyCommands.Hide();
        townCommands.UpdatePanel(tp);
        townCommands.Show();
    }

    public void UpdatePanel(PartyPiece3 pp)
    {
        townCommands.Hide();
        partyCommands.UpdatePanel(pp);
        partyCommands.Show();
    }
}
