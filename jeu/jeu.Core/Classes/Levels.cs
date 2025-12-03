using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace jeu.Core.Classes;

[XmlRoot("Levels")]
public class Levels
{
	[XmlElement("Path")]
	public List<PathEntry> Paths { get; private set; } = [];

	public static Levels LoadLevels()
	{
		var path = Path.Combine(AppContext.BaseDirectory, "Content", "Levels", "levels.xml");
		using var stream = File.OpenRead(path);
		var serializer = new XmlSerializer(typeof(Levels));
		return (Levels)serializer.Deserialize(stream);
	}

	public Level GetLevel(int index)
	{
		return Level.LoadLevel(Path.Combine(AppContext.BaseDirectory, "Content", "Levels", Paths[index].Value));
	}
}

public class PathEntry
{
	[XmlAttribute("value")]
	public string Value;
}