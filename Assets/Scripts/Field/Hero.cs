using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero
{
    public int dbId;
    public string heroName;
    public string className;

    public int level;
    public long experience;

    public int atrCommand;
    public int atrOffense;
    public int atrDefense;
    public int atrPower;
    public int atrFocus;

    public int commandMax;
    public int commandUsed;
    public int manaMax;
    public int manaCurrent;
    public int moveMax;
    public int moveCurrent;

    public Sprite imgProfile;
    public Sprite imgField;
    public Sprite imgCombat;

    public Hero(int dbId, DB_Hero dbData, int level = 1, long experience = 0, int commandUsed = 0)
    {
        DB_Class classs = dbData.classs;

        this.dbId = dbId;
        heroName = dbData.name;
        className = classs.className;

        this.level = level;
        this.experience = experience;

        atrCommand = classs.command;
        atrOffense = classs.offense;
        atrDefense = classs.defense;
        atrPower = classs.power;
        atrFocus = classs.focus;

        commandMax = DBFormulas.CommandMax(atrCommand);
        this.commandUsed = commandUsed;
        manaMax = DBFormulas.ManaMax(atrFocus);
        manaCurrent = manaMax;
        moveMax = DBFormulas.MoveMax();
        moveCurrent = moveMax;

        imgProfile = dbData.profilePicture;
        imgField = classs.fieldPicture;
        imgCombat = classs.combatPicture;
    }
}
