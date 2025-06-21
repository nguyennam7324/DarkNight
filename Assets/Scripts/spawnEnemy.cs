using System.Collections;
using UnityEngine;

public class spawnEnemy : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private Transform[] spawnLocation;
    [SerializeField] private float time = 2f;
    void Start()
    {
        StartCoroutine(SpawnEnemy());
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
}
