using System;
using jeu.Core.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace jeu.Core
{

	public class JeuGame : Game
	{
		// Do not remove this field even if it seems unused
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;

		public Track Track { get; private set; }
		public Car Car { get; private set; }
		public int Passengers { get; private set; } = 5;

		private readonly EnemyManager enemyManager = new();

		Texture2D pixel;

		SpriteFont font;

		public JeuGame()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
			Track = new Track(
			[
				new(100, 100),
				new(50, 450),
				new(200, 150),
				new(700, 100),
				new(700, 200),
				new(350, 200),
				new(300, 300),
				new(700, 450)
			], true);
			Car = new Car(Track);
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			pixel = new Texture2D(GraphicsDevice, 1, 1);
			pixel.SetData([Color.White]);

			font = Content.Load<SpriteFont>("Default");

			enemyManager.LoadContent(GraphicsDevice);
			enemyManager.Add(new HorizontalPatrolEnemy(16f, new Vector2(400, 150), new Vector2(400, 250)));
		}

		protected override void Update(GameTime gameTime)
		{
			var k = Keyboard.GetState();

			if (k.IsKeyDown(Keys.Escape)) Exit();

			float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

			if (k.IsKeyDown(Keys.Up)) Car.Accelerate(dt);
			else if (k.IsKeyDown(Keys.Down)) Car.Decelerate(dt);
			else Car.ApplyFriction(dt);

			Car.Update(dt);

			foreach (var enemy in enemyManager.GetEnemies())
			{
				if (Car.Hitbox.Intersects(enemy.Hitbox))
				{
					Car.HitEnemy(enemy.Speed);
				}
			}

			enemyManager.Update(gameTime);

			base.Update(gameTime);
		}

		private void DrawTrackLine()
		{
			for (int i = 0; i < Track.Points.Count - 1; i++)
			{
				DrawLine(_spriteBatch, Track.Points[i], Track.Points[i + 1], Color.Gray, 2f);
			}
		}


		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);


			_spriteBatch.Begin();

			DrawTrackLine();

			_spriteBatch.DrawString(
				font,
				$"Speed: {Car.Speed:0.00} px/s",
				new(10, 10),
				Color.Black
			);

			_spriteBatch.DrawString(
				font,
				$"Completion: {Car.PositionAlongTrack / Track.TotalLength * 100:0.00}%",
				new(10, 30),
				Color.Black
			);

			_spriteBatch.DrawString(
				font,
				$"Passengers: {Car.Passengers}",
				new(10, 50),
				Color.Black
			);

			Car.Draw(_spriteBatch, pixel);

			enemyManager.Draw(_spriteBatch);

			_spriteBatch.End();

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