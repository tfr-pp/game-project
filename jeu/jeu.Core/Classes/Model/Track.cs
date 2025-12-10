using System.Collections.Generic;
using System.Numerics;

namespace jeu.Core.Classes.Model;

public class Track
{
	public List<Vector2> points { get; private set; }
	private List<float> segmentLengths;
	private float totalLength;
	public float getTotalLength => totalLength;

	public Track(List<Vector2> init_points)
	// points where track rail goes and "clean" the curving or not of path
	{
		if (init_points.Count >= 4)
		{
			points = [];

			var keyPoints = new List<Vector2>
			{
				init_points[0]
			};
			keyPoints.AddRange(init_points);
			keyPoints.Add(init_points[^1]);

			for (int i = 0; i < keyPoints.Count - 3; i++)
			{
				for (float t = 0; t < 1; t += 0.01f)
				{
					points.Add(CatmullRom(keyPoints[i], keyPoints[i + 1], keyPoints[i + 2], keyPoints[i + 3], t));
				}
			}

			points.Add(keyPoints[^2]);
		}
		else
		{
			points = init_points;
		}

		segmentLengths = [];
		totalLength = 0f;

		for (int i = 0; i < points.Count - 1; i++)
		{
			float segmentLength = Vector2.Distance(points[i], points[i + 1]);
			segmentLengths.Add(segmentLength);
			totalLength += segmentLength;
		}
	}

	public Vector2 GetPositionAtDistance(float s)
	{
		if (s <= 0f) return points[0];
		if (s >= totalLength) return points[^1];

		float remaining = s;
		for (int i = 0; i < segmentLengths.Count; i++)
		{
			if (remaining <= segmentLengths[i])
				return Vector2.Lerp(points[i], points[i + 1], remaining / segmentLengths[i]);
			remaining -= segmentLengths[i];
		}
		return points[^1];
	}

	public Vector2 GetTangentAtDistance(float s)
	{
		if (s <= 0f) return Vector2.Normalize(points[1] - points[0]);
		if (s >= totalLength) return Vector2.Normalize(points[^1] - points[^2]);

		float remaining = s;
		for (int i = 0; i < segmentLengths.Count; i++)
		{
			if (remaining <= segmentLengths[i])
				return Vector2.Normalize(points[i + 1] - points[i]);
			remaining -= segmentLengths[i];
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
