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

			base.Initialize();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			font = Content.Load<SpriteFont>("Default");
			carTexture = Content.Load<Texture2D>("Sprites/Car");

			gameManager.Load(GraphicsDevice, carTexture);

			saveManager = new SaveManager();

			playerProfiles = saveManager.LoadAllProfiles();
			playerProfile = playerProfiles.Count > 0 ? playerProfiles[0] : new PlayerProfile
			{
				Id = Guid.NewGuid().ToString(),
				Name = "Player1",
				CreationDate = DateTime.Now
			};

			gameManager.LoadNextLevel();
			currentState = GameState.Playing;
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


			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			gameManager.Draw(GraphicsDevice, spriteBatch, font);

			base.Draw(gameTime);
		}
	}
}