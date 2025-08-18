using TMPro;
using UnityEngine;

public class Sniper : MonoBehaviour, IGun
{
    private float rotationOffSet = 180f;

    [Header("Thiết lập súng")]
    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float delayShot = 1f;   // delay 1 giây giữa 2 phát
    [SerializeField] private float maxAmmo = 5f;     // băng đạn 5 viên
    public float currentAmmo;
    private float nextShot;

    [SerializeField] public TextMeshProUGUI ammoText;
    public bool isEquipped = false;

    [Header("Recoil")]
    [SerializeField] private float recoilDistance = 0.25f; // mạnh hơn shotgun 1 chút
    [SerializeField] private float recoilReturnSpeed = 4f;
    private Vector3 originalLocalPos;

    [Header("Âm thanh riêng")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip reloadClip;

    [Header("Reload Settings")]
    [SerializeField] private float reloadTime = 2.8f;
    private bool isReloading = false;

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

        if (Time.time >= nextShot)
        {
            RotationGun();
            Shot();
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            StartCoroutine(ReloadCoroutine());
        }

        UpdateAmmoText();
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
        if (Input.GetMouseButtonDown(0) && currentAmmo > 0) // chỉ bắn 1 phát mỗi lần click
        {
            nextShot = Time.time + delayShot;

            // Tạo đạn sniper
            GameObject bullet = Instantiate(bulletPrefab, firePos.position, firePos.rotation);

            // nếu đạn có Rigidbody2D thì set tốc độ ra nòng nhanh
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = firePos.right * 30f; // tốc độ cao (có thể chỉnh theo ý)
            }

            currentAmmo--;
            UpdateAmmoText();

            if (audioSource && shootClip)
                audioSource.PlayOneShot(shootClip);

            // recoil mạnh hơn các súng khác
            transform.localPosition -= transform.right * recoilDistance;
        }
    }

    private System.Collections.IEnumerator ReloadCoroutine()
    {
        isReloading = true;

        if (audioSource && reloadClip)
            audioSource.PlayOneShot(reloadClip);

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        UpdateAmmoText();

        isReloading = false;
    }

    void HandleRecoil()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * recoilReturnSpeed);
    }

    void UpdateAmmoText()
    {
        if (ammoText != null)
            ammoText.text = isReloading
                ? "RELOADING..."
                : (currentAmmo > 0 ? currentAmmo.ToString() : "EMPTY");
    }

    public void AddAmmo(float amount)
    {
        currentAmmo += amount;
        currentAmmo = Mathf.Min(currentAmmo, maxAmmo);
        UpdateAmmoText();
    }

    public void SetEquipped(bool equipped) => isEquipped = equipped;
    public void SetAmmoText(TextMeshProUGUI text) => ammoText = text;
    public void SetAudioManager(AudioManager audio) { }
}
