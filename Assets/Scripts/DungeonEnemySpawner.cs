using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonEnemySpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int baseEnemyCount = 5;
    [SerializeField] private float delayBeforeNextLevel = 1.5f;
    
    [Header("UI References")]
    [SerializeField] private GameObject victoryPanel; // Kéo cái VictoryPanel vào đây

    private int currentEnemiesAlive = 0;
    public static int currentLevel = 1; 

    // Biến quy định số màn chơi tối đa
    private const int MAX_LEVEL = 5; 

    public void SpawnEnemies(IEnumerable<Vector2Int> floorPositions)
    {
        // Nếu vừa vào lại game từ menu, nhớ đảm bảo victoryPanel đang tắt
        if(victoryPanel != null) victoryPanel.SetActive(false);

        // Logic sinh quái cũ giữ nguyên...
        int enemiesToSpawn = baseEnemyCount + ((currentLevel - 1) * currentLevel);
        currentEnemiesAlive = 0;

        List<Vector2Int> floors = new List<Vector2Int>(floorPositions);
        // ... (Đoạn code Random và Instantiate quái giữ nguyên như cũ) ...
        // Tóm tắt đoạn sinh quái để code ngắn gọn:
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            if (floors.Count == 0) break;
            int r = Random.Range(0, floors.Count);
            Instantiate(enemyPrefab, new Vector3(floors[r].x + 0.5f, floors[r].y + 0.5f, 0), Quaternion.identity, transform);
            floors.RemoveAt(r);
            currentEnemiesAlive++;
        }
    }

    public void OnEnemyKilled()
    {
        currentEnemiesAlive--;
        
        if (currentEnemiesAlive <= 0)
        {
            // Kiểm tra: Nếu đã đến Level 5 (hoặc cao hơn) thì THẮNG
            if (currentLevel >= MAX_LEVEL)
            {
                WinGame();
            }
            else
            {
                // Nếu chưa đến lv5 thì đi tiếp
                Debug.Log("Room Cleared! Next level...");
                StartCoroutine(LoadNextLevelRoutine());
            }
        }
    }

    void WinGame()
    {
        Debug.Log("VICTORY!");
        // Hiện bảng chiến thắng
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }
        
        // Dừng thời gian lại để nhân vật không chạy lung tung nữa
        Time.timeScale = 0; 
    }

    IEnumerator LoadNextLevelRoutine()
    {
        yield return new WaitForSeconds(delayBeforeNextLevel);
        currentLevel++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Hàm dùng cho nút "Chơi lại" hoặc "Về Menu"
    public void ResetGame()
    {
        currentLevel = 1; // Reset level về 1
        Time.timeScale = 1; // Mở lại thời gian (quan trọng)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Hoặc load lại Scene hiện tại để chơi lại
    }
}