using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownInputListener : AbstractInputListener
{
    public KeyCode kbm_exitTown = KeyCode.Escape;
    public KeyCode kbm_buildStructure = KeyCode.W;
    public KeyCode kbm_recruitHero = KeyCode.S;
    public KeyCode kbm_recruitCreature = KeyCode.D;

    public bool ExitTownDown()
    {
        bool kbm = Input.GetKeyDown(kbm_exitTown);
        return kbm;
    }

    public bool BuildStructureDown()
    {
        bool kbm = Input.GetKeyDown(kbm_buildStructure);
        return kbm;
    }

    public bool RecruitHeroDown()
    {
        bool kbm = Input.GetKeyDown(kbm_recruitHero);
        return kbm;
    }

    public bool RecruitCreatureDown()
    {
        bool kbm = Input.GetKeyDown(kbm_recruitCreature);
        return kbm;
    }
}
