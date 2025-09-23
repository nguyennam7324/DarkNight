using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BossEnemy : Enemy
{
    [SerializeField] private GameObject bulletPrefabs;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float speedDanThuong = 20f;
    [SerializeField] private float speedDanVongTron = 10f;
    [SerializeField] private float hpValue = 100f;
    [SerializeField] private GameObject miniEnemy;
    [SerializeField] private float skillCooldown = 0.5f;
    private float nextSkillTime = 0f;
    private List<SkillProportion> skillProps;

    private void Start()
    {
        InitPropSkill();
    }

    private void InitPropSkill()
    {
        skillProps = new List<SkillProportion>();
        skillProps.Add(new SkillProportion() { id = 0 , propValue = 0.2f });
        skillProps.Add(new SkillProportion() { id = 1 , propValue = 0.2f });
        skillProps.Add(new SkillProportion() { id = 2 , propValue = 0.1f });
        skillProps.Add(new SkillProportion() { id = 3 , propValue = 0.2f });
        skillProps.Add(new SkillProportion() { id = 4 , propValue = 0.3f });
    }

    protected override void Update()
    {

        if (Time.time >= nextSkillTime)
        {
            SuDungSkill();
            nextSkillTime = Time.time + 2f;

        }
        Debug.Log($"Update running - Time: {Time.time}");

        if (player != null)
        {
            Debug.Log($"Moving towards player: {player.transform.position}");
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.transform.position,
                movetoPlayer * Time.deltaTime
            );
        }
        
    }

    protected override void MoveEnemy()
    {
        if (player != null)
        {
            Vector2 newPos = Vector2.MoveTowards(transform.position, player.transform.position, movetoPlayer * Time.deltaTime);
            transform.position = newPos;
            FlipEnemy();
            Debug.Log($"Moving to player. Position: {transform.position}");
        }
        else
        {
            Debug.LogWarning("Player is NULL!");
        }
    }
        private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (player != null)
            {
                player.TakeDamage(enterDamage);
                if (animator != null)
                {
                    animator.SetTrigger("Attack");
                }
            }
        }
    }
    public int GetRandomSkill()
    {
        // Tính tổng tỷ lệ
        float totalProbability = 0f;
        foreach (var item in skillProps)
        {
            totalProbability += item.propValue;
        }

        // Random giá trị
        float randomPoint = Random.Range(0f, totalProbability);

        // Chọn item dựa trên tỷ lệ
        float currentProbability = 0f;
        foreach (var item in skillProps)
        {
            currentProbability += item.propValue;
            if (randomPoint <= currentProbability)
            {
                return item.id;
            }
        }

        return skillProps[skillProps.Count - 1].id; // Fallback
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (player != null)
            {
                player.TakeDamage(stayDamage);
                if (animator != null)
                {
                    animator.SetTrigger("Attack");
                }
            }
        }
    }
    private void BanDanThuong()
    {
        if (player != null)
        {
            Vector3 directionToPlayer=player.transform.position-firePoint.position;
            directionToPlayer.Normalize();
            GameObject bullet=Instantiate(bulletPrefabs,firePoint.position,Quaternion.identity);
            EnemyBullet enemyBullet=bullet.AddComponent<EnemyBullet>();
            enemyBullet.SetMovementDirection(directionToPlayer * speedDanThuong);
        }
    }
    private void BanDanVongTron()
    {
        const int bulletCount = 12;
        float angleStep = 360f / bulletCount;
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep;
            Vector3 bulletDirection = new Vector3(Mathf.Cos(Mathf.Deg2Rad*angle),Mathf.Sin(Mathf.Deg2Rad*angle),0);
            GameObject bullet = Instantiate(bulletPrefabs, transform.position, Quaternion.identity);
            EnemyBullet enemyBullet=bullet.AddComponent<EnemyBullet>();
            enemyBullet.SetMovementDirection(bulletDirection * speedDanVongTron);
        }
    }
    private void HoiMau(float hpAmount)
    {
        currentHP = Mathf.Min(currentHP + hpAmount, maxHP);
        UpdateHpBar();
    }
    private void SinhMiniEnemy()
    {
        Instantiate(miniEnemy, transform.position, Quaternion.identity);
    }
    private void DichChuyen()
    {
        if(player != null)
        {
            transform.position=player.transform.position;
        }
    }
    private void ChonSkillNgauNhien()
    {
        int randomSkill = GetRandomSkill();
        switch (randomSkill)
        {
            case 0:
                BanDanThuong();
                break;
            case 1:
                BanDanVongTron();
                break;
            case 2:
                HoiMau(hpValue);
                break;
            case 3:
                SinhMiniEnemy();
                break;
            case 4:
                DichChuyen();
                break;
        }
    }
    private void SuDungSkill()
    {
   
        ChonSkillNgauNhien();
    }
}

public class SkillProportion
{
    public int id;
    public float propValue;
}
