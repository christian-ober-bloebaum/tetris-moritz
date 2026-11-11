using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Tetris
{
    public class UserProfile
    {
        public string Name { get; set; } = string.Empty;
        public int Highscore { get; set; }
        public bool IsGuest { get; set; }
    }

    public class SaveData
    {
        public List<UserProfile> Accounts { get; set; } = new List<UserProfile>();
        public string LastActiveAccountName { get; set; } = string.Empty;
    }
    public static class ProfileManager
    {
        private static readonly string FolderPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "TetrisGame"
        );
        private static readonly string FilePath = Path.Combine(FolderPath, "profiles.json");

        public static SaveData LoadData()
        {
            if (!File.Exists(FilePath))
            {
                // Standard-Daten, falls noch keine Datei existiert
                return new SaveData();
            }

            try
            {
                string json = File.ReadAllText(FilePath);
                return JsonSerializer.Deserialize<SaveData>(json) ?? new SaveData();
            }
            catch
            {
                return new SaveData();
            }
        }

        public static void SaveDataToFile(SaveData data)
        {
            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }

            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
    }
}
