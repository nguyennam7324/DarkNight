using UnityEngine;
using TMPro;

public class MachineGun : MonoBehaviour, IGun
{
    [Header("Thiết lập súng")]
    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float delayShot = 0.07f;
    [SerializeField] private float maxAmmo = 128f;
    [SerializeField] private float reloadTime = 3.5f;

    [Header("Độ giật & Spread")]
    [SerializeField] private float recoilAmount = 0.15f;
    [SerializeField] private float spreadAngle = 6f;

    [Header("Mana")]
    [SerializeField] private float manaCostPerShot = 5f; // tốn mana mỗi viên
    private ManaSystem manaSystem;                       // được gắn qua SetManaSystem

    [Header("Âm thanh riêng cho súng")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip reloadClip;

    private float currentAmmo;
    private float nextShot;
    private bool isReloading = false;
    private bool isEquipped = false;

    private Vector3 originalLocalPos;
    private TextMeshProUGUI ammoText;

    void Start()
    {
        currentAmmo = maxAmmo;
        originalLocalPos = transform.localPosition;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isEquipped || isReloading) return;

        RotationGun();
        Shot();
        Reload();
        UpdateAmmoText();
    }

    // ===== IGun implementation =====
    public void SetEquipped(bool equipped)
    {
        isEquipped = equipped;
        if (isEquipped) originalLocalPos = transform.localPosition;
    }

    public void SetAmmoText(TextMeshProUGUI text) => ammoText = text;

    public void SetAudioManager(AudioManager manager) { /* dùng audio riêng */ }

    public void AddAmmo(float amount)
    {
        currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo);
        UpdateAmmoText();
    }

    // kiểm tra đủ ammo + (nếu có manaSystem) thì đủ mana
    public bool CanShoot()
    {
        bool hasAmmo = currentAmmo > 0 && !isReloading;
        bool hasMana = (manaSystem == null) || (manaSystem.currentMana >= manaCostPerShot);
        return hasAmmo && hasMana;
    }

    public void SetManaSystem(ManaSystem mana) => manaSystem = mana;
    // ===============================

    void RotationGun()
    {
        Vector3 displacement = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(displacement.y, displacement.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 180f);
        transform.localScale = (angle < -90 || angle > 90) ? new Vector3(1, 1, 1) : new Vector3(1, -1, 1);
    }

    void Shot()
    {
        if (Input.GetMouseButton(0) && Time.time > nextShot)
        {
            // gate: đủ điều kiện bắn?
            if (!CanShoot()) return;

            // trừ mana (nếu có manaSystem). UseMana cần amount → truyền đúng!
            if (manaSystem != null && !manaSystem.UseMana(manaCostPerShot))
                return;

            nextShot = Time.time + delayShot;

            // bắn có spread
            Vector3 spread = firePos.eulerAngles;
            spread.z += Random.Range(-spreadAngle, spreadAngle);
            Instantiate(bulletPrefab, firePos.position, Quaternion.Euler(spread));

            currentAmmo--;
            ApplyRecoil();

            if (audioSource && shootClip)
                audioSource.PlayOneShot(shootClip);
        }
    }

    public void SetOriginalLocalPos() => originalLocalPos = transform.localPosition;

    void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
            StartCoroutine(ReloadRoutine());
    }

    private System.Collections.IEnumerator ReloadRoutine()
    {
        isReloading = true;

        if (audioSource && reloadClip)
            audioSource.PlayOneShot(reloadClip);

        UpdateAmmoText();
        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
        nextShot = Time.time; // reset delay sau reload
        UpdateAmmoText();
    }

    private void ApplyRecoil()
    {
        Vector3 recoilDir = -firePos.right * recoilAmount;
        transform.localPosition = originalLocalPos + recoilDir;
        CancelInvoke(nameof(ResetRecoilPosition));
        Invoke(nameof(ResetRecoilPosition), 0.05f);
    }

    private void ResetRecoilPosition() => transform.localPosition = originalLocalPos;

    void UpdateAmmoText()
    {
        if (ammoText == null) return;
        ammoText.text = isReloading ? "RELOADING..." : (currentAmmo > 0 ? currentAmmo.ToString() : "EMPTY");
    }
}
