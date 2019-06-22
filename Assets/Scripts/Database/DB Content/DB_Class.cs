using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_Class : DBContent
{
    public new string name;
    public DB_Faction faction;

    [Header("Stats")]
    public int command;
    public int offense;
    public int defense;
    public int power;
    public int knowledge;
    //public int logistics;     //really needed?

    [Header("Graphics")]
    public Sprite fieldPicture;
    public Sprite combatPicture;
}
