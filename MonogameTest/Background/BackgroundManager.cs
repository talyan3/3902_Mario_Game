using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq.Expressions;
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

    private Dictionary<string, Texture2D> _textures;

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



        // Add decorative elements to the list

        foreach (var element in NumberLoad.Numbers.BackgroundElements)
        {
            Texture2D texture = _textures[element.Type];
            _elements.Add(new BackgroundElement(texture, new Vector2(element.X, element.Y)));
        }
        
    }

    public void Draw(SpriteBatch spriteBatch, float cameraX = 0f)
    {
        foreach (var element in _elements)
        {
            spriteBatch.Draw(
                element.Texture,
                new Vector2(element.Position.X - cameraX, element.Position.Y),
                element.Tint
            );
        }
    }
}
