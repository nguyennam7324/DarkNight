using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float moveBullet = 25f;
    [SerializeField] private float timeDestroy = 0.15f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private GameObject bloodPrefab;

    void Start()
    {
        Player player = GameObject.FindWithTag("Player")?.GetComponent<Player>();

        if (player != null)
        {
            // Tính damage có crit
            damage = player.CalculateDamage(player.baseDamage);

            // Hồi máu nếu có hút máu
            if (player.lifeSteal > 0)
            {
                float healAmount = damage * player.lifeSteal;
                player.Heal(healAmount);
                Debug.Log($"🩸 Hút máu: {healAmount}");
            }
        }
        else
        {
            Debug.LogWarning("⚠️ Không tìm thấy Player trong scene để tính damage cho bullet.");
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
    public void SetDamage(float damage)
    {
        this.damage = damage;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
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
