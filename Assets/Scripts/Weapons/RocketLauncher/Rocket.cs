using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Rocket : MonoBehaviour
{
    [Header("Rocket flight")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 6f;

    [Header("Collision")]
    [SerializeField] private LayerMask collisionMask; // set trong Inspector: Enemy + Walls (KHÔNG include Player)
    [SerializeField] private bool useTrigger = true;  // chọn OnTrigger hoặc OnCollision

    // explosion data (set từ launcher)
    private GameObject explosionPrefab;
    private float explosionRadius;
    private float explosionDamage;

    // optional owner (nếu bạn muốn truyền Player vào)
    private Player ownerPlayer;

    private Rigidbody2D rb;

    // === Public API ===
    // existing 3-arg method (keeps backward compatibility)
    public void SetExplosion(GameObject explosionPrefab, float radius, float damage)
    {
        SetExplosion(explosionPrefab, radius, damage, null);
    }

    // overload 4-arg: nhận thêm owner Player (không bắt buộc dùng)
    public void SetExplosion(GameObject explosionPrefab, float radius, float damage, Player owner)
    {
        this.explosionPrefab = explosionPrefab;
        this.explosionRadius = radius;
        this.explosionDamage = damage;
        this.ownerPlayer = owner;
    }

    // (alternative name used in some versions)
    public void Init(GameObject explosionPrefab, float radius, float damage)
    {
        SetExplosion(explosionPrefab, radius, damage, null);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // give initial velocity along local right
        if (rb != null)
        {
            rb.linearVelocity = transform.right * speed;
            rb.gravityScale = 0f;
        }

        // safety destroy after lifeTime
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!useTrigger) return;

        if (IsInMask(other.gameObject.layer))
        {
            Explode();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (useTrigger) return;

        if (IsInMask(collision.gameObject.layer))
        {
            Explode();
        }
    }

    private bool IsInMask(int layer)
    {
        return ((1 << layer) & collisionMask) != 0;
    }

    private void Explode()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        // AoE damage
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var h in hits)
        {
            Enemy e = h.GetComponent<Enemy>();
            if (e != null)
            {
                e.TakeDamage(explosionDamage);
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
