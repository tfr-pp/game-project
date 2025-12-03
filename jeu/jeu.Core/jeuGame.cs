using System;
using System.Collections.Generic;
using jeu.Core.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace jeu.Core
{
	// Pour gérer sur quel écran on est
	public enum GameState
	{
		MainMenu,
		LevelSelect,
		Playing,
		LevelCompleted,
		HighScores,
		Options
	}

	public class JeuGame : Game
	{
		// Do not remove this field even if it seems unused
		private GraphicsDeviceManager graphics;
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
		private Texture2D ennemySprite;

		private StartScreen startScreen;
		private LevelMenuScreen levelMenuScreen;
		/*
			private HighScoresScreen highScoresScreen;
			private OptionsMenuScreen optionsMenuScreen;
		*/
		private ScreenManager screenManager;

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
			levelMenuScreen = new LevelMenuScreen();
			/*
						highScoresScreen = new HighScoresScreen();
						optionsMenuScreen = new OptionsMenuScreen();
			*/
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
			ennemySprite = Content.Load<Texture2D>("Sprites/Ennemy");

			gameManager.Load(GraphicsDevice, carTexture, bgLevelTexture, ennemySprite);
			startScreen.LoadContent(bgTexture);
			levelMenuScreen.LoadContent(bgLevelTexture);

			saveManager = new SaveManager();

			playerProfiles = saveManager.LoadAllProfiles();
			playerProfile = playerProfiles.Count > 0 ? playerProfiles[0] : new PlayerProfile
			{
				Id = Guid.NewGuid().ToString(),
				Name = "Player1",
				CreationDate = DateTime.Now
			};

			//gameManager.LoadNextLevel();
			//currentState = GameState.Playing;
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
			else if (gameManager.menuManager != null && gameManager.menuManager.IsActive)
			{
				foreach (var key in k.GetPressedKeys())
				{
					gameManager.menuManager.activeMenu.KeyPressed(key);
				}
			}

			if (currentState == GameState.MainMenu)
			{
				//+ gerer à la souris si possible
				if (k.IsKeyDown(Keys.Down)) startScreen.menuDown();
				if (k.IsKeyDown(Keys.Up)) startScreen.menuUp();
				if (Array.Find(k.GetPressedKeys(), OKPressed) != Keys.None) startScreen.selectOpt(this);
			}


			base.Update(gameTime);
		}

		public void setState(GameState state)
		{
			currentState = state;
		}

		public static bool OKPressed(Keys key) //voir controlleur souris/clavier
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
			gameManager.Draw(GraphicsDevice, spriteBatch, font);
			if (currentState == GameState.Playing)
			{

			}

			if (currentState == GameState.MainMenu)
			{
				screenManager.Draw(GraphicsDevice, spriteBatch);
			}
			base.Draw(gameTime);
		}
	}
}