using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownInputInterpreter : AbstractInputInterpreter<TownInputListener>
{
    [Header("Inputs")]
    public bool exitTown;
    public bool buildStructure;
    public bool recruitHero;
    public bool recruitCreature;

    // Start is called before the first frame update
    void Start()
    {
        listener = GetComponent<TownInputListener>();
    }

    // Update is called once per frame
    void Update()
    {
        exitTown = listener.ExitTownDown();
        buildStructure = listener.BuildStructureDown();
        recruitHero = listener.RecruitHeroDown();
        recruitCreature = listener.RecruitCreatureDown();
    }

    public void ClearInputs()
    {
        exitTown = false;
        buildStructure = false;
        recruitHero = false;
        recruitCreature = false;
    }
}
