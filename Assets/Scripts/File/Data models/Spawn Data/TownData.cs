﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownData
{
    public int[] mapPosition;
    public int ownerId;
    public PartyCompositionData garrison;
    public string factionId;
    public string townName;
    public List<TownBuildingData> townBuildings;
}
