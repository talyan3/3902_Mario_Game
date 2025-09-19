using System;
using System.Security.Cryptography.X509Certificates;

namespace MonogameTest;

public class Location
{
    public int X { get; set; }
    public int Y { get; set; }

    public Location()
    {
        X = 0;
        Y = 0;
    }

    public Location(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override bool Equals(Object obj)
    {
        if (obj is Location)
        {
            Location compare = (Location)obj;
            if (compare.X == X && compare.Y == Y) return true;
        }
        return false;
    }

    public override int GetHashCode()
    {
        int res = 31;
        return res * 37 + X + Y;
    }
}