using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace jeu.Core.Classes;

public class EnemyManager
{
	private readonly List<Enemy> _enemies = [];

	protected Texture2D pixel;

	public void Add(Enemy enemy) => _enemies.Add(enemy);

	public void LoadContent(GraphicsDevice graphicsDevice)
	{
		pixel = new Texture2D(graphicsDevice, 1, 1);
		pixel.SetData([Color.White]);
	}

	public void Update(float dt)
	{
		foreach (var enemy in _enemies)
			enemy.Update(dt);
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		foreach (var enemy in _enemies)
			enemy.Draw(spriteBatch, pixel);
	}

	public void Clear() => _enemies.Clear();

	public IEnumerable<Enemy> GetEnemies() => _enemies;
}
