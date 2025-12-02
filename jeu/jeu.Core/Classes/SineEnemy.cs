using System;
using Microsoft.Xna.Framework;

namespace jeu.Core.Classes;

public class SineEnemy(Vector2 startPosition,
					 float speed, float amplitude, float frequency) : Enemy(startPosition, speed)
{
	private readonly float amplitude = amplitude;
	private readonly float frequency = frequency;
	private float _time;

	public override void Update(float dt)
	{
		_time += dt;

		Position = new Vector2(
			Position.X - Speed,
			Position.Y + (float)Math.Sin(_time * frequency) * amplitude
		);
	}
}
