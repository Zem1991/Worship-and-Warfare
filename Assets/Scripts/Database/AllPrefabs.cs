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

    [Header("Field - Scene")]
    public FieldTile fieldTile;
    public TownPiece2 fieldTownPiece;
    public PartyPiece2 fieldPartyPiece;

    [Header("Combat - Scene")]
    public CombatTile combatTile;
    public CombatantHeroPiece2 combatHeroPiece;
    public CombatantUnitPiece2 combatUnitPiece;
    public CombatObstacle combatObstacle;

    [Header("Highlight")]
    public Highlight highlight;

    [Header("Field UI")]
    public FieldUI_InventorySlot_Holder fuiInvSlotHolder;
    //public FieldUI_Panel_LevelUp_AttributeOption fuiAttributeOption;

    [Header("Combat UI")]
    public CombatUI_TurnSequenceItem cuiTurnSequenceItem;

    [Header("Town UI")]
    public TownUI_Building tuiBuilding;
    public TownUI_Panel_BuildStructure_StructureOption tuiStructureOption;
    public TownUI_Panel_RecruitHero_HeroOption tuiHeroOption;
    public TownUI_Panel_RecruitCreature_CreatureOption tuiCreatureOption;
}
