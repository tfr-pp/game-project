using System.Collections.Generic;
using System.Numerics;

namespace jeu.Core.Classes;

public class Track
{
	public List<Vector2> Points { get; private set; }
	private List<float> _segmentLengths;
	private float _totalLength;

	public Track(List<Vector2> points, bool clean = true)
	{
		if (clean && points.Count >= 4)
		{
			Points = [];

			var keyPoints = new List<Vector2>
			{
				points[0]
			};
			keyPoints.AddRange(points);
			keyPoints.Add(points[^1]);

			for (int i = 0; i < keyPoints.Count - 3; i++)
			{
				for (float t = 0; t < 1; t += 0.01f)
				{
					Points.Add(CatmullRom(keyPoints[i], keyPoints[i + 1], keyPoints[i + 2], keyPoints[i + 3], t));
				}
			}

			Points.Add(keyPoints[keyPoints.Count - 2]);
		}
		else
		{
			Points = points;
		}
		Precompute();
	}

	private void Precompute()
	{
		_segmentLengths = [];
		_totalLength = 0f;
		for (int i = 0; i < Points.Count - 1; i++)
		{
			float len = Vector2.Distance(Points[i], Points[i + 1]);
			_segmentLengths.Add(len);
			_totalLength += len;
		}
	}

	public float TotalLength => _totalLength;

	public Vector2 GetPositionAtDistance(float s)
	{
		if (s <= 0f) return Points[0];
		if (s >= _totalLength) return Points[^1];

		float remaining = s;
		for (int i = 0; i < _segmentLengths.Count; i++)
		{
			if (remaining <= _segmentLengths[i])
				return Vector2.Lerp(Points[i], Points[i + 1], remaining / _segmentLengths[i]);
			remaining -= _segmentLengths[i];
		}
		return Points[^1];
	}

	public Vector2 GetTangentAtDistance(float s)
	{
		if (s <= 0f) return Vector2.Normalize(Points[1] - Points[0]);
		if (s >= _totalLength) return Vector2.Normalize(Points[^1] - Points[^2]);

		float remaining = s;
		for (int i = 0; i < _segmentLengths.Count; i++)
		{
			if (remaining <= _segmentLengths[i])
				return Vector2.Normalize(Points[i + 1] - Points[i]);
			remaining -= _segmentLengths[i];
		}
		return Vector2.UnitX;
	}

	public static Vector2 CatmullRom(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
	{
		return 0.5f * (
			(2f * p1) +
			(-p0 + p2) * t +
			(2f * p0 - 5f * p1 + 4f * p2 - p3) * (t * t) +
			(-p0 + 3f * p1 - 3f * p2 + p3) * (t * t * t)
		);
	}

}
