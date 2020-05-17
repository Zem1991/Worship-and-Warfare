using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpData : MonoBehaviour
{
    [Header("Level")]
    public int level;
    //public long experience;

    [Header("Attributes")]
    public int offense;
    public int defense;
    public int support;
    public int command;
    public int magic;
    public int tech;
}
