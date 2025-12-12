using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace jeu.Core.Classes.Model;

/** Contains level data
 * name, level id, its track, its ennemies
 */
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

	/** Load a level from data
	 * \param path the relative path to XML level data
	 * \return A new deserialized level
	 */
	public static Level LoadLevel(string path)
	{
		path = Path.Combine(AppContext.BaseDirectory, "Content", "Levels", path);
		FileStream stream = File.OpenRead(path);
		return (Level)new XmlSerializer(typeof(Level)).Deserialize(stream);
	}
}
