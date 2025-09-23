using UnityEngine;

public class BossEnemy : Enemy
{
    [SerializeField] private Transform firePos;
    [SerializeField] private float speedDanThuong = 20f;
    [SerializeField] private GameObject enemyBullet;
    [SerializeField] private float speedDanVongTron = 20f;
    [SerializeField] private float healValue = 30f;
    [SerializeField] private GameObject miniPrefabs;
    [SerializeField] private float skillCoolDown = 2f;

    private float nextSkill;

    protected override void Update()
    {
        base.Update();
        if (Time.time > nextSkill)
        {
            NextSkill();
        }
    }

    protected override void Die()
    {
        base.Die();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (player != null)
            {
                player.TakeDamage(enterDamage);
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

    private void BanDanThuong()
    {
        if (player == null) return;

        Vector3 directionToPlayer = player.transform.position - transform.position;
        directionToPlayer.Normalize();
        GameObject Bullet = Instantiate(enemyBullet, firePos.position, Quaternion.identity);
        EnemyBullet EnemyBullet = Bullet.GetComponent<EnemyBullet>();
        if (EnemyBullet != null)
        {
            EnemyBullet.SetMovementDirection(directionToPlayer * speedDanThuong);
        }
    }

    private void BanDanVongTron()
    {
        const int bulletCount = 12;
        float angleStep = 360f / bulletCount;
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep;
            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
            direction.Normalize();
            GameObject bullet = Instantiate(enemyBullet, firePos.position, Quaternion.identity);
            EnemyBullet EnemyBullet = bullet.GetComponent<EnemyBullet>();
            if (EnemyBullet != null)
            {
                EnemyBullet.SetMovementDirection(direction * speedDanVongTron);
            }
        }
    }

    private void HoiMau(float heal)
    {
        currentHP = Mathf.Min(currentHP + heal, maxHP);
        UpdateHpBar();
    }

    private void DichChuyen()
    {
        if (player != null)
        {
            transform.position = player.transform.position;
        }
    }

    private void MiniEnemy()
    {
        Instantiate(miniPrefabs, transform.position, Quaternion.identity);
    }

    private void SmartRandomSkill()
    {
        float hpPercentage = currentHP / maxHP;

        if (hpPercentage < 0.25f) // HP dưới 25% - Boss tuyệt vọng, ưu tiên sống sót
        {
            // Tỷ lệ: 15% tấn công thường, 15% tấn công vòng tròn, 40% hồi máu, 25% dịch chuyển, 5% mini
            int randomValue = Random.Range(0, 100);

            if (randomValue < 15) // 15%
            {
                BanDanThuong();
                Debug.Log("Boss tuyệt vọng: Bắn đạn thường");
            }
            else if (randomValue < 30) // 15%
            {
                BanDanVongTron();
                Debug.Log("Boss tuyệt vọng: Bắn đạn vòng tròn");
            }
            else if (randomValue < 70) // 40% - TĂNG TỶ LỆ HỒI MÁU
            {
                HoiMau(healValue);
                Debug.Log("Boss tuyệt vọng: Hồi máu!");
            }
            else if (randomValue < 95) // 25% - TĂNG TỶ LỆ DỊCH CHUYỂN
            {
                DichChuyen();
                Debug.Log("Boss tuyệt vọng: Dịch chuyển trốn tránh!");
            }
            else // 5%
            {
                MiniEnemy();
                Debug.Log("Boss tuyệt vọng: Triệu hồi mini");
            }
        }
        else if (hpPercentage < 0.5f) // HP 25-50% - Boss căng thẳng, cân bằng sinh tồn và tấn công
        {
            // Tỷ lệ: 25% tấn công thường, 25% tấn công vòng tròn, 25% hồi máu, 15% dịch chuyển, 10% mini
            int randomValue = Random.Range(0, 100);

            if (randomValue < 25)
            {
                BanDanThuong();
            }
            else if (randomValue < 50)
            {
                BanDanVongTron();
            }
            else if (randomValue < 75)
            {
                HoiMau(healValue);
            }
            else if (randomValue < 90)
            {
                DichChuyen();
            }
            else
            {
                MiniEnemy();
            }
        }
        else // HP > 50% - Boss tự tin, tấn công chủ yếu
        {
            // Tỷ lệ bình thường: mỗi skill 20%
            int randomSkill = Random.Range(0, 5);
            switch (randomSkill)
            {
                case 0:
                    BanDanThuong();
                    break;
                case 1:
                    BanDanVongTron();
                    break;
                case 2:
                    HoiMau(healValue);
                    break;
                case 3:
                    DichChuyen();
                    break;
                case 4:
                    MiniEnemy();
                    break;
            }
        }
    }

    private void NextSkill()
    {
        // Boss nguy hiểm hơn theo HP - spam skill nhanh hơn khi HP thấp
        float dynamicCooldown = skillCoolDown;

        if (maxHP > 0)
        {
            float hpPercentage = currentHP / maxHP;

            if (hpPercentage < 0.25f) // HP dưới 25% - cực kỳ nguy hiểm
            {
                dynamicCooldown = skillCoolDown * 0.3f; // Spam gấp 3 lần
                Debug.Log("Boss cực kỳ nguy hiểm! HP: " + (hpPercentage * 100f).ToString("F1") + "%");
            }
            else if (hpPercentage < 0.5f) // HP dưới 50% - rất nguy hiểm
            {
                dynamicCooldown = skillCoolDown * 0.5f; // Spam gấp đôi
                Debug.Log("Boss rất nguy hiểm! HP: " + (hpPercentage * 100f).ToString("F1") + "%");
            }
            else if (hpPercentage < 0.75f) // HP dưới 75% - nguy hiểm
            {
                dynamicCooldown = skillCoolDown * 0.7f; // Spam nhanh hơn 1.4 lần
                Debug.Log("Boss nguy hiểm! HP: " + (hpPercentage * 100f).ToString("F1") + "%");
            }
        }

        nextSkill = Time.time + dynamicCooldown;
        SmartRandomSkill(); // Dùng SmartRandomSkill thay vì RandomSkill
    }
}