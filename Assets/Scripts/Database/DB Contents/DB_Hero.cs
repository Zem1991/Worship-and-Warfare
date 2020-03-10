using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_Hero : AbstractDBContent
{
    public string heroName;
    public Sprite profilePicture;

    [Header("References")]
    public DB_HeroClass heroClass;
}
