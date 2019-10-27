using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerOwnable
{
    bool IPO_HasOwner();
    Player IPO_GetOwner();
}
