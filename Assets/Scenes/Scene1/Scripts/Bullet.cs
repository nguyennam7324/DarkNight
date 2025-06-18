using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float moveBullet = 25f;
    [SerializeField] private float timeDestroy = 0.15f;
    [SerializeField] private float damage = 10f;
    void Start()
    {
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
               
            }
            Destroy(gameObject);
        }
    }
}
