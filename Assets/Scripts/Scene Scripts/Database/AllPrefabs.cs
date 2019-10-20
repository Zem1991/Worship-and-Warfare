using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllPrefabs : AbstractSingleton<AllPrefabs>
{
    [Header("Input")]
    public InputHighlight inputHighlight;

    [Header("Player")]
    public AIPersonality aiPersonality;
    public Player player;

    [Header("Any")]
    public Hero hero;
    public Unit unit;
    public Inventory inventory;
    public InventorySlot inventorySlot;
    public Artifact artifact;

    [Header("Field Scene")]
    public FieldTile fieldTile;
    public PartyPiece2 fieldPartyPiece;

    [Header("Field UI")]
    public FUI_InventorySlot_Back fuiInvSlot;

    [Header("Combat Scene")]
    public CombatTile combatTile;
    public CombatantHeroPiece2 combatHeroPiece;
    public CombatantUnitPiece2 combatUnitPiece;

    [Header("Field UI")]
    public CUI_TurnSequenceItem cuiTurnSequenceItem;
}
