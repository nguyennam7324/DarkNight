using TMPro;
using UnityEngine;

public class Pistol : MonoBehaviour, IGun
{
    private float rotationOffSet = 180f;
    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject bulletPrefabs;
    [SerializeField] private float DelayShot = 0.2f;
    [SerializeField] private float maxAmmo = 12f;
    public float currentAmmo;
    private float nextShot;
    [SerializeField] public TextMeshProUGUI ammoText;
    public bool isEquipped = false;

    [Header("Bắn lệch")]
    [SerializeField] private float baseSpread = 2f;
    [SerializeField] private float distanceSpreadMultiplier = 0.2f;

    [Header("Reload")]
    [SerializeField] private float reloadDuration = 1.2f;
    private bool isReloading = false;

    [Header("Recoil")]
    [SerializeField] private float recoilDistance = 0.1f;
    [SerializeField] private float recoilReturnSpeed = 5f;

    [Header("Âm thanh riêng cho súng")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip reloadClip;

    private ManaSystem manaSystem; // tham chiếu mana

    void Start()
    {
        currentAmmo = maxAmmo;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isEquipped) return;

        HandleRecoil();

        if (isReloading) return;

        RotationGun();
        Shot();
        Reload();
        UpdateAmmotext();
    }

    void RotationGun()  
    {
        Vector3 displacement = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(displacement.y, displacement.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + rotationOffSet);
        transform.localScale = (angle < -90 || angle > 90) ? Vector3.one : new Vector3(1, -1, 1);
    }

    void Shot()
    {
        // giờ phải check cả ammo + mana
        if (Input.GetMouseButtonDown(0) && CanShoot() && Time.time > nextShot)
        {
            nextShot = Time.time + DelayShot;

            float distance = Vector2.Distance(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            float totalSpread = baseSpread + distance * distanceSpreadMultiplier;
            float randomAngle = Random.Range(-totalSpread, totalSpread);

            Quaternion spreadRotation = firePos.rotation * Quaternion.Euler(0, 0, randomAngle);
            Instantiate(bulletPrefabs, firePos.position, spreadRotation);

            currentAmmo--;
            manaSystem?.UseMana(5f); // ví dụ mỗi phát tốn 5 mana
            UpdateAmmotext();

            if (audioSource && shootClip)
                audioSource.PlayOneShot(shootClip);

            transform.localPosition -= transform.right * recoilDistance;
        }
    }

    void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo && !isReloading)
        {
            isReloading = true;

            if (audioSource && reloadClip)
                audioSource.PlayOneShot(reloadClip);

            Invoke(nameof(FinishReload), reloadDuration);
        }
    }

    void FinishReload()
    {
        currentAmmo = maxAmmo;
        UpdateAmmotext();
        isReloading = false;
    }

    void HandleRecoil()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * recoilReturnSpeed);
    }

    void UpdateAmmotext()
    {
        if (ammoText != null)
            ammoText.text = currentAmmo > 0 ? currentAmmo.ToString() : "EMPTY";
    }

    public void AddAmmo(float amount)
    {
        currentAmmo += amount;
        currentAmmo = Mathf.Min(currentAmmo, maxAmmo);
        UpdateAmmotext();
    }

    // ================= IGun Implementation =================
    public void SetEquipped(bool equipped) => isEquipped = equipped;
    public void SetAmmoText(TextMeshProUGUI text) => ammoText = text;
    public void SetAudioManager(AudioManager audio) { }

    public bool CanShoot()
    {
        // kiểm tra đủ đạn và mana
        bool hasAmmo = currentAmmo > 0;
        bool hasMana = manaSystem == null || manaSystem.currentMana >= 5f; // ví dụ tốn 5 mana
        return hasAmmo && hasMana;
    }

    public void SetManaSystem(ManaSystem manaSys) => manaSystem = manaSys;
}
