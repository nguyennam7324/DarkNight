using UnityEngine;
using System.Collections;

public class MiniBoss : MonoBehaviour
{
    [Header("Combat Settings")]
    public float attackRange = 5f;        // Phạm vi tấn công thường
    public float skillRange = 8f;         // Phạm vi skill
    public float moveSpeed = 3f;          // Tốc độ di chuyển
    public int normalDamage = 10;         // Sát thương tấn công thường
    public int skillDamage = 30;          // Sát thương skill
    public float attackCooldown = 2f;     // Thời gian chờ giữa các lần tấn công thường
    public float skillCooldown = 5f;      // Thời gian chờ giữa các lần skill

    [Header("Skill Settings")]
    public GameObject skillEffect;        // Hiệu ứng skill (optional)
    public float skillCastTime = 1.5f;    // Thời gian cast skill

    [Header("References")]
    public Transform player;              // Tham chiếu đến nhân vật chính
    public Animator animator;             // Tham chiếu đến Animator

    private float lastAttackTime = 0f;
    private float lastSkillTime = 0f;
    private bool isAttacking = false;
    private bool isCastingSkill = false;
    private bool isInAttackRange = false;
    private bool canUseSkill = true;

    void Start()
    {
        // Tự động tìm nhân vật player nếu chưa được gán
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        // Lấy component Animator
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        lastSkillTime = Time.time;
    }

    void Update()
    {
        if (player == null) return;
        if (isCastingSkill) return; // Dừng các hành động khác khi đang cast skill

        // Tính khoảng cách đến player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Kiểm tra nếu player trong phạm vi tấn công
        isInAttackRange = distanceToPlayer <= attackRange;

        // Kiểm tra có thể dùng skill (sau mỗi 5 giây)
        canUseSkill = Time.time - lastSkillTime >= skillCooldown;

        if (isInAttackRange)
        {
            // Ưu tiên dùng skill nếu có thể
            if (canUseSkill && distanceToPlayer <= skillRange)
            {
                StartCoroutine(UseSkill());
            }
            else
            {
                // Di chuyển về phía player
                MoveTowardsPlayer();

                // Tấn công thường nếu đã hết thời gian chờ
                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    NormalAttack();
                }
            }
        }
        else
        {
            // Di chuyển về phía player nếu không trong phạm vi
            MoveTowardsPlayer();
        }

        // Cập nhật animation
        UpdateAnimations();
    }

    void MoveTowardsPlayer()
    {
        // Tính hướng di chuyển
        Vector3 direction = (player.position - transform.position).normalized;

        // Di chuyển về phía player
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Quay mặt về phía player
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    void NormalAttack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        // Kích hoạt animation tấn công thường
        if (animator != null)
        {
            animator.SetTrigger("NormalAttack");
        }

        // Gây damage thường
        DealDamage(normalDamage);

        // Reset trạng thái tấn công
        StartCoroutine(ResetAttackState(0.5f));
    }

    IEnumerator UseSkill()
    {
        isCastingSkill = true;
        lastSkillTime = Time.time;

        // Dừng di chuyển
        StopMovement();

        // Kích hoạt animation skill
        if (animator != null)
        {
            animator.SetTrigger("Skill");
            animator.SetBool("IsCasting", true);
        }

        // Hiệu ứng cast skill (nếu có)
        if (skillEffect != null)
        {
            skillEffect.SetActive(true);
        }

        // Chờ thời gian cast skill
        yield return new WaitForSeconds(skillCastTime);

        // Thực hiện skill damage
        SkillDamage();

        // Tắt hiệu ứng skill (nếu có)
        if (skillEffect != null)
        {
            skillEffect.SetActive(false);
        }

        // Reset trạng thái
        if (animator != null)
        {
            animator.SetBool("IsCasting", false);
        }

        isCastingSkill = false;
    }

    void SkillDamage()
    {
        // Tìm tất cả player trong phạm vi skill
        Collider[] hitPlayers = Physics.OverlapSphere(transform.position, skillRange);

        foreach (Collider collider in hitPlayers)
        {
            if (collider.CompareTag("Player"))
            {
                Player playerHealth = collider.GetComponent<Player>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(skillDamage);
                    Debug.Log("Skill hit player for " + skillDamage + " damage!");
                }
            }
        }

        // Hiển thị phạm vi skill (debug)
        Debug.Log("Skill used! Damage: " + skillDamage + ", Range: " + skillRange);
    }

    void DealDamage(int damageAmount)
    {
        // Kiểm tra nếu player vẫn trong phạm vi khi tấn công
        if (player != null && Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            // Gửi damage đến player
            Player playerHealth = player.GetComponent<Player>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }

    IEnumerator ResetAttackState(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAttacking = false;
    }

    void StopMovement()
    {
        // Dừng di chuyển trong lúc cast skill
    }

    void UpdateAnimations()
    {
        if (animator != null && !isCastingSkill)
        {
            // Cập nhật trạng thái di chuyển
            animator.SetBool("IsMoving", isInAttackRange && !isAttacking);

            // Cập nhật trạng thái tấn công
            animator.SetBool("IsAttacking", isAttacking);
        }
    }

    // Hiển thị phạm vi trong Scene view (chỉ để debug)
    void OnDrawGizmosSelected()
    {
        // Phạm vi tấn công thường (màu đỏ)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Phạm vi skill (màu vàng)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, skillRange);
    }
}