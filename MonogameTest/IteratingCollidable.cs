using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Runtime.CompilerServices;

// Builds two quadtrees (static & animated) and returns what to test
// The pairs are (animated, other) where other is either a static or another animated object
public class IteratingCollidable
{
    private List<ICollidable> staticObjects;
    private List<ICollidable> animatedObjects;

    public IteratingCollidable()
    {
        staticObjects = new List<ICollidable>();
        animatedObjects = new List<ICollidable>();
    }

    public void SetStaticObjects(IEnumerable<ICollidable> statics)
    {
        staticObjects = new List<ICollidable>(statics ?? new ICollidable[0]);
    }

    public void SetAnimatedObjects(IEnumerable<ICollidable> animated)
    {
        animatedObjects = new List<ICollidable>(animated ?? new ICollidable[0]);
    }


    public List<(ICollidable, ICollidable)> GetCollisionPairs(Rectangle cameraBound)
    {
        var pairs = new List<(ICollidable, ICollidable)>();

        var staticTree = new QuadTree(0, cameraBound);
        var animatedTree = new QuadTree(0, cameraBound);

        // Insert objects into their respective trees
        foreach (var s in staticObjects)
        {
            if (s != null)
                staticTree.Insert(s);
        }

        foreach (var a in animatedObjects)
        {
            if (a != null)
                animatedTree.Insert(a);
        }

        foreach (var animated in animatedObjects)
        {
            if (animated == null) continue;

            // Animated vs Static
            var possibleStatics = staticTree.Retrieve(new List<ICollidable>(), animated.BoundingBox);
            foreach (var s in possibleStatics)
            {
                if (s == null) continue;
                pairs.Add((animated, s));
            }

            // Animated vs Animated
            var possibleAnimated = animatedTree.Retrieve(new List<ICollidable>(), animated.BoundingBox);
            int idA = RuntimeHelpers.GetHashCode(animated);
            foreach (var other in possibleAnimated)
            {
                if (other == null) continue;
                if (ReferenceEquals(other, animated)) continue; // skip self

                int idB = RuntimeHelpers.GetHashCode(other);
                
                if (idA < idB)
                {
                    pairs.Add((animated, other));
                }
            }
        }

        return pairs;
    }
}