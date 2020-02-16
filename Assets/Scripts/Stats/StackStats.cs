using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackStats : MonoBehaviour
{
    [Header("Stack")]
    public int stack_current;
    public int stack_maximum;

    public void Initialize(int stack_maximum)
    {
        this.stack_maximum = stack_maximum;
        stack_current = this.stack_maximum;
    }
}
