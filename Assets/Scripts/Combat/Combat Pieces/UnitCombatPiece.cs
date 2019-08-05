using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCombatPiece : AbstractCombatPiece
{
    //public bool didMove;
    //public bool didAttack;
    //public int hp;
    //public int initiative;

    public string unitName;
    public int hitPoints;
    public int hitPointsCurrent;
    public int stackSize;
    public int stackSizeCurrent;

    public int damageMin;
    public int damageMax;
    public int resistance;
    public int speed;
    public int initiative;

    public Sprite imgProfile;
    public Sprite imgCombat;

    public void Initialize(Unit unit)
    {
        unitName = unit.unitName;
        hitPoints = unit.hitPoints;
        hitPointsCurrent = hitPoints;
        stackSize = unit.stackSize;
        stackSizeCurrent = stackSize;

        damageMin = unit.damageMin;
        damageMax = unit.damageMax;
        resistance = unit.resistance;
        speed = unit.speed;
        initiative = unit.initiative;

        imgProfile = unit.imgProfile;
        imgCombat = unit.imgCombat;

        ChangeSprite(imgCombat);
    }

    public override void InteractWithPiece(AbstractPiece target)
    {
        //CombatPieceManager.Instance.UnitsAreInteracting(this, target as UnitCombat);
    }
}
