using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackHealthStats2 : HealthStats2
{
    [SerializeField] private int stackSize;
    [SerializeField] private int previousStackSize;

    public override void CopyFrom(HealthStats2 healthStats)
    {
        base.CopyFrom(healthStats);

        StackHealthStats2 other = healthStats as StackHealthStats2;
        if (other)
        {
            stackSize = other.stackSize;
            previousStackSize = stackSize;
        }
    }

    //public void Add(int amount, bool increaseMaximum, bool overflow)
    public void AddToStack(int amount)
    {
        previousStackSize = stackSize;
        //if (increaseMaximum) stack_maximum += amount;
        //if (!overflow) amount = Mathf.Clamp(amount, 0, stack_maximum - stack);
        stackSize += amount;
    }

    //public bool Subtract(int amount, bool decreaseMaximum)
    public bool SubtractFromStack(int amount)
    {
        previousStackSize = stackSize;
        stackSize = Mathf.Clamp(stackSize, 0, stackSize - amount);
        //if (decreaseMaximum) stack_maximum = Mathf.Clamp(stack_maximum, 0, stack_maximum - amount);
        return stackSize <= 0;
    }

    public int GetStackSize()
    {
        return stackSize;
    }

    public override bool ReceiveHealing(int amount)
    {
        //TODO CHANGE THIS
        hitPoints_current += amount;
        hitPoints_current = Mathf.Clamp(hitPoints_current, 0, hitPoints_maximum);
        return hitPoints_current >= hitPoints_maximum;
    }

    public override bool ReceiveDamage(int amount)
    {
        int stackLost = amount / hitPoints_maximum;
        int hpLost = amount % hitPoints_maximum;
        hitPoints_current -= hpLost;
        if (hitPoints_current <= 0)
        {
            stackLost++;
            hitPoints_current += hitPoints_maximum;
        }
        if (stackLost > stackSize)
        {
            stackLost = stackSize;
        }
        return SubtractFromStack(stackLost);
    }
}
