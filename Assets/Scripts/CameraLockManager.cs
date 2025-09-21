// 9/8/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CameraLockManager : MonoBehaviour
{
    public Transform cameraTransform; // Tham chiếu tới camera
    public Transform checkpointStart; // Checkpoint hiện tại
    public Transform checkpointEnd;   // Checkpoint tiếp theo
    public spawnEnemy enemySpawner;   // Tham chiếu tới script spawnEnemy

    private bool isLocked = true;

    void Update()
    {
        if (isLocked)
        {
            Vector3 clampedPosition = cameraTransform.position;

            // Kiểm tra vị trí checkpoint để khóa camera theo trục X hoặc Y
            if (checkpointStart.position.y != checkpointEnd.position.y)
            {
                // Khóa camera theo trục Y
                clampedPosition.y = Mathf.Clamp(cameraTransform.position.y, checkpointStart.position.y, checkpointEnd.position.y);
                clampedPosition.x = checkpointStart.position.x; // Giữ nguyên trục X
            }
            else if (checkpointStart.position.x != checkpointEnd.position.x)
            {
                // Khóa camera theo trục X
                clampedPosition.x = Mathf.Clamp(cameraTransform.position.x, checkpointStart.position.x, checkpointEnd.position.x);
                clampedPosition.y = checkpointStart.position.y; // Giữ nguyên trục Y
            }

            cameraTransform.position = clampedPosition;

            // Kiểm tra nếu tất cả quái vật đã bị tiêu diệt
            if (enemySpawner != null && GameObject.FindGameObjectsWithTag("Enemy").Count() == 0)
            {
                isLocked = false; // Mở khóa camera
            }
        }
    }
}
