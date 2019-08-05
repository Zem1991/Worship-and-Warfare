using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCombatPiece : AbstractCombatPiece
{
    public string heroName;
    public string className;
    public int level;

    public int atrCommand;
    public int atrOffense;
    public int atrDefense;
    public int atrPower;
    public int atrFocus;

    public int commandMax;
    public int commandUsed;
    public int manaMax;
    public int manaCurrent;

    public Sprite imgProfile;
    public Sprite imgCombat;

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
        imgCombat = hero.imgCombat;

        ChangeSprite(imgCombat);
    }

    public override void InteractWithPiece(AbstractPiece target)
    {
        throw new System.NotImplementedException();
    }
}
