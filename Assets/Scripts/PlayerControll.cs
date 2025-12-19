using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControll : MonoBehaviour
{
    [Header("Movement Settings")]
    public Rigidbody2D rb;
    public float moveSpeed = 5.0f;

    [Header("Combat Settings")]
    public Transform attackPoint;      // Kéo GameObject AttackPoint vào đây
    public float attackRange = 0.5f;   // Phạm vi đánh
    public LayerMask enemyLayers;      // Chọn Layer Enemy
    public int attackDamage = 1;

    // Input & Animation
    private InputSystem_Actions playerControls;
    private InputAction move;
    private InputAction fire;
    private Animator anim;
    
    private Vector2 moveDirection = Vector2.zero;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>(); // Đảm bảo đã lấy Rigidbody
        playerControls = new InputSystem_Actions();
    }

    void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();

        fire = playerControls.Player.Attack;
        fire.Enable();
        fire.performed += OnAttack;
    }

    void OnDisable()
    {
        move.Disable();
        fire.Disable();
        fire.performed -= OnAttack;
    }

    // --- LOGIC TẤN CÔNG ---
    private void OnAttack(InputAction.CallbackContext context)
    {
        // Kiểm tra xem game có đang pause không, hoặc nhân vật có đang chết không (tùy chỉnh sau)
        DoAttack();
    }

    private void DoAttack()
    {
        // 1. Chạy Animation
        if (anim != null)
        {
            anim.SetTrigger("Fire"); // Đảm bảo Animator của bạn có Trigger tên "Fire" hoặc "Attack"
        }

        // 2. Phát hiện quái vật trong phạm vi đánh
        // Lưu ý: AttackPoint cần là một GameObject con nằm phía trước mặt Player
        if (attackPoint == null) return;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // 3. Gây sát thương
        foreach (Collider2D enemy in hitEnemies)
        {
            // Gọi Interface IDamageable (bạn cần đảm bảo script EnemyHealth kế thừa từ interface này)
            var damageable = enemy.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(attackDamage);
            }
        }
    }

    // --- LOGIC DI CHUYỂN & UPDATE ---
    void Update()
    {
        // Đọc dữ liệu di chuyển (Vector2: x, y)
        moveDirection = move.ReadValue<Vector2>();

        // Cập nhật Animation "Speed" để chuyển từ đứng yên sang chạy
        // Dùng sqrMagnitude để tối ưu hơn magnitude (tránh căn bậc 2 liên tục)
        if (anim != null)
        {
            anim.SetFloat("Speed", moveDirection.sqrMagnitude);
        }

        // Xử lý quay mặt (Flip) theo hướng di chuyển
        if (moveDirection.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveDirection.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void FixedUpdate()
    {
        // Di chuyển Top-Down: Tác động vận tốc lên cả X và Y
        // Sử dụng moveDirection.normalized nếu bạn không muốn đi chéo nhanh hơn đi thẳng
        // Nhưng Input System thường đã xử lý normalize cho Gamepad, với Keyboard thì nên thêm .normalized nếu cần.
        rb.linearVelocity = moveDirection * moveSpeed;
    }

    // Vẽ vùng tấn công trong Editor để dễ căn chỉnh
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}