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
}
