using Microsoft.Xna.Framework;
using Sustem.Collections.Generic;

//I did research on quadtrees and my code is inspired by badecho.com
//where he talked about collision and implementing quadtrees for C$

public class QuadTree
{
    int maxObjects = 4;
    int maxLevels = 5;

    int level;
    List<ICollidable> objects;
    Rectangle bounds;
    QuadTree[] nodes;

    public QuadTree(int pLevel, Rectangle pBounds)
    {
        this.level = pLevel;
        this.objects = new List<ICollidable>();
        this.bounds = pBounds;
        this.nodes = new QuadTree[maxObjects];
    }

    public void Clear()
    {
        objects.Clear();

        for (int i = 0; i < nodes.Length; i++)
        {
            if (nodes[i] != null)
            {
                nodes[i].Clear();
                nodes[i] = null;
            }
        }
    }

    private void Split()
    {
        int subWidth = bounds.Width / 2;
        int subHeight = bounds.Height / 2;
        int x = bounds.X;
        int y = bounds.Y;

        nodes[0] = new QuadTree(level + 1, new Rectangle(x + subWidth, y, subWidth, subHeight));
        nodes[1] = new QuadTree(level + 1, new Rectangle(x, y, subWidth, subHeight));
        nodes[2] = new QuadTree(level + 1, new Rectangle(x, y + subHeight, subWidth, subHeight));
        nodes[3] = new QuadTree(level + 1, new Rectangle(x + subWidth, y + subHeight, subWidth, subHeight));
    }

    private int GetIndex(Rectangle bounds)
    {
        int index = -1;
        double verticalMidpoint = this.bounds.X + (this.bounds.Width / 2);
        double horizontalMidpoint = this.bounds.Y + (this.bounds.Height / 2);

        bool topQuadrant = (bounds.Y < horizontalMidpoint && bounds.Y + bounds.Height < horizontalMidpoint);
        bool bottomQuadrant = (bounds.Y > horizontalMidpoint);

        if (bounds.X < verticalMidpoint && bounds.X + bounds.Width < verticalMidpoint)
        {
            if (topQuadrant)
            {
                index = 1;
            }
            else if (bottomQuadrant)
            {
                index = 2;
            }
        }
        else if (bounds.X > verticalMidpoint)
        {
            if (topQuadrant)
            {
                index = 0;
            }
            else if (bottomQuadrant)
            {
                index = 3;
            }
        }

        return index;
    }

    public void Insert(ICollidable collidable)
    {
        if (nodes[0] != null)
        {
            int index = GetIndex(collidable.BoundingBox);

            if (index != -1)
            {
                nodes[index].Insert(collidable);
                return;
            }
        }

        objects.Add(collidable);

        if (objects.Count > maxObjects && level < maxLevels)
        {
            if (nodes[0] == null)
            {
                Split();
            }

            int i = 0;
            while (i < objects.Count)
            {
                int index = GetIndex(objects[i].BoundingBox);
                if (index != -1)
                {
                    nodes[index].Insert(objects[i]);
                    objects.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }
    }

    public List<ICollidable> Retrieve(List<ICollidable> returnObjects, Rectangle bounds)
    {
        int index = GetIndex(bounds);
        if (index != -1 && nodes[0] != null)
        {
            nodes[index].Retrieve(returnObjects, bounds);
        }

        returnObjects.AddRange(objects);

        return returnObjects;
    }




}