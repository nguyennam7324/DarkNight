using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float movetoPlayer = 2f;
    protected Player player;
   
    
   
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
  
   

}
