using UnityEngine;

public class Bat_Enemy : Enemy
{
    [Header("Bat Settings")]
    [SerializeField] private GameObject bulletPrefabs;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float circularBulletSpeed = 10f;
    [SerializeField] private float skillCooldown = 3f;
    [SerializeField] private float attackRange = 8f;
    [SerializeField] private float flySpeed = 3f;

    private float nextSkillTime = 0f;
    private bool isAttacking = false;
    private float distanceToPlayer;

    protected override void Start()
    {
        base.Start();
        movetoPlayer = flySpeed;
    }

    protected override void Update()
    {
        base.Update();

        if (player != null)
        {
            distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            // Set animator parameters
            animator.SetBool("isFlying", distanceToPlayer > 1f && !isAttacking);

            // Check if can attack
            if (Time.time >= nextSkillTime && distanceToPlayer <= attackRange && !isAttacking)
            {
                PerformCircularAttack();
            }
        }
    }

    private void PerformCircularAttack()
    {
        isAttacking = true;
        nextSkillTime = Time.time + skillCooldown;

        // Dùng animation fly làm attack (bat vẫn vỗ cánh khi bắn)
        animator.SetBool("isFlying", true);

        // Bắn đạn ngay
        FireCircularBullets();
        Invoke(nameof(EndAttack), 1f);
    }

    private void FireCircularBullets()
    {
        const int bulletCount = 10; // 8 viên đạn cho bat (ít hơn boss)
        float angleStep = 360f / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep;
            Vector3 bulletDirection = new Vector3(
                Mathf.Cos(Mathf.Deg2Rad * angle),
                Mathf.Sin(Mathf.Deg2Rad * angle),
                0
            );

            Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
            GameObject bullet = Instantiate(bulletPrefabs, spawnPos, Quaternion.identity);

            EnemyBullet enemyBullet = bullet.GetComponent<EnemyBullet>();
            if (enemyBullet == null)
            {
                enemyBullet = bullet.AddComponent<EnemyBullet>();
            }

            enemyBullet.SetMovementDirection(bulletDirection * circularBulletSpeed);
        }
    }

    private void EndAttack()
    {
        isAttacking = false;
    }

    protected override void MoveEnemy()
    {
        // Chỉ di chuyển khi không đang tấn công
        if (!isAttacking && player != null)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.transform.position,
                movetoPlayer * Time.deltaTime
            );
            FlipEnemy();
        }
    }

    public override void TakeDamage(float damage)
    {
        // Trigger hit animation
        if (animator != null && currentHP > 0)
        {
            animator.SetTrigger("Hit");
        }

        base.TakeDamage(damage);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (player != null)
            {
                player.TakeDamage(enterDamage);
                // Bỏ animation attack vì không có
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (player != null)
            {
                player.TakeDamage(stayDamage);
            }
        }
    }
}