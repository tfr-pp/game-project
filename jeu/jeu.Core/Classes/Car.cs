using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace jeu.Core.Classes;

public class Car(Track track)
{
	private static readonly float MAX_SPEED = 800f;
	private static readonly float ACCELERATION = 400f;
	private static readonly float FRICTION = 200f;

	private readonly Track _track = track;
	public float PositionAlongTrack { get; private set; } = 0f;
	public float Speed { get; private set; } = 0f;
	public Vector2 Position => _track.GetPositionAtDistance(PositionAlongTrack);
	public float Rotation { get; private set; }
	public int Passengers { get; private set; } = 5;

	private readonly Vector2 _halfSize = new(25f, 12f);

	public Rectangle Hitbox => GetAABB();

	public Vector2[] GetRotatedHitboxCorners()
	{
		var cos = MathF.Cos(Rotation);
		var sin = MathF.Sin(Rotation);

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
				x * cos - y * sin + Position.X,
				x * sin + y * cos + Position.Y
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
		if (PositionAlongTrack < 0f)
		{
			Speed = 0f;
			PositionAlongTrack = 0f;
		}
		else if (PositionAlongTrack > _track.TotalLength)
		{
			Speed = 0f;
			PositionAlongTrack = _track.TotalLength;
		}
	}

	public void Update(float dt)
	{
		PositionAlongTrack += Speed * dt;

		ClampPositionAlongTrack();

		var tangent = _track.GetTangentAtDistance(PositionAlongTrack);
		Rotation = MathF.Atan2(tangent.Y, tangent.X);
	}

	public void Accelerate(float dt)
	{
		Speed = Math.Clamp(Speed + (ACCELERATION * dt), -MAX_SPEED, MAX_SPEED);
	}

	public void Decelerate(float dt)
	{
		Speed = Math.Clamp(Speed - (ACCELERATION * dt), -MAX_SPEED, MAX_SPEED);
	}

	public void ApplyFriction(float dt)
	{
		if (Speed > 0) Speed = Math.Max(0, Speed - FRICTION * dt);
		else if (Speed < 0) Speed = Math.Min(0, Speed + FRICTION * dt);
	}

	public void HitEnemy(float enemySpeed)
	{
		Passengers = Math.Max(0, Passengers - 1);

		var relative = enemySpeed - Speed;

		var impulse = MathF.Abs(relative) * 0.8f + 100f;

		var direction = relative != 0f ? MathF.Sign(relative) : (Speed > 0f ? -1f : 1f);

		Speed = Math.Clamp(Speed + direction * impulse, -MAX_SPEED, MAX_SPEED);

		PositionAlongTrack += Speed * 0.16f;

		ClampPositionAlongTrack();
	}

	public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
	{
		spriteBatch.Draw(pixel,
			position: Position,
			sourceRectangle: null,
			color: Color.Green,
			rotation: Rotation,
			origin: new Vector2(0.5f, 0.5f),
			scale: new Vector2(40, 40),
			effects: SpriteEffects.None,
			layerDepth: 0f);
	}
}
