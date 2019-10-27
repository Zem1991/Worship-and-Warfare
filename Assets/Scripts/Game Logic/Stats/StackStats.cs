using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackStats : MonoBehaviour
{
    [Header("Stack")]
    public int stack_current;
    public int stack_maximum;

    public void Initialize(StackData stackData)
    {
        stack_maximum = stackData.stack_maximum;
        stack_current = stack_maximum;
    }

    public void Initialize(StackStats stackStats)
    {
        stack_maximum = stackStats.stack_maximum;
        stack_current = stack_maximum;
    }
}
