using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldUI_Panel_TradeScreen_Party : MonoBehaviour
{
    [Header("Local reference")]
    public FieldUI_Panel_TradeScreen tradeScreen;

    [Header("Hero info")]
    public UI_HeroInfo heroInfo;
    public UI_AttributeInfo attributeInfo;

    [Header("Inventory Slots")]
    public FieldUI_InventorySlot mainHand;
    public FieldUI_InventorySlot offHand;
    public FieldUI_InventorySlot helmet;
    public FieldUI_InventorySlot armor;
    public FieldUI_InventorySlot trinket1;
    public FieldUI_InventorySlot trinket2;
    public FieldUI_InventorySlot trinket3;
    public FieldUI_InventorySlot trinket4;

    public void UpdatePanel(PartyPiece2 p)
    {
        Hero hero = p.party.hero.slotObj as Hero;
        heroInfo.RefreshInfo(hero);
        attributeInfo.RefreshInfo(hero.attributeStats);

        mainHand.UpdateSlot(tradeScreen, hero.inventory.mainHand);
        offHand.UpdateSlot(tradeScreen, hero.inventory.offHand);
        helmet.UpdateSlot(tradeScreen, hero.inventory.helmet);
        armor.UpdateSlot(tradeScreen, hero.inventory.armor);
        trinket1.UpdateSlot(tradeScreen, hero.inventory.trinket1);
        trinket2.UpdateSlot(tradeScreen, hero.inventory.trinket2);
        trinket3.UpdateSlot(tradeScreen, hero.inventory.trinket3);
        trinket4.UpdateSlot(tradeScreen, hero.inventory.trinket4);
    }
}
