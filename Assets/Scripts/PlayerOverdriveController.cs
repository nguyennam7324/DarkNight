using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerOverdriveController : MonoBehaviour
{
    [Header("Multipliers")]
    public float damageMultiplier = 10f;
    public float moveSpeedMultiplier = 2.4f;
    [SerializeField] private ParticleSystem overdriveAura;

    private Player player;
    private float origDamage;
    private float origMoveSpeed;

    private bool isOverdriveActive = false;

    void Awake()
    {
        player = GetComponent<Player>();
    }

    void Start()
    {
        origMoveSpeed = player.MoveSpeed;
        origDamage = player.baseDamage;
    }

    void OnEnable()
    {
        var odSystem = FindAnyObjectByType<OverdriveSystem>();
        if (odSystem != null)
        {
            odSystem.OnOverdriveStart += ActivateOverdrive;
            odSystem.OnOverdriveEnd += DeactivateOverdrive;
        }
    }

    void OnDisable()
    {
        var odSystem = FindAnyObjectByType<OverdriveSystem>();
        if (odSystem != null)
        {
            odSystem.OnOverdriveStart -= ActivateOverdrive;
            odSystem.OnOverdriveEnd -= DeactivateOverdrive;
        }
    }

    public void ActivateOverdrive()
    {
        if (isOverdriveActive) return;
        isOverdriveActive = true;

        player.MoveSpeed = origMoveSpeed * moveSpeedMultiplier;
        player.baseDamage = origDamage * damageMultiplier;

        if (overdriveAura != null)
            overdriveAura.Play();

        Debug.Log("⚡ Overdrive ON!");
    }

    public void DeactivateOverdrive()
    {
        if (!isOverdriveActive) return;
        isOverdriveActive = false;

        player.MoveSpeed = origMoveSpeed;
        player.baseDamage = origDamage;

        if (overdriveAura != null)
            overdriveAura.Stop();

        Debug.Log("💤 Overdrive OFF!");
    }
}
