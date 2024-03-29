﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_Tileset : AbstractDBContent
{
    [Header("Stats")]
    public int groundMovementCost = 100;
    public bool allowGroundMovement = true;
    public bool allowWaterMovement;
    public bool allowLavaMovement;
    //public bool allowAirMovement;     //if any movement type is allowed, air movement is possible as well

    [Header("Graphics")]
    public Sprite image;

    [Header("Combat Obstacles")]
    public List<DB_CombatObstacle> combatObstacles;
}
