//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class StackStats : MonoBehaviour
//{
//    [Header("Stack")]
//    [SerializeField] private int stack;
//    //[SerializeField] private int stack_maximum;

//    public void Initialize(int stack)
//    {
//        this.stack = stack;
//        //stack_maximum = amount;
//    }

//    //public void Add(int amount, bool increaseMaximum, bool overflow)
//    public void Add(int amount)
//    {
//        //if (increaseMaximum) stack_maximum += amount;
//        //if (!overflow) amount = Mathf.Clamp(amount, 0, stack_maximum - stack);
//        stack += amount;
//    }

//    //public bool Subtract(int amount, bool decreaseMaximum)
//    public bool Subtract(int amount)
//    {
//        stack = Mathf.Clamp(stack, 0, stack - amount);
//        //if (decreaseMaximum) stack_maximum = Mathf.Clamp(stack_maximum, 0, stack_maximum - amount);
//        return stack > 0;
//    }

//    public int Get()
//    {
//        return stack;
//    }

//    //public int GetMaximum()
//    //{
//    //    return stack_maximum;
//    //}
//}
