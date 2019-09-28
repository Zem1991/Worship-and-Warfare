﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllPrefabs : AbstractSingleton<AllPrefabs>
{
    [Header("Input")]
    public InputHighlight inputHighlight;

    [Header("Player")]
    public Player player;

    [Header("Any")]
    public Hero hero;
    public Unit unit;
    public Inventory inventory;
    public InventorySlot inventorySlot;
    public Artifact artifact;

    [Header("Field Scene")]
    public FieldTile fieldTile;
    public FieldPiece fieldPiece;

    [Header("Field UI")]
    public FUI_InventorySlot_Back fuiInvSlot;

    [Header("Combat Scene")]
    public CombatTile combatTile;
    public CombatHeroPiece combatHeroPiece;
    public CombatUnitPiece combatUnitPiece;

    [Header("Field UI")]
    public CUI_TurnSequenceItem cuiTurnSequenceItem;
}