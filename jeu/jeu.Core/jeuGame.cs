using System;
using System.Collections.Generic;
using jeu.Core.Classes.Controller;
using jeu.Core.Classes.Model;
using jeu.Core.Classes.Vue;
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
		private Texture2D pixel;

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
			levelMenuScreen = new LevelMenuScreen(font, [], null, null);
			screenManager = new ScreenManager(currentState, startScreen, levelMenuScreen);
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
			pixel = new Texture2D(GraphicsDevice, 1, 1);
			pixel.SetData([Color.White]);

			gameManager.Load(carTexture, bgLevelTexture, enemySprite, pixel);
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
			var k = Keyboard.GetState();

			if (k.IsKeyDown(Keys.Escape)) Exit();


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
				if (Array.Find(k.GetPressedKeys(), OKPressed) != Keys.None) startScreen.selectOpt(this);
			}
			else if (currentState == GameState.LevelSelect)
			{
				if (k.IsKeyDown(Keys.Down)) levelMenuScreen.KeyPressed(Keys.Down);
				if (k.IsKeyDown(Keys.Up)) levelMenuScreen.KeyPressed(Keys.Up);
				if (Array.Find(k.GetPressedKeys(), OKPressed) != Keys.None) levelMenuScreen.KeyPressed(Keys.Enter);
				if (Array.Find(k.GetPressedKeys(), NOKPressed) != Keys.None) levelMenuScreen.KeyPressed(Keys.Escape);
			}


			base.Update(gameTime);
		}

		public void setState(GameState state)
		{
			if (state == GameState.Playing)
			{
				gameManager.LoadNextLevel();
			}
			else if (state == GameState.LevelSelect)
			{
				var levelNames = gameManager.levels.LevelEntries.ConvertAll(entry => entry.Name);
				levelMenuScreen = new LevelMenuScreen(font, levelNames, (levelName) =>
				{
					gameManager.LoadLevel(gameManager.levels.LevelEntries.Find(entry => entry.Name == levelName).Id);
					setState(GameState.Playing);
				}, () =>
				{
					setState(GameState.MainMenu);
				})
				{
					bgTexture = bgTexture
				};
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