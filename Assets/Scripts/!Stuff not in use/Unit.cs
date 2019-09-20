using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit
{
    public int dbId;
    public string nameSingular;
    public string namePlural;

    public int hitPoints;
    public int stackSize;

    public int damageMin;
    public int damageMax;
    public bool hasRangedAttack;
    public int resistance;
    public int speed;
    public int initiative;

    public Sprite imgProfile;
    public AnimatorOverrideController animatorField;
    public AnimatorOverrideController animatorCombat;

    public Unit(int dbId, DB_Unit dbData, int stackSize)
    {
        this.dbId = dbId;
        nameSingular = dbData.nameSingular;
        namePlural = dbData.namePlural;

        hitPoints = dbData.hitPoints;
        this.stackSize = stackSize;

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
}
