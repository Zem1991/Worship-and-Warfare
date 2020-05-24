using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsStats2 : MonoBehaviour
{
    public bool canWait = true;
    public bool canDefend = true;
    public bool canRetaliate = true;
    public bool canCounter = false;
    public int retaliationsMax = 1;

    public void CopyFrom(SettingsStats2 settingsStats)
    {
        canWait = settingsStats.canWait;
        canDefend = settingsStats.canDefend;
        canRetaliate = settingsStats.canRetaliate;
        canCounter = settingsStats.canCounter;
        retaliationsMax = settingsStats.retaliationsMax;
    }
}
