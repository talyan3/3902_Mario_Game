using System.Collections.Generic;
using Microsoft.Xna.Framework;

using System.IO;
using MonogameTest;

public static class EnemyPositions
{
    public static readonly Vector2[] Goombas = new Vector2[]
    {
        new Vector2(22, 12),
        new Vector2(39, 12),
        new Vector2(50, 12),
        new Vector2(52, 12),
        new Vector2(79, 4),   // these goombas spawn elevated, so having no gravity is a problem.
        new Vector2(81, 4),
        new Vector2(96, 12),
        new Vector2(98, 12),
        new Vector2(113, 12),
        new Vector2(115, 12),
        new Vector2(124, 12),
        new Vector2(126, 12),
        new Vector2(128, 12),
        new Vector2(130, 12),
        new Vector2(173, 12),
        new Vector2(175, 12)
    };
}