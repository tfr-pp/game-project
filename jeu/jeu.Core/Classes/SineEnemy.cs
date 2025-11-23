

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace jeu.Core.Classes;

public class SineEnemy(Vector2 startPosition,
					 float speed, float amplitude, float frequency) : Enemy(startPosition, speed)
{
	private readonly float _amplitude = amplitude;
	private readonly float _frequency = frequency;
	private float _time;

	public override void Update(GameTime gameTime)
	{
		_time += (float)gameTime.ElapsedGameTime.TotalSeconds;

		Position = new Vector2(
			Position.X - Speed,
			Position.Y + (float)Math.Sin(_time * _frequency) * _amplitude
		);
	}
}
