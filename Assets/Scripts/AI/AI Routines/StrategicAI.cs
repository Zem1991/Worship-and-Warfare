using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrategicAI : AbstractAIRoutine
{
    public override void ReadContext()
    {
        ReadVictoryConditions();
        ReadDefeatConditions();
        ReadTurnAndTimeConditions();
        ReadResourcesConditions();
    }

    public override void MakeCalculations()
    {
        CalculateVictory();
        CalculateDefeat();
    }

    public override void TakeDecision()
    {
        //throw new NotImplementedException();
    }

    private void ReadVictoryConditions()
    {
        //throw new NotImplementedException();
    }

    private void ReadDefeatConditions()
    {
        //throw new NotImplementedException();
    }

    private void ReadTurnAndTimeConditions()
    {
        //throw new NotImplementedException();
    }

    private void ReadResourcesConditions()
    {
        //throw new NotImplementedException();
    }

    private void CalculateVictory()
    {
        //throw new NotImplementedException();
    }

    private void CalculateDefeat()
    {
        //throw new NotImplementedException();
    }
}
