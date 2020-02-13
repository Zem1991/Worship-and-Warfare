using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownUI_Panel_Town : AUIPanel
{
    public Text townName;
    public TownUI_Panel_Town_Data townData;
    public TownUI_Panel_Town_Interactions townInteractions;

    public void UpdatePanel(Town town)
    {
        townName.text = town.townName;

        //TODO this?
    }
}
