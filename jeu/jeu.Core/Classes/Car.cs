using System;
using System.Numerics;

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

	public void Update(float dt)
	{
		PositionAlongTrack += Speed * dt;

		if (PositionAlongTrack <= 0f)
		{
			Speed = 0f;
			PositionAlongTrack = 0f;
		}
		else if (PositionAlongTrack >= _track.TotalLength)
		{
			Speed = 0f;
			PositionAlongTrack = _track.TotalLength;
		}

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
}
