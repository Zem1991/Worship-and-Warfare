using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldUI_Panel_TradeScreen_Party : MonoBehaviour, IInventoryRefresh, IPartyPieceRefresh
{
    [Header("Hero info")]
    public UI_HeroInfo heroInfo;
    public UI_AttributeInfo attributeInfo;

    [Header("Drags and Drops")]
    public UI_InventoryInfo inventoryInfo;
    public UI_PartyInfo partyInfo;

    public void UpdatePanel(PartyPiece3 partyPiece, bool refreshBackpackSlots)
    {
        HeroUnit hero = partyPiece.party.GetHeroSlot().Get() as HeroUnit;
        heroInfo.RefreshInfo(hero);
        attributeInfo.RefreshInfo(hero?.attributeStats);
        inventoryInfo.RefreshInfo(partyPiece, refreshBackpackSlots);
        partyInfo.RefreshInfo(partyPiece);
    }

    public void InventoryRefresh(PartyPiece3 partyPiece, bool refreshBackpackSlots)
    {
        UpdatePanel(partyPiece, refreshBackpackSlots);
    }

    public void PartyPieceRefresh(PartyPiece3 partyPiece)
    {
        UpdatePanel(partyPiece, true);
    }
}
