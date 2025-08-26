//using UnityEngine;

//public class EnemyBullet : MonoBehaviour
//{
//    private Vector3 movementDirection;
//    void Start()
//    {
//        Destroy(gameObject, 5f);
//    }


//    void Update()
//    {
//        if(movementDirection == Vector3.zero) return;
//        transform.position += movementDirection * Time.deltaTime;
//    }
//    public void SetMovementDirection(Vector3 direction)
//    {
//        movementDirection = direction;
//    }
//}

using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Vector3 movementDirection;
    [SerializeField] private float damage = 15f; // Sát thương của đạn enemy

    void Start()
    {
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        if (movementDirection == Vector3.zero) return;
        transform.position += movementDirection * Time.deltaTime;
    }

    public void SetMovementDirection(Vector3 direction)
    {
        movementDirection = direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Debug.Log("Enemy bullet hit player for " + damage + " damage!");
            }

            Destroy(gameObject); // Hủy đạn sau khi trúng player
        }

        // Hủy đạn khi trúng tường
        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
