using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CreatureCalculation
{
    public static int CommandMax(int command)
    {
        return 200 + (command * 100);
    }

    public static int ManaMax(int focus)
    {
        return 20 + (focus * 10);
    }

    public static int MoveMax()
    {
        return 1500;
    }
}
