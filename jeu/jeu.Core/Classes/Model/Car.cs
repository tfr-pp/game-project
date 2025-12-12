using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace jeu.Core.Classes.Model;

/** A cable car the player controls 
 * Is created with a track
 */
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

	private readonly Vector2 _halfSize = new(20f, 30f);

	public Rectangle hitBox => GetAABB();

	/** Get the rotated hitbox of car
	 * \return A 4 Vector2D array corresponding to each angle
	 */
	public Vector2[] GetRotatedHitBoxCorners()
	{
		float cos = MathF.Cos(rotation);
		float sin = MathF.Sin(rotation);

		Vector2[] local =
		[
			new(-_halfSize.X, -_halfSize.Y),
			new( _halfSize.X, -_halfSize.Y),
			new( _halfSize.X,  _halfSize.Y),
			new(-_halfSize.X,  _halfSize.Y),
		];

		Vector2[] corners = new Vector2[4];
		for (int i = 0; i < 4; i++)
		{
			float x = local[i].X;
			float y = local[i].Y;
			corners[i] = new Vector2(
				x * cos - y * sin + position.X,
				x * sin + y * cos + position.Y
			);
		}
		return corners;
	}

	/** Get the hitbox of car
	 * \return A 4 Vector2D array corresponding to each angle
	 */
	public Rectangle GetAABB()
	{
		Vector2[] c = GetRotatedHitBoxCorners();
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

	/** Restraint the car position between the beginning and the end of the track
	 * 
	 */
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

		Vector2 tangent = track.GetTangentAtDistance(positionAlongTrack);
		rotation = MathF.Atan2(tangent.Y, tangent.X);

		// Dead zone for very low speeds
		if (speed > -0.5f && speed < 0.5f) speed = 0f;
	}

	/** Augment car speed of ACCELERATION constant
	 * Bound by MAX_SPEED and its inverse
	 * \param dt a float: the expected acceleration
	 */
	public void Accelerate(float dt)
	{
		speed = Math.Clamp(speed + (ACCELERATION * dt), -MAX_SPEED, MAX_SPEED);
	}
	/** Diminish car speed of ACCELERATION constant
	 * Bound by MAX_SPEED and its inverse
	 * \param dt a float: the expected diminish
	 */
	public void Decelerate(float dt)
	{
		speed = Math.Clamp(speed - (ACCELERATION * dt), -MAX_SPEED, MAX_SPEED);
	}

	/** Apply track friction to the car
	 * \param dt a float: the expected friction
	 */
	public void ApplyFriction(float dt)
	{
		if (speed > 0) speed = Math.Max(0, speed - FRICTION * dt);
		else if (speed < 0) speed = Math.Min(0, speed + FRICTION * dt);
	}

	/** Apply the effect of hitting an enemy
	 * Effects: lives-1, speed diminish, pk??
	 */
	public void HitEnemy(float enemySpeed)
	{
		lives = Math.Max(0, lives - 1);

		float relative = enemySpeed - speed;

		float impulse = MathF.Abs(relative) * 0.8f + 100f;

		float direction = relative != 0f ? MathF.Sign(relative) : (speed > 0f ? -1f : 1f);

		speed = Math.Clamp(speed + direction * impulse, -MAX_SPEED, MAX_SPEED);

		positionAlongTrack += speed * 0.16f;

		ClampPositionAlongTrack();
	}

	public void Draw(SpriteBatch spriteBatch, Texture2D carTexture)
	{
		if (carTexture == null) return;

		Vector2 origin = new(carTexture.Width / 2f, 0);

		float scaleX = _halfSize.X * 2f / carTexture.Width;
		float scaleY = _halfSize.Y * 2f / carTexture.Height;
		Vector2 scale = new(scaleX, scaleY);

		spriteBatch.Draw(carTexture,
			position: position,
			sourceRectangle: null,
			color: Color.White,
			rotation: rotation,
			origin: origin,
			scale: scale,
			effects: SpriteEffects.None,
			layerDepth: 0f);
	}

	/** Apply the gravity to the car
	 * \param dt a float: the acceleration consireding gravity
	 */
	public void ApplyGravity(float dt)
	{
		Vector2 tangent = track.GetTangentAtDistance(positionAlongTrack);
		Vector2 gravityDir = new(0f, 1f);
		float accelAlongTangent = Vector2.Dot(gravityDir, tangent) * GRAVITY_ACCEL;
		speed = Math.Clamp(speed + accelAlongTangent * dt, -MAX_SPEED, MAX_SPEED);
	}
}
