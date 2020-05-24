using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStats2 : MonoBehaviour
{
    public int initiative;
    public int movementRange;
    public MovementType movementType;

    public void CopyFrom(MovementStats2 movementStats)
    {
        initiative = movementStats.initiative;
        movementRange = movementStats.movementRange;
        movementType = movementStats.movementType;
    }

    //public void Initialize(int initiative, int movementRange, MovementType movementType)
    //{
    //    this.initiative = initiative;
    //    this.movementRange = movementRange;
    //    this.movementType = movementType;
    //}
}
