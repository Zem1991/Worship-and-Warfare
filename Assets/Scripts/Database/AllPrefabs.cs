using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllPrefabs : AbstractSingleton<AllPrefabs>
{
    [Header("Player")]
    public Player player;
    public AIPersonality aiPersonality;

    [Header("Town")]
    public Town town;
    public TownBuilding townBuilding;

    [Header("Party")]
    public Party party;
    public PartySlot partySlot;
    public Hero hero;
    public Unit unit;

    [Header("Pickup")]
    public ResourcePickupPiece2 resourcePickupPiece;
    public ArtifactPickupPiece2 artifactPickupPiece;

    [Header("Inventory")]
    public Inventory inventory;
    public InventorySlot inventorySlot;
    public Artifact artifact;

    [Header("Projectile")]
    public Projectile projectile;

    [Header("Stats - Combatant")]
    public CombatPieceStats combatPieceStats;
    public ResourceStats resourceStats;
    public AttackStats attackStats;

    [Header("Stats - Hero")]
    public AttributeStats attributeStats;
    public ExperienceStats experienceStats;

    [Header("Stats - Unit")]
    public StackStats stackStats;

    [Header("Field - Scene")]
    public FieldTile fieldTile;
    public TownPiece2 fieldTownPiece;
    public PartyPiece2 fieldPartyPiece;

    [Header("UI")]
    public FieldUI_InventorySlot_Holder fuiInvSlotHolder;
    public CombatUI_TurnSequenceItem cuiTurnSequenceItem;
    public TownUI_Building tuiBuilding;
    public TownUI_Panel_BuildStructure_StructureOption tuiStructureOption;
    public TownUI_Panel_RecruitHero_HeroOption tuiHeroOption;
    public TownUI_Panel_RecruitCreature_CreatureOption tuiCreatureOption;

    [Header("Combat - Scene")]
    public CombatTile combatTile;
    public CombatantHeroPiece2 combatHeroPiece;
    public CombatantUnitPiece2 combatUnitPiece;
    public CombatObstacle combatObstacle;

    [Header("Highlight")]
    public Highlight highlight;
}
