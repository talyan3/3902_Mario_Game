using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest.Managers
{
    public static class AssetLoader
    {
        public static Texture2D LoadTexture(GraphicsDevice device, string normal, string christmas)
        {
            string path = Game1.ChristmasMode ? christmas : normal;
            return Texture2D.FromFile(device, path);
        }
    }
}
