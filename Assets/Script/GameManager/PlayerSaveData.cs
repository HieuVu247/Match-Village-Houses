using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSaveData
{
    // Đã đổi tên biến cho khớp với PlayerStats.cs
    public int level;
    public int exp;
    public int gold;
    public int diamond;

    // Hàm tạo (Constructor)
    public PlayerSaveData(PlayerStats stats)
    {
        // Lấy dữ liệu từ ScriptableObject gốc
        this.level = stats.level;
        this.exp = stats.exp;
        this.gold = stats.gold;
    }
}