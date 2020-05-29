using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldUI_Panel_Inventory : AbstractUIPanel, IInventoryRefresh
{
    [Header("Hero info")]
    public UI_HeroInfo heroInfo;
    public UI_AttributeInfo attributeInfo;
    public UI_InventoryInfo inventoryInfo;

    [Header("Buttons")]
    public Button btnClose;

    public void UpdatePanel(PartyPiece3 partyPiece, bool refreshBackpackSlots)
    {
        HeroUnit hero = partyPiece.IPFC_GetPartyForCombat().GetHeroSlot().Get() as HeroUnit;
        heroInfo.RefreshInfo(hero);
        attributeInfo.RefreshInfo(hero?.attributeStats);
        inventoryInfo.RefreshInfo(partyPiece, refreshBackpackSlots);
    }

    public void InventoryRefresh(PartyPiece3 partyPiece, bool refreshBackpackSlots)
    {
        UpdatePanel(partyPiece, refreshBackpackSlots);
    }
}
