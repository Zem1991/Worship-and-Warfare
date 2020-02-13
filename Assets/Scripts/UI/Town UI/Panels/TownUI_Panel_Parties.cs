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

    public void UpdatePanel(PartyPiece2 visitor, PartyPiece2 garrison)
    {
        UpdateVisitor(visitor);
        UpdateGarrison(garrison);
    }

    private void UpdateVisitor(PartyPiece2 visitor)
    {
        if (visitor != null)
        {
            visitorHeroInfo.RefreshInfo(visitor.partyHero);
            visitorUnitsInfo.RefreshInfo(visitor.partyUnits);
        }
        else
        {
            visitorHeroInfo.RefreshInfo(null);
            visitorUnitsInfo.RefreshInfo(null);
        }
    }

    private void UpdateGarrison(PartyPiece2 garrison)
    {
        if (garrison != null)
        {
            garrisonHeroInfo.RefreshInfo(garrison.partyHero);
            garrisonUnitsInfo.RefreshInfo(garrison.partyUnits);
        }
        else
        {
            garrisonHeroInfo.RefreshInfo(null);
            garrisonUnitsInfo.RefreshInfo(null);
        }
    }
}
