using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float moveSpeed = 3f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    [SerializeField] private float maxHp = 100f;
    [SerializeField] private Image hpBar;
    private float currentHP;
    public float dashBoost;
    public float dashTime;
    private float _dashTime;
    bool isDashing = false;
    public GameObject ghostEffect;
    public float ghostDelaySeconds;
    private Coroutine dashEffectCoroutine;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        currentHP = maxHp;
        UpdateHpBar();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        Dash();
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

    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Q) && _dashTime <= 0 && isDashing == false)
        {
            moveSpeed += dashBoost;
            _dashTime = dashTime;
            isDashing = true;
            StartDashEffect();

        }
        if (_dashTime <= 0 && isDashing == true)
        {
            moveSpeed -= dashBoost;
            isDashing = false;
            StopDashEffect();
        }
        else
        {
            _dashTime -= Time.deltaTime;
        }
    }
    void StopDashEffect()
    {
        if (dashEffectCoroutine != null) StopCoroutine(dashEffectCoroutine);

    }
    void StartDashEffect()
    {
        if (dashEffectCoroutine != null) StopCoroutine(dashEffectCoroutine);
        dashEffectCoroutine = StartCoroutine(DashEffectCoroutine());
    }
    IEnumerator DashEffectCoroutine()
    {
        while (true)
        {
            GameObject ghost = Instantiate(ghostEffect, transform.position, transform.rotation);
            Sprite currentSprite=GetComponent<SpriteRenderer>().sprite;
            ghost.GetComponent<SpriteRenderer>().sprite = currentSprite;
            Destroy(ghost,0.5f);
            yield return new WaitForSeconds(ghostDelaySeconds);
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
        UpdateHpBar();
        if (currentHP <= 0)
        {
            Die();
        }
    }
    private void UpdateHpBar()
    {
        hpBar.fillAmount = currentHP / maxHp;
    }
}
