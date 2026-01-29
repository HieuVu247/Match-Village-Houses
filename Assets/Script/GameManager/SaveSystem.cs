using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveSystem
{
    private static string path = Application.persistentDataPath + "/playerdata.json";

    public static void SaveData(PlayerStats stats)
    {
        PlayerSaveData data = new PlayerSaveData(stats);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
        
        Debug.Log("Game Saved to: " + path);
    }

    public static void LoadData(PlayerStats stats)
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerSaveData data = JsonUtility.FromJson<PlayerSaveData>(json);

            stats.level = data.level;
            stats.exp = data.exp;
            stats.gold = data.gold;

            stats.maxLevelReached = data.maxLevelReached;
            stats.currentSkinIndex = data.currentSkinIndex;
            
            if (data.unlockedSkins != null && data.unlockedSkins.Count > 0)
            {
                stats.unlockedSkins = new List<bool>(data.unlockedSkins);
            }
            
            Debug.Log("Game Loaded Successfully!");
        }
        else
        {
            Debug.LogWarning("Save File nonexist. Creating new save file");
            
            stats.level = 1;   
            stats.exp = 0;
            stats.gold = 0;   
            
            stats.maxLevelReached = 0;
            stats.unlockedSkins = new List<bool> { true, false, false, false, false, false };
            stats.currentSkinIndex = 0;
            SaveData(stats);
        }
    }
}