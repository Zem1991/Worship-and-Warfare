using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllPrefabs : AbstractSingleton<AllPrefabs>
{
    [Header("Player")]
    public AIPersonality aiPersonality;
    public Player player;

    [Header("Any")]
    public Town town;
    public TownBuilding townBuilding;
    public Hero hero;
    public Unit unit;
    public Artifact artifact;

    [Header("Projectile")]
    public Projectile projectile;

    [Header("Stats - Combatant")]
    public CombatPieceStats combatPieceStats;
    public CostStats costStats;
    public AttackStats attackStats;

    [Header("Stats - Hero")]
    public AttributeStats attributeStats;
    public ExperienceStats experienceStats;
    public Inventory inventory;
    public InventorySlot inventorySlot;

    [Header("Stats - Unit")]
    public StackStats stackStats;

    [Header("Field - Scene")]
    public FieldTile fieldTile;
    public TownPiece2 fieldTownPiece;
    public PartyPiece2 fieldPartyPiece;
    public PickupPiece2 fieldPickupPiece;

    [Header("UI")]
    public FieldUI_InventorySlot_Back fuiInvSlot;
    public CombatUI_TurnSequenceItem cuiTurnSequenceItem;
    public TownUI_Building tuiBuilding;

    [Header("Combat - Scene")]
    public CombatTile combatTile;
    public CombatantHeroPiece2 combatHeroPiece;
    public CombatantUnitPiece2 combatUnitPiece;
    public CombatObstacle combatObstacle;

    [Header("Highlight")]
    public Highlight highlight;
}
