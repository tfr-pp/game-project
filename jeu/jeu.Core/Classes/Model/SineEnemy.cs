using System;
using Microsoft.Xna.Framework;

namespace jeu.Core.Classes.Model;

public class CircleEnemy(Vector2 startPosition,
					 float speed, float amplitude) : Enemy(startPosition, speed)
{
	private readonly float radius = amplitude;
	private float _angle;
	private readonly float _baseX = startPosition.X;
	private readonly float _baseY = startPosition.Y;

	public override void Update(float dt)
	{
		_angle += Speed * dt / radius;

		Position = new Vector2(
			_baseX + (float)Math.Cos(_angle) * radius,
			_baseY + (float)Math.Sin(_angle) * radius
		);
	}
}
