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

    public void UpdatePanel(Party visitor, Party garrison)
    {
        UpdateVisitor(visitor);
        UpdateGarrison(garrison);
    }

    private void UpdateVisitor(Party visitor)
    {
        if (visitor != null)
        {
            visitorHeroInfo.RefreshInfo(visitor.hero);
            visitorUnitsInfo.RefreshInfo(visitor.units);
        }
        else
        {
            visitorHeroInfo.RefreshInfo(null);
            visitorUnitsInfo.RefreshInfo(null);
        }
    }

    private void UpdateGarrison(Party garrison)
    {
        if (garrison != null)
        {
            garrisonHeroInfo.RefreshInfo(garrison.hero);
            garrisonUnitsInfo.RefreshInfo(garrison.units);
        }
        else
        {
            garrisonHeroInfo.RefreshInfo(null);
            garrisonUnitsInfo.RefreshInfo(null);
        }
    }
}
