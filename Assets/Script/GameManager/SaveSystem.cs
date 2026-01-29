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

            // Đổ dữ liệu ngược lại vào ScriptableObject
            stats.level = data.level;
            stats.exp = data.exp;
            stats.gold = data.gold;

            Debug.Log("Game Loaded Successfully!");
        }
        else
        {
            Debug.LogWarning("Save file not found. Creating new one or using defaults.");
        }
    }
}