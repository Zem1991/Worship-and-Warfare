using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_Faction : AbstractDBContent
{
    public string factionName;
    //public Sprite image;

    [Header("Town parameters")]
    public List<string> townNames = new List<string>();
    public Sprite townFieldSprite;
    //public AnimatorOverrideController townAnimator;

    [Header("Town Building parameters")]
    public List<DB_TownBuilding> townBuildings = new List<DB_TownBuilding>();
}
