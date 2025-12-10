using System.Collections.Generic;
using jeu.Core.Classes.Model;
using Microsoft.Xna.Framework.Graphics;

namespace jeu.Core.Classes.Controller;

public class EnemyManager
{
	private readonly List<Enemy> _enemies = [];

	protected Texture2D enemySprite;

	public void Add(Enemy enemy) => _enemies.Add(enemy);

	public void LoadContent(Texture2D enemySprite)
	{
		this.enemySprite = enemySprite;
	}

	public void Update(float dt)
	{
		foreach (var enemy in _enemies)
			enemy.Update(dt);
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		foreach (var enemy in _enemies)
			enemy.Draw(spriteBatch, enemySprite);
	}

	public void Clear() => _enemies.Clear();

	public List<Enemy> GetEnemies() => _enemies;
}
