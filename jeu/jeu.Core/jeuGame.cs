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
		
		private List<PlayerProfile> playerProfiles = new List<PlayerProfile>();
		private string inputPseudo;
		private int selectedLevel = 1;
		private int currentLevel = 1;
		
		public Track track { get; private set; }
		public Car car { get; private set; }
		private float levelTimer = 0f;
		private int passengers = 5;

		private readonly EnemyManager enemyManager = new();

		Texture2D pixel;
		SpriteFont font;

		private XmlValidator xmlValidator;
		private XsltTransformer xsltTransformer;
		
		public JeuGame()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true; 
			
			saveManager = new SaveManager();
			playerProfiles = saveManager.LoadAllProfiles();
			
			xmlValidator = new XmlValidator();
			xsltTransformer = new XsltTransformer();
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

			Level level = Level.LoadLevel("level1.xml");
			track = new Track(level.trackPoints.ConvertAll(p => p.ToVector2()));
			car = new Car(track);

			level.enemies.ConvertAll(e => e.ToEnemy()).ForEach(enemyManager.Add);
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
				if (car.hitbox.Intersects(enemy.Hitbox))
				{
					car.HitEnemy(enemy.Speed);
				}
			}

			enemyManager.Update(gameTime);
			
			levelTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
			
			// Fin si la cabine atteint 100% de progression
			float progression = car.positionAlongTrack / track.getTotalLength * 100;
			if (progression >= 100)
			{
				saveManager.CompleteLevel(playerProfile, currentLevel, levelTimer, passengers);
				currentLevel++;
				levelTimer = 0f;
				passengers = 5;
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
				$"Speed: {car.speed:0.00} px/s",
				new(10, 10),
				Color.Black
			);

			spriteBatch.DrawString(
				font,
				$"Completion: {car.positionAlongTrack / track.getTotalLength * 100:0.00}%",
				new(10, 30),
				Color.Black
			);

			spriteBatch.DrawString(
				font,
				$"Passengers: {car.passengers}",
				new(10, 50),
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