using Microsoft.Xna.Framework;

namespace jeu.Core.Classes;

public class HorizontalPatrolEnemy(float speed, Vector2 from, Vector2 to) : Enemy(from, speed)
{
	private readonly Vector2 _from = from;
	private readonly Vector2 _to = to;
	private Vector2 _target = to;
	private const float EPSILON = 0.001f;

	public override void Update(float dt)
	{
		if (_from == _to) return;

		float step = Speed * dt;

		Vector2 toTarget = _target - Position;
		float dist = toTarget.Length();

		if (dist <= step + EPSILON)
		{
			Position = _target;
			_target = (_target == _to) ? _from : _to;
			return;
		}

		if (dist > EPSILON)
		{
			Position += toTarget / dist * step;
		}
	}
}
