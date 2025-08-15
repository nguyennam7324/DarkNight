using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float damage = 80f;
    [SerializeField] private float radius = 2f;
    [SerializeField] private float lifeTime = 0.6f;

    private void Start()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy != null) enemy.TakeDamage(damage);
            }
        }
        Destroy(gameObject, lifeTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
