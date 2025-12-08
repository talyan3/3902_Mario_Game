using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest;

public class StaticMovingMarioSprite : StaticSprite
{
    // See StaticMarioSprite for in-depth comments
    public Color Color { get; set; } = Color.White;
    public float Rotation { get; set; } = 0.0f;
    public Vector2 Scale { get; set; } = new Vector2(5,5);
    SpriteEffects Effects { get; set; } = SpriteEffects.None;
    public float LayerDepth { get; set; } = 0.0f;
    public Vector2 Origin { get; set; } = Vector2.Zero;
    public float Width => Region.Width * Scale.X;
    public float Height => Region.Height * Scale.Y;
    
    // Variables establishing position on screen
    public float xPos { get; set; } = 0;
    public float yPos { get; set; } = 0;

    public StaticMovingMarioSprite(GraphicsDevice graphicsDevice)
    {
        Texture2D texture = Texture2D.FromFile(graphicsDevice, "mario.png");
        Region = new TextureRegion(texture, 120, 0, 28, 28);
        CenterOrigin();
    }

    public StaticMovingMarioSprite(TextureRegion region)
    {
        Region = region;
    }
    public void CenterOrigin()
    {
        Origin = new Vector2(Region.Width, Region.Height) * 0.5f;
    }

    public override void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        Region.Draw(spriteBatch, position, Color, Rotation, Origin, Scale, Effects, LayerDepth);
    }

    public override void Update(GameTime gameTime)
    {
        Origin = new Vector2(++xPos, 0);
    }
}