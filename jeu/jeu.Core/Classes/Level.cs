using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace jeu.Core.Classes;

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
	[XmlArrayItem("SineEnemy", typeof(SineEnemyData))]
	public List<EnemyData> enemies;

	public static Level LoadLevel(string path)
	{
		path = Path.Combine(AppContext.BaseDirectory, "Content", "Levels", path);
		using var stream = File.OpenRead(path);
		var serializer = new XmlSerializer(typeof(Level));
		return (Level)serializer.Deserialize(stream);
	}
}
