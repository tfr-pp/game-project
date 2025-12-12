using System.Collections.Generic;
using System.Xml.Serialization;

namespace jeu.Core.Classes.Model;

/** Saves info on a level for a player (is it completed, time spent, lives left) gotten from XML
 * 
 */
public class LevelSave
{
	[XmlAttribute] public string Id { get; set; }
	[XmlAttribute] public bool Completed { get; set; }
	[XmlElement] public float TimeSpent { get; set; } = 0f;
	[XmlElement] public int LivesLeft { get; set; } = 0;
}

[XmlRoot("SaveLevels")]
public class LevelsSave
{
	[XmlElement("Level")]
	public List<LevelSave> Levels { get; set; } = [];
}