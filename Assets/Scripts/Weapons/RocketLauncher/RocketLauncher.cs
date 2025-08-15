using UnityEngine;
using TMPro;
using System.Collections;

public class RocketLauncher : MonoBehaviour, IGun
{
    [Header("Cài đặt bắn")]
    [SerializeField] private float rotationOffset = 0f;
    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private float reloadTime = 3f;
    [SerializeField] private float recoilAmount = 0.15f;

    [Header("Explosion Settings")]
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private float explosionDamage = 50f;

    private int maxAmmo = 1;
    private int currentAmmo;
    private bool isReloading = false;
    private bool isEquipped = false;

    private TextMeshProUGUI ammoText;
    private AudioManager audioManager;
    private Vector3 originalLocalPos;

    private void Start()
    {
        currentAmmo = maxAmmo;
        // Sẽ được cập nhật lại khi Equip (giống Rifle)
        originalLocalPos = transform.localPosition;
        UpdateAmmoText();
    }

    private void Update()
    {
        if (!isEquipped || isReloading) return;
        RotateTowardsMouse();

        if (Input.GetMouseButtonDown(0) && currentAmmo > 0)
        {
            Shoot();
        }
    }

    private void RotateTowardsMouse()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + rotationOffset);

        // Lật sprite nếu trỏ sang trái (giữ nguyên như bản của bạn)
        Vector3 localScale = Vector3.one;
        if (difference.x < 0) localScale.y = -1;
        transform.localScale = localScale;
    }

    private void Shoot()
    {
        if (currentAmmo <= 0 || isReloading) return;

        currentAmmo--;
        UpdateAmmoText();

        // Tạo rocket và tách khỏi launcher
        GameObject rocket = Instantiate(rocketPrefab, firePos.position, firePos.rotation);
        rocket.transform.parent = null;

        Rocket rocketScript = rocket.GetComponent<Rocket>();
        if (rocketScript != null)
        {
            rocketScript.SetExplosion(explosionPrefab, explosionRadius, explosionDamage, GetComponentInParent<Player>());
        }

        ApplyRecoil();

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
        }
    }

    private void ApplyRecoil()
    {
        // Giật theo hướng nòng súng, sau đó trả về đúng vị trí gốc đã được set khi Equip
        transform.localPosition = originalLocalPos - transform.right * recoilAmount;
        Invoke(nameof(ResetRecoilPosition), 0.05f);
    }

    private void ResetRecoilPosition()
    {
        transform.localPosition = originalLocalPos;
    }

    private IEnumerator Reload()
    {
        isReloading = true;

        if (audioManager != null)
            audioManager.Play("Reload");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        UpdateAmmoText();
        isReloading = false;
    }

    // ⬇️ Hành vi Equip giống Rifle: chỉ set cờ + cập nhật vị trí gốc để recoil trả về đúng điểm tay cầm.
    public void SetEquipped(bool equipped)
    {
        isEquipped = equipped;

        if (equipped)
        {
            // Cập nhật vị trí gốc NGAY khi gắn lên tay (giống cách Rifle dự kiến dùng SetOriginalLocalPos)
            originalLocalPos = transform.localPosition;
        }
        // Không đụng tới Collider2D / Rigidbody2D / GunPickup ở đây.
        // GunHolder sẽ tự xử lý bật/tắt như với Rifle.
    }

    public void SetAmmoText(TextMeshProUGUI text)
    {
        ammoText = text;
        UpdateAmmoText();
    }

    public void SetAudioManager(AudioManager manager) => audioManager = manager;

    public void AddAmmo(float amount)
    {
        currentAmmo = Mathf.Clamp(currentAmmo + (int)amount, 0, maxAmmo);
        UpdateAmmoText();
    }

    // Hàm này giữ để tương thích nếu GunHolder muốn gọi thủ công (giống Rifle)
    public void SetOriginalLocalPos() => originalLocalPos = transform.localPosition;

    private void UpdateAmmoText()
    {
        if (ammoText != null)
            ammoText.text = $"{currentAmmo}/{maxAmmo}";
    }
}
