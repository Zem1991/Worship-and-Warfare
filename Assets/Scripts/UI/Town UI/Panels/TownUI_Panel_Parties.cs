using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownUI_Panel_Parties : AUIPanel
{

    [Header("Visitor")]
    public Text txtVisitor;
    public UI_HeroInfo visitorHeroInfo;
    public UI_UnitsInfo visitorUnitsInfo;

    [Header("Garrison")]
    public Text txtGarrison;
    public UI_HeroInfo garrisonHeroInfo;
    public UI_UnitsInfo garrisonUnitsInfo;

    public void UpdatePanel()
    {
        //TODO this
    }
}
