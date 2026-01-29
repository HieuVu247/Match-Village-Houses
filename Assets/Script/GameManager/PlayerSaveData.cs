using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSaveData
{
    public int level;
    public int exp;
    public int gold;
    public int diamond;

    public PlayerSaveData(PlayerStats stats)
    {
        this.level = stats.level;
        this.exp = stats.exp;
        this.gold = stats.gold;
    }
}