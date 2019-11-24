using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTacticalAI : AbstractAIRoutine
{
    public override void ReadContext()
    {
        ReadYourUnits();
        ReadEnemyUnits();
    }

    public override void MakeCalculations()
    {
        CalculateYourZoneOfControl();
        CalculateEnemyZoneOfControl();
        CalculateYourUnitImportance();
        CalculateEnemyUnitImportance();
    }

    public override void TakeDecision()
    {
        //throw new NotImplementedException();
    }

    private void ReadYourUnits()
    {
        //throw new NotImplementedException();
    }

    private void ReadEnemyUnits()
    {
        //throw new NotImplementedException();
    }

    private void CalculateYourZoneOfControl()
    {
        //throw new NotImplementedException();
    }

    private void CalculateEnemyZoneOfControl()
    {
        //throw new NotImplementedException();
    }

    private void CalculateYourUnitImportance()
    {
        //throw new NotImplementedException();
    }

    private void CalculateEnemyUnitImportance()
    {
        //throw new NotImplementedException();
    }
}
