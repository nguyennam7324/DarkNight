using UnityEngine;

public class PetFollow : MonoBehaviour
{
    public Transform target;
    public float followSpeed = 5f;
    public float followDistance = 1.5f;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (target == null) return;

        float distance = Vector2.Distance(transform.position, target.position);

        if (distance > followDistance)
        {
            // Di chuyển theo player
            transform.position = Vector2.MoveTowards(
                transform.position,
                target.position,
                followSpeed * Time.deltaTime
            );

            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }
}
