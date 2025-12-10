using System;
using System.Xml.Serialization;

namespace jeu.Core.Classes.Model;

public abstract class EnemyData
{
	[XmlAttribute] public float Speed { get; set; }
	[XmlAttribute] public float StartX { get; set; }
	[XmlAttribute] public float StartY { get; set; }
	public virtual Enemy ToEnemy()
	{
		throw new NotImplementedException("ToEnemy must be implemented in derived classes");
	}
}

public class HorizontalPatrolEnemyData : EnemyData
{
	[XmlAttribute] public float EndX { get; set; }
	[XmlAttribute] public float EndY { get; set; }
	public override HorizontalPatrolEnemy ToEnemy()
	{
		return new HorizontalPatrolEnemy(
			Speed,
			new(StartX, StartY),
			new(EndX, EndY)
		);
	}
}

public class CircleEnemyData : EnemyData
{
	[XmlAttribute] public float Amplitude { get; set; }
	[XmlAttribute] public float Frequency { get; set; }

	public override CircleEnemy ToEnemy()
	{
		return new CircleEnemy(
			new(StartX, StartY),
			Speed,
			Amplitude
		);
	}
}
