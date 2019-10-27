using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_Hero : DBContent
{
    public string heroName;
    public Sprite profilePicture;

    [Header("References")]
    public DB_Class classs;
}
