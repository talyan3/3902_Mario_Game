//logic: detecting if collision happends between a animated sprite 
// and static/animates sprites using the rectangle method
// check what type of collision it is (left/right or top/bottom)

//NOTES: try sort and sweep? or quadtrees?

public enum typeCollision
{
    None,
    Top,
    Bottom,
    Left,
    Right
}
public class DetectCollisions : Game1
{
    Rectangle pRect;
    Rectangle sRect;

    int xValue = pRect.X - sRect.X;
    int yValue = pRect.Y - sRect.Y;


    //make a collision object? or enum? to have it return? FIX
    public typeCollision(Rectangle pRect, Rectangle sRect)
    {
        if (pRect.Intersects(sRect))
        {
            //the length of the overlap
            int overlapLeft = pRect.Right - sRect.Left;
            int overlapRight = sRect.Right - pRect.Left;
            int overlapTop = pRect.Bottom - sRect.Top;
            int overlapBottom = sRect.Bottom - pRect.Top;

            if(Math.Abs(xValue) > Math.Abs(yValue)) //horizontal collision
            {
                if (overlapLeft < overlapRight)
                {
                    return typeCollision.Left; //player hit static from left
                }
                else
                {
                    return typeCollision.Right; //player hit static from right
                }
            }
            else //vertical collision
            {
                if (overlapTop < overlapBottom)
                {
                    return typeCollision.Top; //player hit static from top
                }
                else
                {
                    return typeCollision.Bottom; //player hit static from bottom
                }
            }
            
        }
        return typeCollision.None; //no collision
    }


}
