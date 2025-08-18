using UnityEngine;

public class SniperBullet : MonoBehaviour
{
    [Header("Thiết lập đạn Sniper")]
    [SerializeField] private float moveSpeed = 35f;          // tốc độ đạn cao
    [SerializeField] private float lifeTime = 2.5f;          // tự hủy sau 2.5s
    [SerializeField] private float baseDamage = 120f;        // damage cơ bản
    [SerializeField] private int pierceCount = 7;            // số kẻ địch xuyên tối đa
    [SerializeField] private GameObject bloodPrefab;         // prefab hiệu ứng máu

    private float damage;
    private int enemiesHit = 0;
    private Player player;

    void Start()
    {
        // Tìm Player
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        // Tính damage có crit
        damage = player.CalculateDamage(baseDamage);

        // Hút máu nếu có
        if (player.lifeSteal > 0)
        {
            float healAmount = damage * player.lifeSteal;
            player.Heal(healAmount);
            Debug.Log($"🩸 Hút máu: {healAmount}");
        }

        Destroy(gameObject, lifeTime); // tránh tồn tại mãi
    }

    void Update()
    {
        // Bay thẳng
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);

                // Spawn blood
                if (bloodPrefab != null)
                {
                    GameObject blood = Instantiate(bloodPrefab, transform.position, Quaternion.identity);
                    Destroy(blood, 1f);
                }
            }

            enemiesHit++;

            // Nếu đạt giới hạn xuyên thì hủy
            if (enemiesHit >= pierceCount)
            {
                Destroy(gameObject);
            }
        }

        if (collision.CompareTag("Wall"))
        {
            Debug.Log("💥 SniperBullet Hit Wall");
            Destroy(gameObject);
        }
    }
}
