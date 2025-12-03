using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace jeu.Core.Classes;

public class Car(Track track)
{
	private static readonly float MAX_SPEED = 400f;
	private static readonly float ACCELERATION = 500f;
	private static readonly float FRICTION = 60f;
	private static readonly float GRAVITY_ACCEL = 150f;

	private readonly Track track = track;
	public float positionAlongTrack { get; private set; } = 0f;
	public float speed { get; private set; } = 0f;
	public Vector2 position => track.GetPositionAtDistance(positionAlongTrack);
	public float rotation { get; private set; }
	public int lives { get; private set; } = 5;

	private readonly Vector2 _halfSize = new(25f, 12f);

	public Rectangle hitBox => GetAABB();

	public Vector2[] GetRotatedHitboxCorners()
	{
		var cos = MathF.Cos(rotation);
		var sin = MathF.Sin(rotation);

		var local = new Vector2[]
		{
			new(-_halfSize.X, -_halfSize.Y),
			new( _halfSize.X, -_halfSize.Y),
			new( _halfSize.X,  _halfSize.Y),
			new(-_halfSize.X,  _halfSize.Y),
		};

		var corners = new Vector2[4];
		for (int i = 0; i < 4; i++)
		{
			var x = local[i].X;
			var y = local[i].Y;
			corners[i] = new Vector2(
				x * cos - y * sin + position.X,
				x * sin + y * cos + position.Y
			);
		}
		return corners;
	}

	public Rectangle GetAABB()
	{
		var c = GetRotatedHitboxCorners();
		float minX = c[0].X, minY = c[0].Y, maxX = c[0].X, maxY = c[0].Y;
		for (int i = 1; i < c.Length; i++)
		{
			minX = MathF.Min(minX, c[i].X);
			minY = MathF.Min(minY, c[i].Y);
			maxX = MathF.Max(maxX, c[i].X);
			maxY = MathF.Max(maxY, c[i].Y);
		}
		return new Rectangle((int)minX, (int)minY, (int)(maxX - minX), (int)(maxY - minY));
	}

	private void ClampPositionAlongTrack()
	{
		if (positionAlongTrack < 0f)
		{
			speed = 0f;
			positionAlongTrack = 0f;
		}
		else if (positionAlongTrack > track.getTotalLength)
		{
			speed = 0f;
			positionAlongTrack = track.getTotalLength;
		}
	}

	public void Update(float dt)
	{
		ApplyGravity(dt);

		positionAlongTrack += speed * dt;

		ClampPositionAlongTrack();

		var tangent = track.GetTangentAtDistance(positionAlongTrack);
		rotation = MathF.Atan2(tangent.Y, tangent.X);

		// Dead zone for very low speeds
		if (speed > -0.5f && speed < 0.5f) speed = 0f;
	}

	public void Accelerate(float dt)
	{
		speed = Math.Clamp(speed + (ACCELERATION * dt), -MAX_SPEED, MAX_SPEED);
	}

	public void Decelerate(float dt)
	{
		speed = Math.Clamp(speed - (ACCELERATION * dt), -MAX_SPEED, MAX_SPEED);
	}

	public void ApplyFriction(float dt)
	{
		if (speed > 0) speed = Math.Max(0, speed - FRICTION * dt);
		else if (speed < 0) speed = Math.Min(0, speed + FRICTION * dt);
	}

	public void HitEnemy(float enemySpeed)
	{
		lives = Math.Max(0, lives - 1);

		var relative = enemySpeed - speed;

		var impulse = MathF.Abs(relative) * 0.8f + 100f;

		var direction = relative != 0f ? MathF.Sign(relative) : (speed > 0f ? -1f : 1f);

		speed = Math.Clamp(speed + direction * impulse, -MAX_SPEED, MAX_SPEED);

		positionAlongTrack += speed * 0.16f;

		ClampPositionAlongTrack();
	}

	public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
	{
		spriteBatch.Draw(pixel,
			position: position,
			sourceRectangle: null,
			color: Color.Green,
			rotation: rotation,
			origin: new Vector2(0.5f, 0.5f),
			scale: new Vector2(40, 40),
			effects: SpriteEffects.None,
			layerDepth: 0f);
	}

	public void ApplyGravity(float dt)
	{
		var tangent = track.GetTangentAtDistance(positionAlongTrack);
		var gravityDir = new Vector2(0f, 1f);
		var accelAlongTangent = Vector2.Dot(gravityDir, tangent) * GRAVITY_ACCEL;
		speed = Math.Clamp(speed + accelAlongTangent * dt, -MAX_SPEED, MAX_SPEED);
	}
}
