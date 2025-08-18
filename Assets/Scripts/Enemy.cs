using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    
    [SerializeField] protected float movetoPlayer = 2f;
    protected Player player;
    [SerializeField] protected float maxHP = 50f;
    protected float currentHP;
    [SerializeField] protected float enterDamage = 10f;
    [SerializeField] protected float stayDamage = 1f;
    [SerializeField] protected Image hpBar;
    protected Animator animator;
    //LootTable
    [Header("Loot")]
    public List<LootItem> lootTable = new List<LootItem>(); 
    
    protected virtual void Start()
    {
        player = FindAnyObjectByType<Player>();
        currentHP = maxHP;
        UpdateHpBar();
        animator = GetComponent<Animator>();
    }
    protected virtual void Update()
    {
        MoveEnemy();
    }
    protected void MoveEnemy()
    {
        if (player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, movetoPlayer * Time.deltaTime);
            FlipEnemy();
        }
    }
    protected void FlipEnemy()
    {
        if (player != null)
        {
            transform.localScale = new Vector3(player.transform.position.x < transform.position.x ? -1 : 1, 1, 1);
        }
    }
    protected virtual void Die()
    {
        if (animator != null)
        {
            animator.Play("Death");
        }
        //spawn item
        foreach (LootItem lootItem in lootTable)
        {
            if (Random.Range(0f, 100f) <= lootItem.dropChange)
            {
                InstantiateLoot(lootItem.itemPrefab);
            }
        }
        this.enabled = false; // Dừng script
        Destroy(gameObject, 0.5f); // Huỷ sau khi animation xong
    }

    public void TakeDamage(float damage)
    {
        if (currentHP <= 0) return ; // Đã chết thì không nhận damage

        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0);
        UpdateHpBar();

        if (currentHP <= 0)
        {
            Die();
        }
    }

    protected void UpdateHpBar()
    {
        hpBar.fillAmount = currentHP / maxHP;
    }

    void InstantiateLoot(GameObject loot)
    {
        GameObject droppedLoot = Instantiate(loot, transform.position, Quaternion.identity);
    }

}
