using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSaveData
{
    public int level;
    public int exp;
    public int gold;

    public int maxLevelReached;
    public List<bool> unlockedSkins;
    public int currentSkinIndex;
    public PlayerSaveData(PlayerStats stats)
    {
        this.level = stats.level;
        this.exp = stats.exp;
        this.gold = stats.gold;
        
        this.maxLevelReached = stats.maxLevelReached;
        this.unlockedSkins = new List<bool>(stats.unlockedSkins);
        this.currentSkinIndex = stats.currentSkinIndex;
    }
}