using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Xsl;
using jeu.Core.Classes.Controller;
using jeu.Core.Classes.Model;
using jeu.Core.Classes.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace jeu.Core
{
	public enum GameState
	{
		MainMenu,
		LevelSelect,
		Playing
	}

	public class JeuGame : Game
	{
		// Do not remove this field even if it seems unused
		private readonly GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;

		private PlayerProfile playerProfile;
		private SaveManager saveManager;
		private GameState currentState;

		private List<PlayerProfile> playerProfiles = [];

		private GameManager gameManager;

		private SpriteFont font;
		private Texture2D carTexture;
		private Texture2D bgTexture;
		private Texture2D bgLevelTexture;
		private Texture2D enemySprite;

		private StartScreen startScreen;
		private LevelMenuScreen levelMenuScreen;
		private ScreenManager screenManager;

		private KeyboardState previousKeyboardState = Keyboard.GetState();

		public JeuGame()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			gameManager = new GameManager((level, time, lives) =>
			{
				saveManager.CompleteLevel(playerProfile, level, time, lives);
			});
			currentState = GameState.MainMenu;
			startScreen = new StartScreen();
			levelMenuScreen = new LevelMenuScreen(font, [], [], null, null);
			screenManager = new ScreenManager(currentState, startScreen, levelMenuScreen);

			TextureCache.Initialize(GraphicsDevice);
			base.Initialize();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			font = Content.Load<SpriteFont>("Default");
			carTexture = Content.Load<Texture2D>("Sprites/Car");
			bgTexture = Content.Load<Texture2D>("Sprites/BG");
			bgLevelTexture = Content.Load<Texture2D>("Sprites/LevelBG");
			enemySprite = Content.Load<Texture2D>("Sprites/Enemy");

			gameManager.Load(carTexture, bgLevelTexture, enemySprite);
			startScreen.LoadContent(bgTexture);

			saveManager = new SaveManager();

			playerProfiles = saveManager.LoadAllProfiles();
			playerProfile = playerProfiles.Count > 0 ? playerProfiles[0] : new PlayerProfile
			{
				Id = Guid.NewGuid().ToString(),
				Name = "Player1",
				CreationDate = DateTime.Now
			};

			currentState = GameState.MainMenu;
		}

		protected override void Update(GameTime gameTime)
		{
			KeyboardState k = Keyboard.GetState();

			if (k.IsKeyDown(Keys.Escape)) Exit();

			if (!previousKeyboardState.IsKeyDown(Keys.F5) && k.IsKeyDown(Keys.F5)) exportProfile();

			if (currentState == GameState.Playing)
			{
				float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

				if (k.IsKeyDown(Keys.Up)) gameManager.car.Accelerate(dt);
				else if (k.IsKeyDown(Keys.Down)) gameManager.car.Decelerate(dt);
				else gameManager.car.ApplyFriction(dt);

				gameManager.Update(dt);
			}
			else if (currentState == GameState.MainMenu)
			{
				if (Array.Find(previousKeyboardState.GetPressedKeys(), OKPressed) == Keys.None && Array.Find(k.GetPressedKeys(), OKPressed) != Keys.None) startScreen.selectOpt(this);
			}
			else if (currentState == GameState.LevelSelect)
			{
				if (!previousKeyboardState.IsKeyDown(Keys.Down) && k.IsKeyDown(Keys.Down)) levelMenuScreen.KeyPressed(Keys.Down);
				else if (!previousKeyboardState.IsKeyDown(Keys.Up) && k.IsKeyDown(Keys.Up)) levelMenuScreen.KeyPressed(Keys.Up);
				else if (Array.Find(previousKeyboardState.GetPressedKeys(), OKPressed) == Keys.None && Array.Find(k.GetPressedKeys(), OKPressed) != Keys.None) levelMenuScreen.KeyPressed(Keys.Enter);
				else if (Array.Find(previousKeyboardState.GetPressedKeys(), NOKPressed) == Keys.None && Array.Find(k.GetPressedKeys(), NOKPressed) != Keys.None) levelMenuScreen.KeyPressed(Keys.Escape);
				else levelMenuScreen.KeyPressed(Keys.None);
			}

			previousKeyboardState = k;

			base.Update(gameTime);
		}

		private void exportProfile()
		{
			string path = Path.Combine("Saves", playerProfile.Id + ".xml");
			if (!File.Exists(path)) return;

			if (!Directory.Exists("Export"))
				Directory.CreateDirectory("Export");

			XslCompiledTransform xslt = new();
			xslt.Load(Path.Combine(AppContext.BaseDirectory, "Content", "export_profile.xslt"));

			xslt.Transform(path, Path.Combine("Export", "PlayerProfile.html"));
		}

		public void setState(GameState state)
		{
			if (state == GameState.Playing)
			{
				gameManager.LoadNextLevel();
			}
			else if (state == GameState.LevelSelect)
			{
				levelMenuScreen = new LevelMenuScreen(font, gameManager.levels.LevelEntries.ConvertAll(entry => entry.Name), gameManager.levels.LevelEntries.ConvertAll(entry => entry.Id), (levelId) =>
				{
					gameManager.LoadLevel(levelId);
					currentState = GameState.Playing;
				}, () =>
				{
					setState(GameState.MainMenu);
				})
				{
					bgTexture = bgTexture
				};
				screenManager = new ScreenManager(state, startScreen, levelMenuScreen);
			}
			else if (state == GameState.MainMenu)
			{
				screenManager = new ScreenManager(state, startScreen, levelMenuScreen);
			}
			currentState = state;
		}

		public static bool OKPressed(Keys key)
		{
			return key.Equals(Keys.Enter) == true ||
				   key.Equals(Keys.Space) == true ||
				   key.Equals(Keys.A) == true;
		}

		public static bool NOKPressed(Keys key)
		{
			return key.Equals(Keys.Back) == true ||
				   key.Equals(Keys.Delete) == true ||
				   key.Equals(Keys.Z) == true;
		}

		protected override void Draw(GameTime gameTime)
		{
			spriteBatch.Begin();
			if (currentState == GameState.Playing)
			{
				gameManager.Draw(GraphicsDevice, spriteBatch, font);
			}
			else if (currentState == GameState.MainMenu || currentState == GameState.LevelSelect)
			{
				screenManager.Draw(GraphicsDevice, spriteBatch);
			}
			spriteBatch.End();
			base.Draw(gameTime);
		}
	}
}