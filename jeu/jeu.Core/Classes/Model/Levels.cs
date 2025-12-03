using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace jeu.Core.Classes.Model;

[XmlRoot("Levels")]
public class Levels
{
	[XmlElement("Level")]
	public List<LevelEntry> LevelEntries = [];

	public static Levels LoadLevels()
	{
		var path = Path.Combine(AppContext.BaseDirectory, "Content", "Levels", "levels.xml");
		using var stream = File.OpenRead(path);
		var serializer = new XmlSerializer(typeof(Levels));
		return (Levels)serializer.Deserialize(stream);
	}

	public Level GetLevel(int index)
	{
		return Level.LoadLevel(LevelEntries[index].Path);
	}

	public Level GetLevel(string id)
	{
		return Level.LoadLevel(LevelEntries.Find(entry => entry.Id == id).Path);
	}
}

public class LevelEntry
{
	[XmlAttribute("path")]
	public string Path { get; set; }

	[XmlAttribute("id")]
	public string Id { get; set; }

	[XmlAttribute("name")]
	public string Name { get; set; }
}