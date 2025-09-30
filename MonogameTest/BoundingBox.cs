using System.Security.Cryptography.X509Certificates;

namespace MonogameTest;

// A box designated by two Location objects
public class BoundingBox
{
    // Top left of box
    public Location BoxStart { get; set; }
    // Bottom right of box
    public Location BoxEnd { get; set; }

    // Constructors
    public BoundingBox()
    {
        BoxStart = new Location();
        BoxEnd = new Location();
    }

    public BoundingBox(Location endLoc)
    {
        BoxStart = new Location();
        BoxEnd = endLoc;
    }

    public BoundingBox(Location startLoc, Location endLoc)
    {
        BoxStart = startLoc;
        BoxEnd = endLoc;
    }

    // Checks if a Location falls inside (or on the border of) this BoundingBox
    public bool locationIsWithinBox(Location loc)
    {
        return BoxStart.X <= loc.X && BoxStart.Y <= loc.Y && BoxEnd.X >= loc.X && BoxEnd.Y >= loc.Y;
    }

    public override bool Equals(object obj)
    {
        if (obj is BoundingBox)
        {
            BoundingBox compare = (BoundingBox)obj;
            if (BoxStart.Equals(compare.BoxStart) && BoxEnd.Equals(compare.BoxEnd)) return true;
        }
        return false;

    }

    public override int GetHashCode()
    {
        int res = 3;
        return res * 37 + BoxStart.GetHashCode() + BoxEnd.GetHashCode();
    }
}