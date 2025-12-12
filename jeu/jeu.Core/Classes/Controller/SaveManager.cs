using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using jeu.Core.Classes.Model;

namespace jeu.Core.Classes.Controller;

/** A SaveManager of a skylink game
 * saves game data in "./Saves"
 */
public class SaveManager
{
	private readonly string _folder = "Saves";

	/** Constructor
	 * Creates "./Saves" folder if it doesn't exists
	 */
	public SaveManager()
	{
		if (!Directory.Exists(_folder)) Directory.CreateDirectory(_folder);
	}

	private string GetFilePath(string playerId)
	{
		return Path.Combine(_folder, $"{playerId}.xml");
	}

	/** Loads the profile of a player
	 * \param playerId the string id of a player
	 * \return A new PlayerProfile of the loaded player profile
	 */
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

		using FileStream stream = File.OpenRead(file);
		return (PlayerProfile)new XmlSerializer(typeof(PlayerProfile)).Deserialize(stream);
	}

	/** Saves the profile of a player
	 * \param profile a PlayerProfile to save
	 */
	public void SaveProfile(PlayerProfile profile)
	{
		using FileStream stream = File.Create(GetFilePath(profile.Id));
		new XmlSerializer(typeof(PlayerProfile)).Serialize(stream, profile);
	}

	/** Sets the scores when a player win a level
	 * \param profile a PlayerProfile to add scores
	 * \param levelId a string, the id  of won level
	 * \param timeSpent a float, the time spent on level
	 * \param livesLeft an int, the lives left when player won
	 */
	public void CompleteLevel(PlayerProfile profile, string levelId, float timeSpent, int livesLeft)
	{
		LevelSave level = profile.Levels.Levels.Find(l => l.Id == levelId);
		if (level == null)
		{
			profile.Levels.Levels.Add(new LevelSave
			{
				Id = levelId,
				Completed = true,
				TimeSpent = timeSpent,
				LivesLeft = livesLeft
			});
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

	/** Loads every registered player profile
	 * \return A List<> of every PlayerProfile registered
	 */
	public List<PlayerProfile> LoadAllProfiles()
	{
		List<PlayerProfile> profiles = [];
		if (!Directory.Exists(_folder))
			return profiles;

		XmlSerializer serializer = new(typeof(PlayerProfile));
		foreach (string file in Directory.GetFiles(_folder, "*.xml"))
		{
			using FileStream stream = File.OpenRead(file);
			profiles.Add((PlayerProfile)serializer.Deserialize(stream));
		}

		return profiles;
	}
}