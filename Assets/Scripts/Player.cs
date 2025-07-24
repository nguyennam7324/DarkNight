using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    [SerializeField] public float maxHp = 100f;
    [SerializeField] private Image hpBar;
    private float currentHP;

    public float dashBoost;
    public float dashTime;
    private float _dashTime;
    bool isDashing = false;
    public GameObject ghostEffect;
    public float ghostDelaySeconds;
    private Coroutine dashEffectCoroutine;

    public GameManager gameManager;

    // ⭐ THÊM MỚI:
    public float expMultiplier = 1f; // Tăng EXP
    public float critChance = 0f;    // Tỉ lệ chí mạng

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

    void Update()
    {
        MovePlayer();
        Dash();
    }

    void MovePlayer()
    {
        Vector2 playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.linearVelocity = playerInput.normalized * moveSpeed;

        spriteRenderer.flipX = playerInput.x < 0;

        animator.SetBool("isRun", playerInput != Vector2.zero);
    }

    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Q) && _dashTime <= 0 && !isDashing)
        {
            moveSpeed += dashBoost;
            _dashTime = dashTime;
            isDashing = true;
            StartDashEffect();
        }

        if (_dashTime <= 0 && isDashing)
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

    void StartDashEffect()
    {
        if (dashEffectCoroutine != null) StopCoroutine(dashEffectCoroutine);
        dashEffectCoroutine = StartCoroutine(DashEffectCoroutine());
    }

    void StopDashEffect()
    {
        if (dashEffectCoroutine != null) StopCoroutine(dashEffectCoroutine);
    }

    IEnumerator DashEffectCoroutine()
    {
        while (true)
        {
            GameObject ghost = Instantiate(ghostEffect, transform.position, transform.rotation);
            ghost.GetComponent<SpriteRenderer>().sprite = spriteRenderer.sprite;
            Destroy(ghost, 0.5f);
            yield return new WaitForSeconds(ghostDelaySeconds);
        }
    }

    public void ApplySpeedBoost(float multiplier, float duration)
    {
        StopCoroutine("SpeedBoostCoroutine");
        StartCoroutine(SpeedBoostCoroutine(multiplier, duration));
    }

    private IEnumerator SpeedBoostCoroutine(float multiplier, float duration)
    {
        float originalSpeed = moveSpeed;
        moveSpeed *= multiplier;
        yield return new WaitForSeconds(duration);
        moveSpeed = originalSpeed;
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0);
        UpdateHpBar();
        if (currentHP <= 0)
        {
            gameManager.GameOver();
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public void Heal(float amount)
    {
        if (currentHP >= maxHp) return;
        currentHP += amount;
        currentHP = Mathf.Min(currentHP, maxHp);
        UpdateHpBar();
    }

    private void UpdateHpBar()
    {
        hpBar.fillAmount = currentHP / maxHp;
    }

    // ⭐ Ví dụ dùng crit (nếu Sensei muốn dùng thử)
    public float CalculateDamage(float baseDamage)
    {
        if (Random.value < critChance)
        {
            Debug.Log("Chí mạng!");
            return baseDamage * 2f;
        }
        return baseDamage;
    }
}