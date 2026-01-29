using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkinShopManager : MonoBehaviour
{
    public PlayerStats playerStats;
    public SkinData[] skins;

    public Button[] skinButtons;
    public TextMeshProUGUI[] skinButtonTexts;

    private int equippedIndex = 0;

    void Start()
    {
        SyncDataFromPlayerStats();

        for (int i = 0; i < skins.Length; i++)
        {
            int index = i;
            skinButtons[i].onClick.AddListener(() => OnSkinButtonClicked(index));
            UpdateButton(index);
        }

        EquipSkin(playerStats.currentSkinIndex);
    }

    void SyncDataFromPlayerStats()
    {
        // Đảm bảo list trong PlayerStats đủ số lượng (đề phòng lỗi null hoặc thiếu)
        if (playerStats.unlockedSkins == null || playerStats.unlockedSkins.Count < skins.Length)
        {
            Debug.LogWarning("Danh sách skin trong PlayerStats không khớp, đang tự sửa...");
            if (playerStats.unlockedSkins == null) playerStats.unlockedSkins = new System.Collections.Generic.List<bool>();
            
            while (playerStats.unlockedSkins.Count < skins.Length)
            {
                playerStats.unlockedSkins.Add(false); // Mặc định là chưa mở khóa
            }
            playerStats.unlockedSkins[0] = true; // Skin mặc định luôn mở
        }

        // Copy trạng thái từ PlayerStats sang biến isUnlocked của từng nút skin
        for (int i = 0; i < skins.Length; i++)
        {
            skins[i].isUnlocked = playerStats.unlockedSkins[i];
        }
    }
    
    void OnSkinButtonClicked(int index)
    {
        Debug.Log($"Skin button {index} clicked");

        if (!playerStats.unlockedSkins[index]) 
        {
            if (playerStats.gold >= skins[index].price)
            {
                playerStats.gold -= skins[index].price;
                
                skins[index].isUnlocked = true;
                playerStats.unlockedSkins[index] = true;

                SaveSystem.SaveData(playerStats);
                
                UpdateButton(index);
                
                // Cập nhật UI tiền (nếu có UI hiển thị tiền ở đây thì gọi hàm update)
                // UIManager.Instance.UpdateGoldUI(); 
            }
            else
            {
                Debug.Log("Not enough Gold!");
            }
        }
        else
        {
            EquipSkin(index);
        }
    }

    void EquipSkin(int index)
    {
        playerStats.currentSkinIndex = index;
        SaveSystem.SaveData(playerStats);
        for (int i = 0; i < skins.Length; i++)
        {
            if (skins[i].isUnlocked)
            {
                skinButtonTexts[i].text = (i == index) ? "Equipped" : "Equip";
            }
            else
            {
                skinButtonTexts[i].text = skins[i].price + " Gold";
            }
        }
    }

    public int GetEquippedSkinIndex()
    {
        return equippedIndex;
    }

    void UpdateButton(int index)
    {
        if (skinButtonTexts[index] != null)
        {
            if (playerStats.currentSkinIndex == index)
            {
                skinButtonTexts[index].text = "Equipped";
            }
            else if (playerStats.unlockedSkins[index])
            {
                skinButtonTexts[index].text = "Select";
            }
            else
            {
                skinButtonTexts[index].text = skins[index].price.ToString();
            }
        }
    }

    void SaveData()
    {
        SkinSaveData data = new SkinSaveData();
        data.unlocked = new bool[skins.Length];
        for (int i = 0; i < skins.Length; i++)
        {
            data.unlocked[i] = skins[i].isUnlocked;
        }
        data.equippedIndex = equippedIndex;

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("SkinSaveData", json);
        PlayerPrefs.Save();

        Debug.Log("Skin data saved");
    }

    void LoadData()
    {
        if (PlayerPrefs.HasKey("SkinSaveData"))
        {
            string json = PlayerPrefs.GetString("SkinSaveData");
            SkinSaveData data = JsonUtility.FromJson<SkinSaveData>(json);

            if (data.unlocked.Length == skins.Length)
            {
                for (int i = 0; i < skins.Length; i++)
                {
                    skins[i].isUnlocked = data.unlocked[i];
                }
                equippedIndex = data.equippedIndex;
            }
            else
            {
                Debug.LogWarning("SkinSaveData length mismatch");
            }
        }
        else
        {
            Debug.Log("No skin save data found, using default");
        }
    }
}

[System.Serializable]
public class SkinSaveData
{
    public bool[] unlocked;
    public int equippedIndex;
}

[System.Serializable]
public class SkinData
{
    public string skinName;
    public int price;
    public bool isUnlocked;
}
