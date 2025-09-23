// 9/7/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using UnityEditor;
using UnityEngine;
using System.Collections;

public class spawnEnemy : MonoBehaviour
{
    public static spawnEnemy Instance;
    public int maxEnemys = 10;          // Số quái tối đa trên bản đồ
    public int enemyPerSpawn = 2;      // Số quái spawn mỗi lần
    [SerializeField] private GameObject[] enemies;          // Quái thường
    [SerializeField] private GameObject miniBossPrefab;     // Mini Boss
    [SerializeField] private GameObject bossPrefab;         // Boss lớn

    private Transform player;              // Tham chiếu tới nhân vật chính
    [SerializeField] private float spawnRadius = 5f;        // Bán kính spawn xung quanh nhân vật
    [SerializeField] private float time = 2f;               // Thời gian spawn quái thường

    private float timer = 0f;                // Thời gian chơi tổng cộng
    private bool miniBossSpawned = false;    // Kiểm tra đã spawn mini boss chưa
    private bool bossSpawned = false;        // Kiểm tra đã spawn boss chưa
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 300f && !miniBossSpawned) // Sau 5 phút
        {
            SpawnMiniBoss();
            miniBossSpawned = true;
        }

        if (timer >= 600f && !bossSpawned) // Sau 10 phút
        {
            SpawnBoss();
            bossSpawned = true;
        }
    }

    private void SpawnEnemy(int enemyCount, int[] enemyType)
    {
       
        float angleStep = 360f / enemyCount; // Góc giữa mỗi enemy
        float angle = 0f;

        for (int i = 0; i < enemyCount; i++)
        {
            
            // Tính toán vị trí spawn chia đều trên đường tròn
            float spawnX = player.position.x + spawnRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
            float spawnY = player.position.y + spawnRadius * Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector2 spawnPosition = new Vector2(spawnX, spawnY);
            var rdIndex = enemyType[Random.Range(0, enemyType.Length)];
            GameObject enemy = enemies[rdIndex];
            Instantiate(enemy, spawnPosition, Quaternion.identity);

            angle += angleStep; // Tăng góc để chia đều
        }
    }

    private void SpawnMiniBoss()
    {
        Vector2 spawnPosition = (Vector2)player.position + new Vector2(spawnRadius, 0); // Spawn mini boss ở vị trí cố định
        Instantiate(miniBossPrefab, spawnPosition, Quaternion.identity);
        Debug.Log("Mini Boss đã xuất hiện! ⚔️");
    }

    public void SpawnBoss()
    {
        Vector2 spawnPosition = (Vector2)player.position + new Vector2(spawnRadius, 0); // Spawn boss ở vị trí cố định
        Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
        Debug.Log("Boss to tổ bố xuất hiện rồi đó Sensei!! 💀💢");
    }

    internal void StartSpawning(int enemyPerSpawn, int enemySpawnCount, float spawnInterval, int[] enemyType)
    {
        StartCoroutine(Spawn(enemyPerSpawn, enemySpawnCount, spawnInterval, enemyType));
    }

    public IEnumerator Spawn(int enemyPerSpawn, int enemySpawnCount, float spawnInterval, int[] enemyType)
    {
        int spawns = 0;
        while (spawns < enemySpawnCount)
        {
            SpawnEnemy(enemyPerSpawn, enemyType);
            spawns++;
            if(spawns >= enemySpawnCount)
            {
                GameManager.IsSpawnedCheckpoint = true;
                yield break; // Kết thúc coroutine nếu đã spawn đủ số lần
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
