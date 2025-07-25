using UnityEngine;
using System.Collections;

public class spawnEnemy : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;          // Quái thường
    [SerializeField] private GameObject miniBossPrefab;     // Mini Boss
    [SerializeField] private GameObject bossPrefab;         // Boss lớn

    [SerializeField] private Transform[] spawnLocation;     // Vị trí spawn
    [SerializeField] private float time = 2f;               // Thời gian spawn quái thường

    private float timer = 0f;                // Thời gian chơi tổng cộng
    private bool miniBossSpawned = false;    // Kiểm tra đã spawn mini boss chưa
    private bool bossSpawned = false;        // Kiểm tra đã spawn boss chưa

    void Start()
    {
        StartCoroutine(SpawnEnemy());
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

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            GameObject enemy = enemies[Random.Range(0, enemies.Length)];
            Transform spawn = spawnLocation[Random.Range(0, spawnLocation.Length)];
            Instantiate(enemy, spawn.position, Quaternion.identity);
        }
    }

    private void SpawnMiniBoss()
    {
        Transform spawn = spawnLocation[Random.Range(0, spawnLocation.Length)];
        Instantiate(miniBossPrefab, spawn.position, Quaternion.identity);
        Debug.Log("Mini Boss đã xuất hiện! ⚔️");
    }

    private void SpawnBoss()
    {
        Transform spawn = spawnLocation[Random.Range(0, spawnLocation.Length)];
        Instantiate(bossPrefab, spawn.position, Quaternion.identity);
        Debug.Log("Boss to tổ bố xuất hiện rồi đó Sensei!! 💀💢");
    }
}
