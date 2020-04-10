using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldUI_Panel_Inventory : AbstractUIPanel, IInventoryPanel
{
    [Header("Hero info")]
    public UI_HeroInfo heroInfo;
    public UI_AttributeInfo attributeInfo;
    public UI_InventoryInfo inventoryInfo;

    [Header("Buttons")]
    public Button btnClose;

    public void UpdatePanel(PartyPiece2 partyPiece, bool refreshBackpackSlots)
    {
        Hero hero = partyPiece.party.hero.GetSlotObject() as Hero;
        heroInfo.RefreshInfo(hero);
        attributeInfo.RefreshInfo(hero?.attributeStats);
        inventoryInfo.RefreshInfo(partyPiece, refreshBackpackSlots);
    }

    public void CallUpdatePanel(PartyPiece2 partyPiece, bool refreshBackpackSlots)
    {
        UpdatePanel(partyPiece, refreshBackpackSlots);
    }
}
