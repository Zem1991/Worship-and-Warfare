using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public DB_Unit dbData;

    public string nameSingular;
    public string namePlural;

    public int hitPointsMax;
    public int hitPointsCurrent;
    public int stackSizeStart;
    public int stackSizeCurrent;

    public int damageMin;
    public int damageMax;
    public bool hasRangedAttack;
    public int resistance;
    public int speed;
    public int initiative;

    public Sprite imgProfile;
    public AnimatorOverrideController animatorField;
    public AnimatorOverrideController animatorCombat;

    public void Initialize(UnitData unitData, DB_Unit dbData)
    {
        this.dbData = dbData;

        nameSingular = dbData.nameSingular;
        namePlural = dbData.namePlural;

        hitPointsMax = dbData.hitPoints;
        hitPointsCurrent = hitPointsMax;
        stackSizeStart = unitData.stackSize;
        stackSizeCurrent = stackSizeStart;

        damageMin = dbData.damageMin;
        damageMax = dbData.damageMax;
        hasRangedAttack = dbData.hasRangedAttack;
        resistance = dbData.resistance;
        speed = dbData.movementRange;
        initiative = dbData.initiative;

        imgProfile = dbData.profilePicture;
        animatorField = dbData.animatorField;
        animatorCombat = dbData.animatorCombat;
    }

    public string GetName()
    {
        if (stackSizeCurrent <= 1) return nameSingular;
        return namePlural;
    }
}
