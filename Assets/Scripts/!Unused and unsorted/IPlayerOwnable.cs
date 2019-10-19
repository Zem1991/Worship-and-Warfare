using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerOwnable
{
    Player playerOwner { get; set; }
}
