using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MonogameTest;

    public static class AnimationLoader
    {
        public static Dictionary<string, Animation> LoadPlayerAnimations(ContentManager content)
        {
            // Load your textures
            Texture2D idleTexture = content.Load<Texture2D>("Sprites/player_idle");
            Texture2D runTexture = content.Load<Texture2D>("Sprites/player_run");
            Texture2D jumpTexture = content.Load<Texture2D>("Sprites/player_jump");

            // Build and return the dictionary
            return new Dictionary<string, Animation>
            {
                { "Idle", new Animation(idleTexture, 32, 32, 4, 0.15f, true) },
                { "Run", new Animation(runTexture, 32, 32, 6, 0.1f, true) },
                { "Jump", new Animation(jumpTexture, 32, 32, 2, 0.2f, false) }
            };
        }
    }

