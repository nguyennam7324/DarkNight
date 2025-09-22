using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MiniBoss : MonoBehaviour
{
    [Header("AI Settings")]
    public float moveSpeed = 3f;
    public float attackRange = 2f;
    public float detectionRange = 10f;
    public float skillCooldown = 5f;
    public float summonCooldown = 10f;
    public float attackRate = 1f;

    [Header("Health Settings")]
    public float maxHealth = 100;
    public float currentHealth;
    public Image healthBar;

    [Header("Combat Settings")]
    public int normalDamage = 10;
    public int skillDamage = 25;
    public float damageCooldown = 1f; // Thời gian chờ giữa các lần gây damage

    [Header("Animation Names")]
    public string idleAnimation = "idle2";
    public string shellAnimation = "shell";
    public string attackAnimation = "attack";
    public string skillAnimation = "skill1";
    public string summonAnimation = "summon";
    public string deathAnimation = "death";
    public string hitAnimation = "hit";

    private Animator animator;
    private Transform player;
    private bool isDead = false;
    private bool isAttacking = false;
    private bool isTakingDamage = false;
    private Rigidbody2D rb;

    // Cooldown tracking
    private float lastSkillTime = -10f;
    private float lastSummonTime = -10f;
    private float lastAttackTime = 0f;
    private float lastDamageTime = 0f;

    // State machine
    private enum BossState { Idle, Chase, Attack, Skill, Summon, Dead, Hit }
    private BossState currentState = BossState.Idle;

    // Animation tracking
    private string currentAnimation = "";

    // Damage trigger
    private bool isPlayerInDamageZone = false;
    private Collider2D damageTrigger;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Khởi tạo máu
        currentHealth = maxHealth;

        // Tìm hoặc tạo health bar
        InitializeHealthBar();

        // Tìm damage trigger
        FindDamageTrigger();

        if (animator == null) Debug.LogError("Animator not found!");
        if (player == null) Debug.LogError("Player not found!");
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);


        // Kiểm tra điều kiện gây damage liên tục
        CheckContinuousDamage();

        // State machine logic
        switch (currentState)
        {
            case BossState.Idle:
                HandleIdleState(distanceToPlayer);
                break;
            case BossState.Chase:
                HandleChaseState(distanceToPlayer);
                break;
            case BossState.Attack:
                HandleAttackState(distanceToPlayer);
                break;
            case BossState.Skill:
                if (!isAttacking) currentState = BossState.Attack;
                break;
            case BossState.Summon:
                if (!isAttacking) currentState = BossState.Chase;
                break;
            case BossState.Hit:
                if (!isTakingDamage) currentState = BossState.Chase;
                break;
        }

        UpdateAnimator();
    }

    private void InitializeHealthBar()
    {

        healthBar.fillAmount = maxHealth / maxHealth;
        healthBar.fillAmount = currentHealth / maxHealth;
        healthBar.gameObject.SetActive(true);
    }




    private void FindDamageTrigger()
    {
        // Tìm collider damage trigger (có thể là trigger collider riêng)
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            if (col.isTrigger)
            {
                damageTrigger = col;
                break;
            }
        }
    }

    private void CheckContinuousDamage()
    {
        if (isPlayerInDamageZone && CanDealContinuousDamage() && IsPlayerAlignedWithBoss())
        {
            DealContinuousDamage();
        }
    }

    private bool IsPlayerAlignedWithBoss()
    {
        if (player == null) return false;

        // Kiểm tra player có nằm ngang hoặc bằng với boss không
        float heightDifference = Mathf.Abs(player.position.y - transform.position.y);
        return heightDifference <= 0.2f; // Cho phép sai số 1 unit
    }

    private bool CanDealContinuousDamage()
    {
        return Time.time >= lastDamageTime + damageCooldown && !isTakingDamage;
    }

    private void DealContinuousDamage()
    {
        lastDamageTime = Time.time;

        // Gây damage nhỏ liên tục
        Player damageable = player.GetComponent<Player>();
        if (damageable != null)
        {
            damageable.TakeDamage(normalDamage / 3); // Damage nhỏ hơn normal attack
            Debug.Log($"Boss gây {normalDamage / 3} damage liên tục cho player!");
        }
    }

    private void HandleIdleState(float distance)
    {
        if (distance <= detectionRange)
        {
            currentState = BossState.Chase;
            PlayAnimation(idleAnimation);
        }
    }

    private void HandleChaseState(float distance)
    {
        if (distance <= attackRange)
        {
            currentState = BossState.Attack;
        }
        else if (distance <= detectionRange)
        {
            // Di chuyển về phía player
            Vector2 direction = (player.position - transform.position).normalized;

            // Sử dụng Rigidbody2D để di chuyển mượt mà hơn
            if (rb != null && !isTakingDamage)
            {
                rb.linearVelocity = direction * moveSpeed;
            }
            else if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }
            else
            {
                transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
            }

            // Flip sprite theo hướng di chuyển
            if (direction.x != 0 && !isTakingDamage)
            {
                transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);
            }

            // Kiểm tra skill khả dụng
            if (CanUseSkill() && distance <= attackRange * 1.5f && !isAttacking)
            {
                StartCoroutine(SkillRoutine());
            }
            else if (CanUseSummon() && distance >= attackRange * 2f && !isAttacking)
            {
                StartCoroutine(SummonRoutine());
            }
            else if (!isTakingDamage)
            {
                PlayAnimation("Run"); // Hoặc idleAnimation nếu không có animation chạy
            }
        }
        else
        {
            currentState = BossState.Idle;
            if (rb != null) rb.linearVelocity = Vector2.zero;
        }
    }


        private void HandleAttackState(float distance)
    {
        if (distance > attackRange * 1.2f)
        {
            currentState = BossState.Chase;
            if (rb != null) rb.linearVelocity = Vector2.zero;
            return;
        }

        // KIỂM TRA NẾU CHƯA NGANG HÀNG VỚI PLAYER
        if (!IsPlayerAlignedWithBoss())
        {
            // Di chuyển đến vị trí ngang hàng với player
            Vector2 alignmentDirection = GetAlignmentDirection();

            if (rb != null && !isTakingDamage)
            {
                rb.linearVelocity = alignmentDirection * moveSpeed;
            }

            // Flip sprite theo hướng di chuyển
            if (alignmentDirection.x != 0 && !isTakingDamage)
            {
                transform.localScale = new Vector3(Mathf.Sign(alignmentDirection.x), 1, 1);
            }

            PlayAnimation("Run");
            return; // Thoát khỏi hàm, không tấn công khi chưa ngang hàng
        }

        // Dừng di chuyển khi tấn công (chỉ khi đã ngang hàng)
        if (rb != null) rb.linearVelocity = Vector2.zero;

        // Quay mặt về player
        Vector2 direction = (player.position - transform.position).normalized;
        if (direction.x != 0 && !isTakingDamage)
        {
            transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);
        }

        // Thực hiện tấn công (chỉ khi đã ngang hàng với player)
        if (Time.time >= lastAttackTime + attackRate && !isAttacking && !isTakingDamage)
        {
            StartCoroutine(AttackRoutine());
        }

        // Kiểm tra skill khả dụng (chỉ khi đã ngang hàng với player)
        if (CanUseSkill() && !isAttacking && !isTakingDamage)
        {
            StartCoroutine(SkillRoutine());
        }
        else if (CanUseSummon() && distance >= attackRange * 1.2f && !isAttacking && !isTakingDamage)
        {
            StartCoroutine(SummonRoutine());
        }
    
    }
    private Vector2 GetAlignmentDirection()
    {
        if (player == null) return Vector2.zero;

        // Tính hướng di chuyển để đạt được cùng chiều cao với player
        float targetY = player.position.y;
        float currentY = transform.position.y;

        // Ưu tiên di chuyển theo chiều dọc để căn chỉnh trước
        if (Mathf.Abs(targetY - currentY) > 0.2f)
        {
            return new Vector2(0, Mathf.Sign(targetY - currentY));
        }

        // Khi đã gần ngang hàng, có thể kết hợp di chuyển ngang
        Vector2 toPlayer = (player.position - transform.position).normalized;
        return toPlayer;
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        // Chọn loại tấn công ngẫu nhiên (normal attack hoặc shell)
        string attackAnim;
        float attackDelay;

        if (Random.Range(0, 100) < 30) // 30% chance for shell attack
        {
            attackAnim = shellAnimation;
            attackDelay = 0.1f;
        }
        else
        {
            attackAnim = attackAnimation;
            attackDelay = 0.1f;
        }

        // Play animation
        PlayAnimation(attackAnim);

        // Chờ đến thời điểm gây damage trong animation
        yield return new WaitForSeconds(attackDelay);

        // Gây sát thương (chỉ khi player aligned)
        if (IsPlayerAlignedWithBoss())
        {
            DealDamageToPlayer(normalDamage);
        }

        // Chờ animation hoàn thành
        yield return new WaitForSeconds(GetAnimationLength(attackAnim) - attackDelay);

        isAttacking = false;
    }

    private IEnumerator SkillRoutine()
    {
        isAttacking = true;
        currentState = BossState.Skill;
        lastSkillTime = Time.time;

        PlayAnimation(skillAnimation);

        yield return new WaitForSeconds(0.8f); // Thời gian trước khi gây damage

        // Gây sát thương (chỉ khi player aligned)
        if (IsPlayerAlignedWithBoss())
        {
            DealDamageToPlayer(skillDamage);
        }

        yield return new WaitForSeconds(GetAnimationLength(skillAnimation) - 0.8f);

        isAttacking = false;
    }

    private IEnumerator SummonRoutine()
    {
        isAttacking = true;
        currentState = BossState.Summon;
        lastSummonTime = Time.time;

        PlayAnimation(summonAnimation);

        yield return new WaitForSeconds(1f); // Thời gian trước khi triệu hồi

        SummonMinions();

        yield return new WaitForSeconds(GetAnimationLength(summonAnimation) - 1f);

        isAttacking = false;
    }

    private void DealDamageToPlayer(int damage)
    {
        if (player == null) return;

        Player damageable = player.GetComponent<Player>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
            Debug.Log($"MiniBoss gây {damage} sát thương cho player!");
        }
    }

    private void SummonMinions()
    {
        Debug.Log("MiniBoss triệu hồi quân!");
        // Implement logic triệu hồi quân ở đây
    }

    private bool CanUseSkill()
    {
        return Time.time >= lastSkillTime + skillCooldown;
    }

    private bool CanUseSummon()
    {
        return Time.time >= lastSummonTime + summonCooldown;
    }

    private void UpdateAnimator()
    {
        if (isTakingDamage) return;

        // Nếu không có animation nào đang chạy và không tấn công, chơi idle
        if (!isAttacking && string.IsNullOrEmpty(currentAnimation))
        {
            PlayAnimation(idleAnimation);
        }
    }

    private void PlayAnimation(string animationName)
    {
        if (animator == null || currentAnimation == animationName || isTakingDamage) return;

        if (!AnimationExists(animationName))
        {
            Debug.LogWarning($"Animation '{animationName}' không tồn tại!");
            return;
        }

        animator.Play(animationName);
        currentAnimation = animationName;
    }

    private bool AnimationExists(string animationName)
    {
        if (animator == null) return false;

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == animationName) return true;
        }
        return false;
    }

    private float GetAnimationLength(string animationName)
    {
        if (animator == null) return 1f;

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == animationName)
                return clip.length;
        }
        return 1f;
    }

    // Interface implementation
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        UpdateHealthBar();

        Debug.Log($"MiniBoss nhận {damage} sát thương! Máu hiện tại: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            PlayDeath();
        }
        else
        {
            StartCoroutine(TakeDamageRoutine());
        }
    }

    private IEnumerator TakeDamageRoutine()
    {
        isTakingDamage = true;
        currentState = BossState.Hit;

        // Dừng di chuyển
        if (rb != null) rb.linearVelocity = Vector2.zero;

        // Play hit animation
        if (!string.IsNullOrEmpty(hitAnimation) && AnimationExists(hitAnimation))
        {
            PlayAnimation(hitAnimation);
            yield return new WaitForSeconds(GetAnimationLength(hitAnimation));
        }
        else
        {
            // Hiệu ứng nhấp nháy nếu không có animation hit
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                Color originalColor = spriteRenderer.color;
                spriteRenderer.color = Color.red;
                yield return new WaitForSeconds(0.2f);
                spriteRenderer.color = originalColor;
            }
            else
            {
                yield return new WaitForSeconds(0.2f);
            }
        }

        isTakingDamage = false;
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth/maxHealth;
        }
    }

    public void PlayDeath()
    {
        if (!isDead)
        {
            isDead = true;
            currentState = BossState.Dead;

            PlayAnimation(deathAnimation);

            // Vô hiệu hóa AI và physics
            if (rb != null) rb.linearVelocity = Vector2.zero;
            GetComponent<Collider2D>().enabled = false;
            enabled = false;

            // Ẩn health bar
            if (healthBar != null) healthBar.gameObject.SetActive(false);

            StartCoroutine(OnDeathComplete());
        }
    }

    private IEnumerator OnDeathComplete()
    {
        yield return new WaitForSeconds(GetAnimationLength(deathAnimation));
        gameObject.SetActive(false);
    }

    // Trigger events for damage zone
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInDamageZone = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInDamageZone = false;
        }
    }

    // Gọi từ animation event
    public void OnAttackEvent()
    {
        if (IsPlayerAlignedWithBoss())
        {
            DealDamageToPlayer(normalDamage);
        }
    }

    public void OnSkillEvent()
    {
        if (IsPlayerAlignedWithBoss())
        {
            DealDamageToPlayer(skillDamage);
        }
    }

    // Debug visualization
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Vẽ damage zone alignment
        Gizmos.color = Color.cyan;
        Vector3 leftPoint = transform.position + new Vector3(-attackRange, 1f, 0);
        Vector3 rightPoint = transform.position + new Vector3(attackRange, 1f, 0);
        Gizmos.DrawLine(leftPoint, rightPoint);
        Gizmos.DrawLine(transform.position + new Vector3(-attackRange, -1f, 0),
                        transform.position + new Vector3(attackRange, -1f, 0));
    }
}