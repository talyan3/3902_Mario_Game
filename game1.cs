using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonogameTest;

public class Game1 : Game
{
	private GraphicsDeviceManager _graphics;
	private SpriteBatch _spriteBatch;
	public MarioManager MarioManager { get; set; } = new MarioManager();
	public CommandManager CommandManager { get; set; }

	private SmallMarioSprite _smallMario;
	private BigMarioSprite _bigMario;
	private StaticSprite _currentMario;
	private bool _isBig = false;
	private bool _bHeldLast = false;

	public Game1()
	{
		_graphics = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
		IsMouseVisible = true;
	}

	protected override void Initialize()
	{
		CommandManager = new CommandManager(this, MarioManager);
		base.Initialize();
	}

	protected override void LoadContent()
	{
		_spriteBatch = new SpriteBatch(GraphicsDevice);

		new SpriteCommand(GraphicsDevice, MarioManager).Execute();

		// load both mario sprites so i can switch between them
		_smallMario = new SmallMarioSprite(GraphicsDevice);
		_bigMario = new BigMarioSprite(GraphicsDevice);

		// set both at the same starting position
		var vp = GraphicsDevice.Viewport;
		var pos = new Vector2(vp.Width * 0.5f, vp.Height * 0.85f);

		_smallMario.Position = pos;
		_bigMario.Position = pos;

		// start the game with small mario active
		_currentMario = _smallMario;
	}

	protected override void Update(GameTime gameTime)
	{
		var kb = Keyboard.GetState();

		if (kb.IsKeyDown(Keys.Escape))
			Exit();

		CommandManager.checkKeys();
		CommandManager.checkClicks();

		// press B to toggle between small and big mario
		bool bDown = kb.IsKeyDown(Keys.B);
		if (bDown && !_bHeldLast)
			ToggleMarioSize();
		_bHeldLast = bDown;

		// update the active mario sprite
		_currentMario.Update(gameTime);

		if (MarioManager.ActiveSprite != null)
			MarioManager.ActiveSprite.Update(gameTime);

		base.Update(gameTime);
	}

	private void ToggleMarioSize()
	{
		_isBig = !_isBig;

		// keep mario in the same spot when switching forms
		Vector2 pos = (_currentMario is SmallMarioSprite sm ? sm.Position :
					   (_currentMario is BigMarioSprite bm ? bm.Position : Vector2.Zero));

		_currentMario = _isBig ? (StaticSprite)_bigMario : _smallMario;

		if (_currentMario is SmallMarioSprite sm2) sm2.Position = pos;
		if (_currentMario is BigMarioSprite bm2) bm2.Position = pos;
	}

	protected override void Draw(GameTime gameTime)
	{
		GraphicsDevice.Clear(Color.CornflowerBlue);

		_spriteBatch.Begin();

		// only draw whichever mario is currently active
		_currentMario.Draw(_spriteBatch, Vector2.Zero);

		if (MarioManager.ActiveSprite != null)
		{
			int h = GraphicsDevice.Viewport.Height;
			int w = GraphicsDevice.Viewport.Width;
			MarioManager.ActiveSprite.Draw(_spriteBatch, new Vector2(w / 2, h / 2));
		}

		_spriteBatch.End();

		base.Draw(gameTime);
	}
}
