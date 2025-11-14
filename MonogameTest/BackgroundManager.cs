using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Net.Mime;

namespace MonogameTest;

public class BackgroundManager
{
    private readonly GraphicsDevice _graphicsDevice;
    private readonly List<BackgroundElement> _elements = new();

    // Store texture references here
    private Texture2D cloud1, cloud2, cloud3;
    private Texture2D bush1, bush2, bush3;
    private Texture2D hillSmall, hillBig;
    private ContentManager _content;

    public BackgroundManager(GraphicsDevice graphicsDevice, ContentManager content)
    {
        _graphicsDevice = graphicsDevice;
        _content = content;
    }

    public void LoadContent()
    {
        // Load textures once
        cloud1 = _content.Load<Texture2D>("Sprites/BGSprites/Cloud1");
        cloud2 = _content.Load<Texture2D>("Sprites/BGSprites/Cloud2");
        cloud3 = _content.Load<Texture2D>("Sprites/BGSprites/Cloud3");
        bush1 = _content.Load<Texture2D>("Sprites/BGSprites/Bush1");
        bush2 = _content.Load<Texture2D>("Sprites/BGSprites/Bush2");
        bush3 = _content.Load<Texture2D>("Sprites/BGSprites/Bush3");
        hillSmall = _content.Load<Texture2D>("Sprites/BGSprites/SmallHill");
        hillBig = _content.Load<Texture2D>("Sprites/BGSprites/BigHill");
        //cloud1 = Texture2D.FromFile(_graphicsDevice, "Cloud1.png");
        //cloud2 = Texture2D.FromFile(_graphicsDevice, "Cloud2.png");
        //cloud3 = Texture2D.FromFile(_graphicsDevice, "Cloud3.png");
        //bush1 = Texture2D.FromFile(_graphicsDevice, "Bush1.png");
        //bush2 = Texture2D.FromFile(_graphicsDevice, "Bush2.png");
        //bush3 = Texture2D.FromFile(_graphicsDevice, "Bush3.png");
        //hillSmall = Texture2D.FromFile(_graphicsDevice, "SmallHill.png");
        //hillBig = Texture2D.FromFile(_graphicsDevice, "BigHill.png");

        // Add decorative elements to the list
        _elements.AddRange(new[]
        {
            // Hills + bushes
            new BackgroundElement(hillBig,   new Vector2(0,   173)),
            new BackgroundElement(bush3,     new Vector2(184, 192)),
            new BackgroundElement(hillSmall, new Vector2(256, 189)),
            new BackgroundElement(bush1,     new Vector2(360, 192)),
            new BackgroundElement(bush2,     new Vector2(664, 192)),
            new BackgroundElement(hillBig,   new Vector2(768, 173)),
            new BackgroundElement(bush3,     new Vector2(952, 192)),
            new BackgroundElement(hillSmall, new Vector2(1024, 189)),
            new BackgroundElement(bush1,     new Vector2(1144, 192)),
            new BackgroundElement(bush2,     new Vector2(1432, 192)),
            new BackgroundElement(hillBig,   new Vector2(1536, 173)),
            new BackgroundElement(bush3,     new Vector2(1720, 192)),
            new BackgroundElement(hillSmall, new Vector2(1792, 189)),
            new BackgroundElement(bush1,     new Vector2(1912, 192)),
            new BackgroundElement(bush2,     new Vector2(2200, 192)),
            new BackgroundElement(hillBig,   new Vector2(2304, 173)),
            new BackgroundElement(hillSmall, new Vector2(2560, 189)),
            new BackgroundElement(bush1,     new Vector2(2680, 192)),
            new BackgroundElement(hillBig,   new Vector2(3072, 173)),

            // Clouds
            new BackgroundElement(cloud1, new Vector2(136, 50)),
            new BackgroundElement(cloud1, new Vector2(300, 25)),
            new BackgroundElement(cloud3, new Vector2(425, 50)),
            new BackgroundElement(cloud2, new Vector2(575, 25)),
            new BackgroundElement(cloud1, new Vector2(925, 50)),
            new BackgroundElement(cloud1, new Vector2(1100, 25)),
            new BackgroundElement(cloud3, new Vector2(1225, 50)),
            new BackgroundElement(cloud2, new Vector2(1375, 25)),
            new BackgroundElement(cloud1, new Vector2(1675, 50)),
            new BackgroundElement(cloud1, new Vector2(1850, 25)),
            new BackgroundElement(cloud3, new Vector2(1975, 50)),
            new BackgroundElement(cloud2, new Vector2(2125, 25)),
            new BackgroundElement(cloud1, new Vector2(2450, 50)),
            new BackgroundElement(cloud1, new Vector2(2625, 25)),
            new BackgroundElement(cloud3, new Vector2(2725, 50)),
            new BackgroundElement(cloud2, new Vector2(2875, 25)),
            new BackgroundElement(cloud1, new Vector2(3207, 50))
        });
    }

    public void Draw(SpriteBatch spriteBatch, float cameraX = 0f)
    {
        foreach (var element in _elements)
        {
            // Subtract camera X if you want the background to scroll
            spriteBatch.Draw(element.Texture, new Vector2(element.Position.X - cameraX, element.Position.Y), element.Tint);
        }
    }
}
