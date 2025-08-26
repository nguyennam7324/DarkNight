using UnityEngine;

public class PetFollow : MonoBehaviour
{
    [SerializeField] private Transform target; // Player
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private float followDistance = 1.5f;

    private Animator anim;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (target == null) return;

        // Tính khoảng cách tới Player
        float distance = Vector2.Distance(transform.position, target.position);

        if (distance > followDistance)
        {
            // Di chuyển về phía Player
            transform.position = Vector2.MoveTowards(
                transform.position,
                target.position,
                followSpeed * Time.deltaTime
            );

            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }

        // Xác định hướng quay mặt theo Player
        if (target.position.x > transform.position.x)
            spriteRenderer.flipX = false; // Quay mặt sang phải
        else if (target.position.x < transform.position.x)
            spriteRenderer.flipX = true;  // Quay mặt sang trái
    }
}
