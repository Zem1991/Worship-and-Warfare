using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_Ability : AbstractDBContent
{
    public new string name;
    public Sprite sprite;

    [Header("Action")]
    public AES_Action action;
}
