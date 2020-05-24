using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_Ability : AbstractDBContent
{
    [Header("Ability identification")]
    public new string name;
    public Sprite sprite;

    [Header("Action identification")]
    public AES_Action action;
}
