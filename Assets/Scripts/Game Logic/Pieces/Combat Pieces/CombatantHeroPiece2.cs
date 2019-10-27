using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatantHeroPiece2 : AbstractCombatantPiece2
{
    [Header("Hero data")]
    public Hero hero;

    [Header("Prefab references")]
    public AttributeStats attributeStats;
    //public ExperienceStats experienceStats;
    //public Inventory inventory;

    public void Initialize(Hero hero, Player owner, int spawnId, bool defenderSide = false)
    {
        Initialize(owner, hero.combatPieceStats, spawnId, defenderSide);

        AttributeStats prefabAS = AllPrefabs.Instance.attributeStats;
        //ExperienceStats prefabES = AllPrefabs.Instance.experienceStats;

        this.hero = hero;
        name = "P" + owner.id + " - " + hero.dbData.heroName + ", " + hero.dbData.classs.className;
        profilePicture = hero.dbData.profilePicture;

        attributeStats = Instantiate(prefabAS, transform);
        attributeStats.Initialize(hero.attributeStats);

        ACtP_ResetMovementPoints();
        SetAnimatorOverrideController(hero.dbData.classs.animatorCombat);
    }

    public override void ACtP_ResetMovementPoints()
    {
        if (!pieceMovement) pieceMovement = GetComponent<PieceMovement>();
        Debug.Log("throw new System.NotImplementedException()");
    }

    public override void ACtP_MakeAttack()
    {
        Debug.Log("throw new System.NotImplementedException()");
    }

    public override void ACtP_MakeHurt()
    {
        Debug.Log("throw new System.NotImplementedException()");
    }

    public override int ACtP_CalculateDamage()
    {
        throw new System.NotImplementedException();
    }

    public override void ACtP_Attack(bool ranged)
    {
        throw new System.NotImplementedException();
    }

    public override void ACtP_Retaliate()
    {
        throw new System.NotImplementedException();
    }
}
