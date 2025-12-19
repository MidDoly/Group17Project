using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Kéo Player vào đây
    public float smoothSpeed = 0.125f; // Độ mượt khi di chuyển
    public Vector3 offset = new Vector3(0, 0, -10); // Giữ camera luôn ở trên cao (-10 z)

    void LateUpdate() // Dùng LateUpdate để tránh camera bị rung
    {
        if (target != null)
        {
            // Vị trí mong muốn
            Vector3 desiredPosition = target.position + offset;
            // Dịch chuyển từ từ (Lerp) để tạo độ mượt
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}