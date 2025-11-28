using System.IO;
using System.Xml.Serialization;

namespace jeu.Core.Classes;

public class SaveManager
{
    private string _folder = "Saves";
    private string _file => Path.Combine(_folder, "SaveLevels.xml");

    public SaveManager()
    {
        if (!Directory.Exists(_folder))
            Directory.CreateDirectory(_folder);
    }

    public LevelsSave LoadLevels()
    {
        if (!File.Exists(_file))
            return new LevelsSave();
        
        XmlSerializer serializer = new XmlSerializer(typeof(LevelsSave));
        using (FileStream stream = File.OpenRead(_file))
            return (LevelsSave)serializer.Deserialize(stream);
    }

    public void CompleteLevel(int levelId)
    {
        LevelsSave save = LoadLevels();
        LevelSave level = save.Levels.Find(l => l.Id == levelId);
        if (level == null)
        {
            level = new LevelSave { Id = levelId, Completed = true };
            save.Levels.Add(level);
        }
        else
            level.Completed = true;

        XmlSerializer serializer = new XmlSerializer(typeof(LevelsSave));
        using (FileStream stream = File.Create(_file))
            serializer.Serialize(stream, save);
    }

    public bool IsLevelUnlocked(int levelId)
    {
        if (levelId == 1)
            return true; // Pour que le premier soit toujours dispo
        
        LevelsSave save = LoadLevels();
        LevelSave prev = save.Levels.Find(l => l.Id == levelId - 1);
        return prev != null && prev.Completed;
    }
}