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
    //public Party party;
    public PartySlot partySlot;
    public HeroUnit hero;
    public CombatUnit unit;

    [Header("Inventory")]
    public Inventory inventory;
    public InventorySlot inventorySlot;
    public Artifact artifact;

    [Header("Projectile")]
    public Projectile projectile;

    [Header("Field scene")]
    public FieldTile fieldTile;
    public TownPiece3 fieldTownPiece;
    public PartyPiece3 fieldPartyPiece;
    public PickupPiece3 pickupPiece;

    [Header("Combat scene")]
    public CombatTile combatTile;
    public HeroUnitPiece3 combatHeroPiece;
    public CombatUnitPiece3 combatUnitPiece;
    public DoodadPiece3 combatDoodad;

    [Header("Highlight")]
    public Highlight highlight;

    [Header("Field UI")]
    public FieldUI_InventorySlot_Holder fuiInvSlotHolder;
    //public FieldUI_Panel_LevelUp_AttributeOption fuiAttributeOption;

    [Header("Combat UI")]
    public CombatUI_TurnSequenceItem cuiTurnSequenceItem;

    [Header("Town UI")]
    public TownUI_Structure tuiBuilding;
    public TownUI_Panel_BuildStructure_StructureOption tuiStructureOption;
    public TownUI_Panel_RecruitHero_HeroOption tuiHeroOption;
    public TownUI_Panel_RecruitCreature_CreatureOption tuiCreatureOption;
}
