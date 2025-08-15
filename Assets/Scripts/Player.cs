using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
public float MoveSpeed
{
    get => moveSpeed;
    set => moveSpeed = value;
}
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    [SerializeField] public float maxHp = 100f;
    [SerializeField] private Image hpBar;
    public float currentHP = 100f;

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
    public float baseDamage = 10f;

    // ⭐ THÊM KHIÊN
    public float shield = 0f;
    public float maxShield = 50f;
    public float shieldRegenDelay = 10f;
    private float lastDamageTime = -999f;

    // ⭐ THÊM HÚT MÁU
    public float lifeSteal = 0f; // VD: 0.1 = 10% sát thương hút máu

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        currentHP = maxHp;
        shield = maxShield;
        UpdateHpBar();
    }

    void Update()
    {
        MovePlayer();
        Dash();
        RegenerateShield(); // ⭐ HỒI KHIÊN
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
        lastDamageTime = Time.time; // ⭐ RESET HỒI KHIÊN

        if (shield > 0)
        {
            float shieldAbsorb = Mathf.Min(shield, damage);
            shield -= shieldAbsorb;
            damage -= shieldAbsorb;
        }

        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0);
        UpdateHpBar();

        if (currentHP <= 0)
        {
            gameManager.GameOver();
            Die();
        }
    }

    private void RegenerateShield()
    {
        if (Time.time - lastDamageTime >= shieldRegenDelay && shield < maxShield)
        {
            shield = Mathf.MoveTowards(shield, maxShield, 10f * Time.deltaTime); // Tăng 10 mỗi giây
            UpdateHpBar(); // Có thể dùng thêm 1 thanh shield UI riêng
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

    // ⭐ GÂY SÁT THƯƠNG
// Chỉ trích đoạn CalculateDamage được sửa lại cho rõ ràng
        public float CalculateDamage(float baseDamage)
{
    float finalDamage = baseDamage;

    if (Random.value < critChance)
    {
        Debug.Log("⚡ Chí mạng!");
        finalDamage *= 2f;
    }

    // ❌ Không hút máu ở đây nữa
    return finalDamage;
}

}
