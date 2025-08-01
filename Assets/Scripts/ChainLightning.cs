using UnityEngine;

public class ChainLightning : MonoBehaviour
{
    public LayerMask enemyLayer;
    public float damage;
    public GameObject chainLightningEffect;
    public GameObject beenStruck;
    public int amountToChain = 3; // Số lần lan

    private void Start()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 3f, enemyLayer);
        GameObject currentTarget = null;

        float closestDistance = float.MaxValue;
        foreach (var enemy in hitEnemies)
        {
            if (!enemy.GetComponentInChildren<EnemyStruck>())
            {
                float dist = Vector2.Distance(transform.position, enemy.transform.position);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    currentTarget = enemy.gameObject;
                }
            }
        }

        if (currentTarget != null)
        {
            ChainToEnemy(currentTarget, amountToChain);
        }

        Destroy(gameObject, 1f); // Auto destroy sau khi cast
    }

    void ChainToEnemy(GameObject target, int chainsLeft)
    {
        if (target == null || chainsLeft <= 0) return;

        Instantiate(chainLightningEffect, target.transform.position, Quaternion.identity);
        Instantiate(beenStruck, target.transform);

        Enemy enemyScript = target.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.TakeDamage(damage);
        }

        // Đánh dấu là đã bị sét đánh
        if (target.GetComponentInChildren<EnemyStruck>() == null)
            target.AddComponent<EnemyStruck>();

        // Tìm enemy gần nhất kế tiếp chưa bị đánh
        Collider2D[] nearby = Physics2D.OverlapCircleAll(target.transform.position, 3f, enemyLayer);
        GameObject nextTarget = null;
        float closest = float.MaxValue;
        foreach (var enemy in nearby)
        {
            if (enemy.gameObject != target && !enemy.GetComponentInChildren<EnemyStruck>())
            {
                float dist = Vector2.Distance(target.transform.position, enemy.transform.position);
                if (dist < closest)
                {
                    closest = dist;
                    nextTarget = enemy.gameObject;
                }
            }
        }

        if (nextTarget != null)
        {
            // Delay nhỏ giữa mỗi lần lan (tuỳ chọn)
            Invoke(nameof(DelayChain), 0.1f);
            ChainToEnemy(nextTarget, chainsLeft - 1);
        }
    }

    void DelayChain() { } // để gọi Invoke() không bị lỗi
}