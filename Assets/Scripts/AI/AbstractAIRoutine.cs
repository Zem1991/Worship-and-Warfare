using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractAIRoutine : MonoBehaviour
{
    protected AIPersonality aiPersonality;

    public void Awake()
    {
        aiPersonality = GetComponentInParent<AIPersonality>();
    }

    public void FullRoutine()
    {
        ReadContext();
        MakeCalculations();
        TakeDecision();
    }

    public abstract void ReadContext();
    public abstract void MakeCalculations();
    public abstract void TakeDecision();
}
