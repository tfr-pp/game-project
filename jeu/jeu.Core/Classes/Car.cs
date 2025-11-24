using System;
using System.Numerics;

namespace jeu.Core.Classes;

public class Car(Track track)
{
	private static readonly float MAX_SPEED = 800f;
	private static readonly float ACCELERATION = 400f;
	private static readonly float FRICTION = 200f;

	private readonly Track _track = track;
	public float positionAlongTrack { get; private set; } = 0f;
	public float speed { get; private set; } = 0f;
	public Vector2 position => _track.GetPositionAtDistance(positionAlongTrack);
	public float rotation { get; private set; }

	public void update(float dt)
	{
		positionAlongTrack += speed * dt;

		if (positionAlongTrack <= 0f)
		{
			speed = 0f;
			positionAlongTrack = 0f;
		}
		else if (positionAlongTrack >= _track.getTotalLength)
		{
			speed = 0f;
			positionAlongTrack = _track.getTotalLength;
		}

		var tangent = _track.GetTangentAtDistance(positionAlongTrack);
		rotation = MathF.Atan2(tangent.Y, tangent.X);
	}

	public void accelerate(float dt)
	{
		speed = Math.Clamp(speed + (ACCELERATION * dt), -MAX_SPEED, MAX_SPEED);
	}

	public void decelerate(float dt)
	{
		speed = Math.Clamp(speed - (ACCELERATION * dt), -MAX_SPEED, MAX_SPEED);
	}

	public void applyFriction(float dt)
	{
		if (speed > 0) speed = Math.Max(0, speed - FRICTION * dt);
		else if (speed < 0) speed = Math.Min(0, speed + FRICTION * dt);
	}
}
