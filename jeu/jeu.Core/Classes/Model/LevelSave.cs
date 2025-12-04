using System.Collections.Generic;
using System.Xml.Serialization;

namespace jeu.Core.Classes.Model;

public class LevelSave
{
	[XmlAttribute] public string Id { get; set; }
	[XmlAttribute] public bool Completed { get; set; }
	[XmlElement] public float TimeSpent { get; set; } = 0f;       // Temps mis pour finir le niveau
	[XmlElement] public int LivesLeft { get; set; } = 0;     // Nombre de vies restantes
}

[XmlRoot("SaveLevels")]
public class LevelsSave
{
	[XmlElement("Level")]
	public List<LevelSave> Levels { get; set; } = [];
}