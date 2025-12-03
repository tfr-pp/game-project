using System;
using jeu.Core.Classes.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace jeu.Core.Classes.Controller;

public class GameManager
{
	public Level currentLevel { get; private set; } = null;

	private int currentLevelIndex = -1;

	public Track track { get; private set; }
	public Car car { get; private set; }
	private float levelTimer = 0f;

	private readonly EnemyManager enemyManager = new();

	public event Action<string, float, int> OnLevelCompleted;

	private Texture2D pixel;
	private Texture2D carTexture;
	private Texture2D bgLevelTexture;

	private Levels levels;

	public GameManager(Action<string, float, int> onLevelCompleted)
	{
		OnLevelCompleted = onLevelCompleted;
	}
	public void Load(GraphicsDevice graphicsDevice, Texture2D carTexture, Texture2D bgLevelTexture, Texture2D ennemySprite, Texture2D pixel)
	{
		enemyManager.LoadContent(graphicsDevice, ennemySprite);
		this.carTexture = carTexture;
		this.bgLevelTexture = bgLevelTexture;

		this.pixel = pixel;

		levels = Levels.LoadLevels();
	}

	public void LoadLevel(string levelId)
	{
		enemyManager.Clear();
		currentLevelIndex = levels.LevelEntries.FindIndex(entry => entry.Id == levelId);

		if (currentLevelIndex < 0)
		{
			currentLevelIndex = 0;
		}

		currentLevel = levels.GetLevel(currentLevelIndex);

		track = new Track(currentLevel.trackPoints.ConvertAll(p => p.ToVector2()));
		car = new Car(track);

		currentLevel.enemies.ConvertAll(e => e.ToEnemy()).ForEach(enemyManager.Add);
		levelTimer = 0f;
	}

	public void LoadNextLevel()
	{
		enemyManager.Clear();
		currentLevelIndex++;

		if (currentLevelIndex < 0)
		{
			currentLevelIndex = 0;
			currentLevel = levels.GetLevel(currentLevelIndex);
		}
		else if (currentLevelIndex == levels.LevelEntries.Count)
		{
			currentLevelIndex = 0;
			currentLevel = levels.GetLevel(currentLevelIndex);
		}
		else
		{
			currentLevel = levels.GetLevel(currentLevelIndex);
		}

		track = new Track(currentLevel.trackPoints.ConvertAll(p => p.ToVector2()));
		car = new Car(track);

		currentLevel.enemies.ConvertAll(e => e.ToEnemy()).ForEach(enemyManager.Add);
		levelTimer = 0f;
	}

	public void Update(float dt)
	{
		car.Update(dt);

		foreach (var enemy in enemyManager.GetEnemies())
		{
			if (car.hitBox.Intersects(enemy.hitBox))
			{
				car.HitEnemy(enemy.Speed);
			}
		}

		enemyManager.Update(dt);

		levelTimer += dt;

		// Fin si la cabine atteint 100% de progression
		if (car.positionAlongTrack / track.getTotalLength >= 1)
		{
			OnLevelCompleted.Invoke(currentLevel.id, levelTimer, car.lives);

			LoadNextLevel();
		}
	}

	public void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, SpriteFont font)
	{
		graphicsDevice.Clear(Color.CornflowerBlue);
		Rectangle bgRect = new(0, 0, graphicsDevice.PresentationParameters.BackBufferWidth,
			graphicsDevice.PresentationParameters.BackBufferHeight);
		spriteBatch.Draw(bgLevelTexture, new Vector2(0, 0), Color.White);

		DrawTrackLine(spriteBatch);

		spriteBatch.DrawString(
			font,
			$"Time: {levelTimer:0.00} s",
			new(10, 10),
			Color.Black
		);

		spriteBatch.DrawString(
			font,
			$"Lives: {car.lives}",
			new(10, 30),
			Color.Black
		);

		car.Draw(spriteBatch, carTexture);

		enemyManager.Draw(spriteBatch);
	}

	private void DrawTrackLine(SpriteBatch spriteBatch)
	{
		for (int i = 0; i < track.points.Count - 1; i++)
		{
			DrawLine(spriteBatch, track.points[i], track.points[i + 1], Color.Black, 2f);
		}
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