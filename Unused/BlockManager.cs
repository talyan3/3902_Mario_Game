using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest
{
    public class BlockManager
    {
        private Texture2D texture;
        private int currentIndex = 0;
        private const int BlockSize = 16;
        private const int TotalBlocks = 7; // number of blocks in your spritesheet

        public BlockManager(Texture2D texture)
        {
            this.texture = texture;
        }

        public void NextBlock()
        {
            currentIndex = (currentIndex + 1) % TotalBlocks;
        }

        public void PreviousBlock()
        {
            currentIndex = (currentIndex - 1 + TotalBlocks) % TotalBlocks;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            // source rectangle for current block
            Rectangle source = new Rectangle(currentIndex * BlockSize, 0, BlockSize, BlockSize);

            spriteBatch.Draw(texture, position, source, Color.White, 
                             0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
        }
    }
}
