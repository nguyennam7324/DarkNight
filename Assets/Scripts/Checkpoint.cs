// 9/7/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEditor;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public spawnEnemy enemySpawner; // Tham chiếu tới script spawn enemy
    public int enemyPerSpawn = 5; // Số lượng enemy mỗi spawn
    public int enemySpawnCount = 5; // Số lần spawn enemy
    public float spawnInterval = 1f; // Thời gian giữa mỗi lần spawn

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Kiểm tra nếu nhân vật chạm vào checkpoint
        {
            if (enemySpawner != null)
            {
                GameManager.IsSpawnedCheckpoint = false;
                enemySpawner.StartSpawning(enemyPerSpawn, enemySpawnCount, spawnInterval); // Kích hoạt spawn enemy với cấu hình riêng
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
}
