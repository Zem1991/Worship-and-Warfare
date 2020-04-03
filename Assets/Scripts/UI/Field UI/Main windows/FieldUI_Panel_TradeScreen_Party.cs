﻿using System.Collections;
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
    public FieldUI_InventorySlot head;
    public FieldUI_InventorySlot torso;
    public FieldUI_InventorySlot trinket1;
    public FieldUI_InventorySlot trinket2;
    public FieldUI_InventorySlot trinket3;
    public FieldUI_InventorySlot trinket4;

    public void UpdatePanel(PartyPiece2 p)
    {
        Hero hero = p.party.hero.slotObj as Hero;
        heroInfo.RefreshInfo(hero);
        attributeInfo.RefreshInfo(hero.attributeStats);

        Inventory inv = hero.inventory;
        mainHand.UpdateSlot(tradeScreen, inv.GetEquipmentSlot(ArtifactType.MAIN_HAND));
        offHand.UpdateSlot(tradeScreen, inv.GetEquipmentSlot(ArtifactType.OFF_HAND));
        head.UpdateSlot(tradeScreen, inv.GetEquipmentSlot(ArtifactType.HEAD));
        torso.UpdateSlot(tradeScreen, inv.GetEquipmentSlot(ArtifactType.TORSO));
        trinket1.UpdateSlot(tradeScreen, inv.GetEquipmentSlot(ArtifactType.TRINKET, 1));
        trinket2.UpdateSlot(tradeScreen, inv.GetEquipmentSlot(ArtifactType.TRINKET, 2));
        trinket3.UpdateSlot(tradeScreen, inv.GetEquipmentSlot(ArtifactType.TRINKET, 3));
        trinket4.UpdateSlot(tradeScreen, inv.GetEquipmentSlot(ArtifactType.TRINKET, 4));
    }
}
