using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownUI_Panel_Parties : AbstractUIPanel, ITownPieceRefresh
{
    [Header("Garrison")]
    public Text txtGarrison;
    public UI_PartyInfo garrisonInfo;

    [Header("Visitor")]
    public Text txtVisitor;
    public UI_PartyInfo visitorInfo;

    public void UpdatePanel(TownPiece3 townPiece)
    {
        garrisonInfo.RefreshInfo(townPiece);
        visitorInfo.RefreshInfo(townPiece.visitorPiece);
    }

    public void TownPieceRefresh(TownPiece3 townPiece)
    {
        UpdatePanel(townPiece);
    }
}
