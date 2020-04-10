using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownUI_Panel_Parties : AbstractUIPanel
{
    [Header("Garrison")]
    public Text txtGarrison;
    public UI_PartyInfo garrisonInfo;

    [Header("Visitor")]
    public Text txtVisitor;
    public UI_PartyInfo visitorInfo;

    public void UpdatePanel(Party garrison, Party visitor)
    {
        garrisonInfo.RefreshInfo(garrison);
        visitorInfo.RefreshInfo(visitor);
    }
}
