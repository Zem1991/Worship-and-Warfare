using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractUnit : MonoBehaviour
{
    [Header("Database references")]
    [SerializeField] protected DB_Unit dbUnit;

    [Header("Unit stats")]
    public SettingsStats2 settingsStats;
    public HealthStats2 healthStats;
    public MovementStats2 movementStats;
    public AttributeStats2 attributeStats;
    public OffenseStats2 offenseStats;
    public DefenseStats2 defenseStats;
    public AbilityStats2 abilityStats;

    public void Initialize(DB_Unit dbUnit)
    {
        this.dbUnit = dbUnit;

        settingsStats = Instantiate(dbUnit.settingsStats, transform);
        healthStats = Instantiate(dbUnit.healthStats, transform);
        movementStats = Instantiate(dbUnit.movementStats, transform);
        attributeStats = Instantiate(dbUnit.attributeStats, transform);
        offenseStats = Instantiate(dbUnit.offenseStats, transform);
        defenseStats = Instantiate(dbUnit.defenseStats, transform);
        abilityStats = Instantiate(dbUnit.abilityStats, transform);
    }

    public DB_Unit GetDBUnit()
    {
        return dbUnit;
    }

    //public bool CompareDatabaseEntry(AbstractUnit compareWithThis)
    //{
    //    if (unitType != compareWithThis.unitType) return false;

    //    switch (unitType)
    //    {
    //        case UnitType.HERO:
    //            HeroUnit thisHero = this as HeroUnit;
    //            HeroUnit otherHero = compareWithThis as HeroUnit;
    //            if (thisHero.dbData == otherHero.dbData) return true;
    //            break;
    //        case UnitType.CREATURE:
    //            CombatUnit thisUnit = this as CombatUnit;
    //            CombatUnit otherUnit = compareWithThis as CombatUnit;
    //            if (thisUnit.dbData == otherUnit.dbData) return true;
    //            break;
    //        //case PartyElementType.SIEGE_ENGINE:
    //        //    break;
    //        //default:
    //        //    break;
    //    }
    //    return false;
    //}

    public abstract Sprite AU_GetProfileImage();

    public abstract string AU_GetUnitName();
}
