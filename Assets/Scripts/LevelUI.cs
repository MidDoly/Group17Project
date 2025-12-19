using UnityEngine;
using TMPro; // Thư viện TextMeshPro

public class LevelUI : MonoBehaviour
{
    private TextMeshProUGUI levelText;

    void Start()
    {
        levelText = GetComponent<TextMeshProUGUI>();
        
        // Lấy số level từ biến static bên Spawner để hiển thị
        levelText.text = "Floor: " + DungeonEnemySpawner.currentLevel;
    }
}