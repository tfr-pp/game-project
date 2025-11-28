using System;
using System.Xml.Serialization;

namespace jeu.Core.Classes;

[XmlRoot("PlayerProfile")]
public class PlayerProfile
{
    [XmlAttribute] public string Id { get; set; }
    [XmlElement] public string Name { get; set; }
    [XmlElement] public DateTime CreationDate { get; set; }

    [XmlElement] public LevelsSave Levels { get; set; } = new LevelsSave();

    // Options du joueur
    [XmlElement] public float MusicVolume { get; set; } = 80;
    [XmlElement] public float SfxVolume { get; set; } = 80;
}