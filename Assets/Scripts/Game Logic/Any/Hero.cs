using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public DB_Hero dbData;

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
    public AnimatorOverrideController animatorField;
    public AnimatorOverrideController animatorCombat;

    public Inventory inventory;

    public void Initialize(HeroData heroData, DB_Hero dbData)
    {
        this.dbData = dbData;
        DB_Class classs = dbData.classs;

        Inventory prefabInventory = AllPrefabs.Instance.inventory;

        heroName = dbData.heroName;
        className = classs.className;

        int level = heroData.level;
        this.level = Mathf.Clamp(level, 1, 20);
        //this.experience = experience; //TODO add automatic experience calculator

        //atrCommand = classs.atrCommand;
        //atrOffense = classs.atrOffense;
        //atrDefense = classs.atrDefense;
        //atrPower = classs.atrPower;
        //atrFocus = classs.atrFocus;

        //commandMax = DBFormulas.CommandMax(atrCommand);
        ////this.commandUsed = commandUsed;   //TODO set commandUsed in the piece, with automatic calculator
        //manaMax = DBFormulas.ManaMax(atrFocus);
        //manaCurrent = manaMax;
        //moveMax = DBFormulas.MoveMax();
        //moveCurrent = moveMax;

        imgProfile = dbData.profilePicture;
        animatorField = classs.animatorField;
        animatorCombat = classs.animatorCombat;

        inventory = Instantiate(prefabInventory, transform);
        inventory.Initialize(heroData.inventory, this);
    }

    public void RecalculateParameters()
    {
        DB_Class classs = dbData.classs;

        atrCommand = classs.atrCommand + inventory.atrCommand;
        atrOffense = classs.atrCommand + inventory.atrOffense;
        atrDefense = classs.atrCommand + inventory.atrDefense;
        atrPower = classs.atrCommand + inventory.atrPower;
        atrFocus = classs.atrCommand + inventory.atrFocus;
    }
}
