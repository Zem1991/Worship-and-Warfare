using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_Faction : AbstractDBContent
{
    public string factionName;
    public FactionTree factionTree;
    //public Sprite image;

    [Header("Town parameters")]
    public List<string> townNames = new List<string>();
    public Sprite townFieldSprite;
    //public AnimatorOverrideController townAnimator;

    //[Header("Tech tree")]
    //public List<DB_TownBuilding> buildings = new List<DB_TownBuilding>();
    //public List<DB_Unit> units = new List<DB_Unit>();
}
