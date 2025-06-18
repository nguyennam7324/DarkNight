using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float moveSpeed = 3f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    [SerializeField] private float maxHp = 100f;
    private float currentHP;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        currentHP = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }
    void MovePlayer()
    {
        Vector2 playerInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.linearVelocity = playerInput.normalized * moveSpeed;
        if (playerInput.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
        if (playerInput != Vector2.zero)
        {
            animator.SetBool("isRun", true);
        }
        else
        {
            animator.SetBool("isRun", false);
        }

    }
    private void Die()
    {
        Destroy(gameObject);

    }
    public void TakeDamage(float damage)
    {
        Debug.Log("Máu hiện tại" + currentHP);
        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0);
        Debug.Log("Máu hiện tại" + currentHP);
        
        if (currentHP <= 0)
        {
            Die();
        }
    }
}
