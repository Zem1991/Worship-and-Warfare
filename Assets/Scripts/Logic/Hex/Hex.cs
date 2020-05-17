using System;
using System.Collections.Generic;

public class Hex
{
    public static Dictionary<HexDirection, Hex> Directions = new Dictionary<HexDirection, Hex>
    {
        { HexDirection.TOP_RIGHT, new Hex(1, 0, -1) },
        { HexDirection.RIGHT, new Hex(1, -1, 0) },
        { HexDirection.BOTTOM_RIGHT, new Hex(0, -1, 1) },
        { HexDirection.BOTTOM_LEFT, new Hex(-1, 0, 1) },
        { HexDirection.LEFT, new Hex(-1, 1, 0) },
        { HexDirection.TOP_LEFT, new Hex(0, 1, -1) }
    };

    public readonly int q;
    public readonly int r;
    public readonly int s;

    public Hex(int q, int r, int s)
    {
        if (q + r + s != 0) throw new ArgumentException("Cannot create hex: q + r + s must be 0");
        this.q = q;
        this.r = r;
        this.s = s;
    }

    public static Hex operator +(Hex a, Hex b)
    {
        return new Hex(a.q + b.q, a.r + b.r, a.s + b.s);
    }

    public static Hex operator -(Hex a, Hex b)
    {
        return new Hex(a.q - b.q, a.r - b.r, a.s - b.s);
    }

    public static Hex operator *(Hex a, int s)
    {
        return new Hex(a.q * s, a.r * s, a.s * s);
    }

    public static Hex Neighbour(Hex a, HexDirection dir)
    {
        return a + Directions[dir];
    }

    public static int Length(Hex a)
    {
        return (int)((Math.Abs(a.q) + Math.Abs(a.r) + Math.Abs(a.s)) / 2F);
    }

    public static int Distance(Hex a, Hex b)
    {
        return Length(a - b);
    }
}
