using System.Collections; // Cần thêm dòng này để dùng Coroutine
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleEnemyAI : MonoBehaviour
{
    private Transform player;
    
    [Header("Settings")]
    public float moveSpeed = 2f;
    [SerializeField] private float startWaitTime = 2f; // Thời gian đứng yên lúc đầu (giây)

    private bool isReady = false; // Biến cờ: false là đứng yên, true là được đi
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Tìm Player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        // Bắt đầu đếm ngược thời gian đứng yên
        StartCoroutine(WaitBeforeMoving());
    }

    // Hàm đếm ngược
    IEnumerator WaitBeforeMoving()
    {
        // Chờ trong khoảng thời gian startWaitTime
        yield return new WaitForSeconds(startWaitTime);
        
        // Sau khi chờ xong, bật cờ lên để cho phép di chuyển
        isReady = true;
    }

    void FixedUpdate()
    {
        // Nếu chưa sẵn sàng (đang trong thời gian chờ) hoặc không tìm thấy player thì không làm gì cả
        if (!isReady || player == null) return;

        // Logic di chuyển cũ
        Vector2 direction = (player.position - transform.position).normalized;
        rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.fixedDeltaTime));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            KillPlayer(collision.gameObject);
        }
    }

    void KillPlayer(GameObject playerObject)
    {
        Debug.Log("Player Died!");
        Destroy(playerObject); 
        Invoke("ReloadScene", 1f); 
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}