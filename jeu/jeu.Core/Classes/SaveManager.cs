using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace jeu.Core.Classes;

public class SaveManager
{
	private readonly string _folder = "Saves";

	public SaveManager()
	{
		if (!Directory.Exists(_folder))
			Directory.CreateDirectory(_folder);
	}

	private string GetFilePath(string playerId)
	{
		return Path.Combine(_folder, $"{playerId}.xml");
	}

	// Charge le profil d'un joueur depuis XML
	public PlayerProfile LoadProfile(string playerId)
	{
		string file = GetFilePath(playerId);
		if (!File.Exists(file))
		{
			return new PlayerProfile
			{
				Id = playerId,
				Name = playerId,
				CreationDate = DateTime.Now,
				Levels = new LevelsSave()
			};
		}

		XmlSerializer serializer = new(typeof(PlayerProfile));
		using FileStream stream = File.OpenRead(file);
		return (PlayerProfile)serializer.Deserialize(stream);
	}

	// Sauvegarde un profil
	public void SaveProfile(PlayerProfile profile)
	{
		string file = GetFilePath(profile.Id);
		XmlSerializer serializer = new(typeof(PlayerProfile));
		using FileStream stream = File.Create(file);
		serializer.Serialize(stream, profile);
	}

	// Complète un niveau et met à jour temps + passagers
	public void CompleteLevel(PlayerProfile profile, int levelId, float timeSpent, int passengersLeft)
	{
		LevelSave level = profile.Levels.Levels.Find(l => l.Id == levelId);
		if (level == null)
		{
			level = new LevelSave
			{
				Id = levelId,
				Completed = true,
				TimeSpent = timeSpent,
				PassengersLeft = passengersLeft
			};
			profile.Levels.Levels.Add(level);
		}
		else
		{
			level.Completed = true;
			level.TimeSpent = timeSpent;
			level.PassengersLeft = passengersLeft;
		}

		SaveProfile(profile);
	}

	public static bool IsLevelUnlocked(PlayerProfile profile, int levelId)
	{
		if (levelId == 1)
			return true; // Pour que le premier soit toujours dispo

		LevelSave prev = profile.Levels.Levels.Find(l => l.Id == levelId - 1);
		return prev != null && prev.Completed;
	}

	// Liste tous les profils existants
	public List<PlayerProfile> LoadAllProfiles()
	{
		List<PlayerProfile> profiles = [];
		if (!Directory.Exists(_folder))
			return profiles;

		foreach (string file in Directory.GetFiles(_folder, "*.xml"))
		{
			XmlSerializer serializer = new(typeof(PlayerProfile));
			using FileStream stream = File.OpenRead(file);
			profiles.Add((PlayerProfile)serializer.Deserialize(stream));
		}

		return profiles;
	}
}