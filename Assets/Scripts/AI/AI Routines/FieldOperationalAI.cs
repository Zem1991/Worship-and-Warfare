using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOperationalAI : AbstractAIRoutine
{
    public override void ReadContext()
    {
        //throw new NotImplementedException();
    }

    public override void MakeCalculations()
    {
        //throw new NotImplementedException();
    }

    public override void TakeDecision()
    {
        //TODO ACTUAL DECISIONS
        GameManager.Instance.EndTurn();
    }
}
