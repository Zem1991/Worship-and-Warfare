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

        attributeStats = Instantiate(prefabAS, transform);
        attributeStats.Initialize(hero.attributeStats);

        IMP_ResetMovementPoints();
        SetAnimatorOverrideController(hero.dbData.classs.animatorCombat);
    }

    //public override bool ACP_TakeDamage(int amount)
    //{
    //    bool result = combatPieceStats.TakeDamage(amount);
    //    string log = hero.dbData.heroName + " took " + amount + " damage.";
    //    CombatManager.Instance.AddEntryToLog(log);

    //    if (result)
    //    {
    //        ACP_Die();
    //        return true;
    //    }
    //    else
    //    {
    //        isHurt = true;
    //        return false;
    //    }
    //}
}
