using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCombatPiece : AbstractCombatPiece
{
    [Header("Hero identification")]
    public string heroName;
    public string className;
    public int level;

    [Header("Hero attributes")]
    public int atrCommand;
    public int atrOffense;
    public int atrDefense;
    public int atrPower;
    public int atrFocus;

    [Header("Hero resources")]
    public int commandMax;
    public int commandUsed;
    public int manaMax;
    public int manaCurrent;

    public void Initialize(Hero hero)
    {
        heroName = hero.heroName;
        className = hero.className;
        level = hero.level;

        atrCommand = hero.atrCommand;
        atrOffense = hero.atrOffense;
        atrDefense = hero.atrDefense;
        atrPower = hero.atrPower;
        atrFocus = hero.atrFocus;

        commandMax = hero.commandMax;
        commandUsed = hero.commandUsed;
        manaMax = hero.manaMax;
        manaCurrent = hero.manaCurrent;

        imgProfile = hero.imgProfile;
        SetAnimatorOverrideController(hero.animatorCombat);
    }

    protected override void InteractWithPiece(AbstractPiece target)
    {
        Debug.LogWarning("THIS HERO DOESN'T KNOW HOW TO HANDLE OTHER PIECES!");
        //CombatPieceManager.Instance.UnitsAreInteracting(this, target as HeroCombatPiece);
    }

    public override int CalculateDamage()
    {
        throw new System.NotImplementedException();
    }

    public override bool TakeDamage(float amount)
    {
        throw new System.NotImplementedException();
    }

    public override void Die()
    {
        throw new System.NotImplementedException();
    }
}
