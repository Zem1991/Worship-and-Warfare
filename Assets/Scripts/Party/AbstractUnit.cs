using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SettingsStats2))]
[RequireComponent(typeof(HealthStats2))]
[RequireComponent(typeof(MovementStats2))]
[RequireComponent(typeof(AttributeStats2))]
[RequireComponent(typeof(OffenseStats2))]
[RequireComponent(typeof(DefenseStats2))]
[RequireComponent(typeof(AbilityStats2))]
public abstract class AbstractUnit : MonoBehaviour
{
    [Header("Database references")]
    [SerializeField] protected DB_Unit dbUnit;

    [Header("Object components")]
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

        settingsStats.CopyFrom(dbUnit.settingsStats);
        healthStats.CopyFrom(dbUnit.healthStats);
        movementStats.CopyFrom(dbUnit.movementStats);
        attributeStats.CopyFrom(dbUnit.attributeStats);
        offenseStats.CopyFrom(dbUnit.offenseStats);
        defenseStats.CopyFrom(dbUnit.defenseStats);
        abilityStats.CopyFrom(dbUnit.abilityStats);
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
