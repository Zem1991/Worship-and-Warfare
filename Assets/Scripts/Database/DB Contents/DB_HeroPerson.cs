using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_HeroPerson : AbstractDBContent
{
    public string heroName;
    public Sprite profilePicture;

    [Header("References")]
    public DB_HeroUnit heroClass;
}
