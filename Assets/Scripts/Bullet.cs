using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float moveBullet = 25f;
    [SerializeField] private float timeDestroy = 0.15f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private GameObject bloodPrefab;

    private GameManager gameManager; // ✅ Thêm biến GameManager

    void Start()
    {
        // Lấy tham chiếu GameManager
        gameManager = FindAnyObjectByType<GameManager>();

        // Tìm player
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();

        // Tính damage có crit
        damage = player.CalculateDamage(player.baseDamage);

        // Hồi máu nếu có hút máu
        if (player.lifeSteal > 0)
        {
            float healAmount = damage * player.lifeSteal;
            player.Heal(healAmount);
            Debug.Log($"🩸 Hút máu: {healAmount}");
        }

        Destroy(gameObject, timeDestroy);
    }

    void Update()
    {
        MoveBullet();
    }

    void MoveBullet()
    {
        transform.Translate(Vector2.right * moveBullet * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
           if (enemy != null)
        {
        enemy.TakeDamage(damage);

    // ✅ Cộng Overdrive khi bắn trúng
            if (gameManager != null && gameManager.overdriveSystem != null)
        {
        gameManager.overdriveSystem.OnEnemyHit();
        }   

    // Hiệu ứng máu
        GameObject blood = Instantiate(bloodPrefab, transform.position, Quaternion.identity);
        Destroy(blood, 1f);
        }

            Destroy(gameObject);
        }

        if (collision.CompareTag("Wall"))
        {
            Debug.Log("💥 Hit Wall");
            Destroy(gameObject);
        }
    }
    
}
