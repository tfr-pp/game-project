using System.Collections.Generic;
using jeu.Core.Classes.Model;
using Microsoft.Xna.Framework.Graphics;

namespace jeu.Core.Classes.Controller;

public class EnemyManager
{
	private readonly List<Enemy> _enemies = [];

	protected Texture2D ennemySprite;

	public void Add(Enemy enemy) => _enemies.Add(enemy);

	public void LoadContent(GraphicsDevice graphicsDevice, Texture2D ennemySprite)
	{
		this.ennemySprite = ennemySprite;
	}

	public void Update(float dt)
	{
		foreach (var enemy in _enemies)
			enemy.Update(dt);
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		foreach (var enemy in _enemies)
			enemy.Draw(spriteBatch, ennemySprite);
	}

	public void Clear() => _enemies.Clear();

	public IEnumerable<Enemy> GetEnemies() => _enemies;
}
