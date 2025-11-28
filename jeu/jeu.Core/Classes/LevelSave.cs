using System.Collections.Generic;
using System.Xml.Serialization;

namespace jeu.Core.Classes;

public class LevelSave
{
    [XmlAttribute] public int Id { get; set; }
    [XmlAttribute] public bool Completed { get; set; }
}

[XmlRoot("SaveLevels")]
public class LevelsSave
{
    [XmlElement("Level")]
    public List<LevelSave> Levels { get; set; } = new List<LevelSave>();
}