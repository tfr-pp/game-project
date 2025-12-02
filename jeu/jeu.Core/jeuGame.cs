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
		private GameState currentState = GameState.MainMenu;

		private List<PlayerProfile> playerProfiles = [];
		private string inputPseudo;
		private int currentLevel = 1;

		public Track track { get; private set; }
		public Car car { get; private set; }
		private float levelTimer = 0f;

		private readonly EnemyManager enemyManager = new();

		Texture2D pixel;
		SpriteFont font;

		public JeuGame()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			base.Initialize();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			enemyManager.LoadContent(GraphicsDevice);
			font = Content.Load<SpriteFont>("Default");

			pixel = new Texture2D(GraphicsDevice, 1, 1);
			pixel.SetData([Color.White]);

			saveManager = new SaveManager();

			playerProfiles = saveManager.LoadAllProfiles();
			playerProfile = playerProfiles.Count > 0 ? playerProfiles[0] : new PlayerProfile
			{
				Id = Guid.NewGuid().ToString(),
				Name = "Player1",
				CreationDate = DateTime.Now
			};

			LoadLevel(1);
		}

		protected void LoadLevel(int levelId)
		{
			enemyManager.Clear();
			currentLevel = levelId;

			Level level = Level.LoadLevel($"level{levelId}.xml");
			track = new Track(level.trackPoints.ConvertAll(p => p.ToVector2()));
			car = new Car(track);

			level.enemies.ConvertAll(e => e.ToEnemy()).ForEach(enemyManager.Add);

			levelTimer = 0f;
		}

		protected override void Update(GameTime gameTime)
		{
			var k = Keyboard.GetState();

			if (k.IsKeyDown(Keys.Escape)) Exit();

			float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

			if (k.IsKeyDown(Keys.Up)) car.Accelerate(dt);
			else if (k.IsKeyDown(Keys.Down)) car.Decelerate(dt);
			else car.ApplyFriction(dt);

			car.Update(dt);

			foreach (var enemy in enemyManager.GetEnemies())
			{
				if (car.hitBox.Intersects(enemy.hitBox))
				{
					car.HitEnemy(enemy.Speed);
				}
			}

			enemyManager.Update(gameTime);

			levelTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

			// Fin si la cabine atteint 100% de progression
			if (car.positionAlongTrack / track.getTotalLength >= 1)
			{
				saveManager.CompleteLevel(playerProfile, currentLevel, levelTimer, car.passengers);
				LoadLevel(currentLevel + 1);
			}

			base.Update(gameTime);
		}

		private void DrawTrackLine()
		{
			for (int i = 0; i < track.points.Count - 1; i++)
			{
				DrawLine(spriteBatch, track.points[i], track.points[i + 1], Color.Gray, 2f);
			}
		}


		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);


			spriteBatch.Begin();

			DrawTrackLine();

			spriteBatch.DrawString(
				font,
				$"Time: {levelTimer:0.00} s",
				new(10, 10),
				Color.Black
			);

			spriteBatch.DrawString(
				font,
				$"Passengers: {car.passengers}",
				new(10, 30),
				Color.Black
			);

			car.Draw(spriteBatch, pixel);

			enemyManager.Draw(spriteBatch);

			spriteBatch.End();

			base.Draw(gameTime);
		}

		private void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end, Color color, float thickness)
		{
			Vector2 edge = end - start;
			float angle = (float)Math.Atan2(edge.Y, edge.X);

			sb.Draw(
				pixel,
				new Rectangle(
					(int)start.X - 1,
					(int)start.Y - 1,
					(int)edge.Length() + 2,
					(int)thickness
				),
				null,
				color,
				angle,
				Vector2.Zero,
				SpriteEffects.None,
				0f
			);
		}
	}
}