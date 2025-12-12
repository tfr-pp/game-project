using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using jeu.Core.Classes.Model;

namespace jeu.Core.Classes.Controller;

public class SaveManager
{
	private readonly string _folder = "Saves";

	public SaveManager()
	{
		if (!Directory.Exists(_folder)) Directory.CreateDirectory(_folder);
	}

	private string GetFilePath(string playerId)
	{
		return Path.Combine(_folder, $"{playerId}.xml");
	}

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

	public void SaveProfile(PlayerProfile profile)
	{
		string file = GetFilePath(profile.Id);
		XmlSerializer serializer = new(typeof(PlayerProfile));
		using FileStream stream = File.Create(file);
		serializer.Serialize(stream, profile);
	}

	public void CompleteLevel(PlayerProfile profile, string levelId, float timeSpent, int livesLeft)
	{
		LevelSave level = profile.Levels.Levels.Find(l => l.Id == levelId);
		if (level == null)
		{
			level = new LevelSave
			{
				Id = levelId,
				Completed = true,
				TimeSpent = timeSpent,
				LivesLeft = livesLeft
			};
			profile.Levels.Levels.Add(level);
		}
		else
		{
			level.Id = levelId;
			level.Completed = true;
			level.TimeSpent = timeSpent;
			level.LivesLeft = livesLeft;
		}

		SaveProfile(profile);
	}

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