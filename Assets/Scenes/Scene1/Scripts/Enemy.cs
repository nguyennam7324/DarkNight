using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float movetoPlayer = 2f;
    protected Player player;
    [SerializeField] protected float maxHP = 50f;
    protected float currentHP;


    protected virtual void Start()
    {
        player = FindAnyObjectByType<Player>();
        
        
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
        Destroy(gameObject);
    }
    public void TakeDamage(float damage)
    {
        Debug.Log("Máu Enemy hiện tại:" + currentHP);
        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0);
        Debug.Log("Máu Enemy hiện tại:" + currentHP);

        
        if (currentHP <= 0)
        {
            Die();
        }
    }



}
