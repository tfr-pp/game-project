using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace jeu.Core.Classes.Model;

[XmlRoot("Level")]
public class Level
{
	[XmlArray("Track")]
	[XmlArrayItem("Point")]
	public List<Point> trackPoints;

	[XmlElement("Name")]
	public string name;

	[XmlElement("Id")]
	public string id;

	[XmlArray("Enemies")]
	[XmlArrayItem("HorizontalPatrolEnemy", typeof(HorizontalPatrolEnemyData))]
	[XmlArrayItem("CircleEnemy", typeof(CircleEnemyData))]
	public List<EnemyData> enemies;

	public static Level LoadLevel(string path)
	{
		path = Path.Combine(AppContext.BaseDirectory, "Content", "Levels", path);
		using var stream = File.OpenRead(path);
		return (Level)new XmlSerializer(typeof(Level)).Deserialize(stream);
	}
}
