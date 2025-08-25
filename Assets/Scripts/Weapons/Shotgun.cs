using TMPro;
using UnityEngine;

public class Shotgun : MonoBehaviour, IGun
{
    private float rotationOffSet = 180f;

    [Header("Thiết lập súng")]
    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject bulletPrefabs;
    [SerializeField] private float delayShot = 1.0f;
    [SerializeField] private float maxAmmo = 8f;
    public float currentAmmo;
    private float nextShot;

    [SerializeField] private TextMeshProUGUI ammoText;
    public bool isEquipped = false;

    [Header("Spread")]
    [SerializeField] private int pelletsPerShot = 8;
    [SerializeField] private float totalSpreadAngle = 25f;

    [Header("Recoil")]
    [SerializeField] private float recoilDistance = 0.15f;
    [SerializeField] private float recoilReturnSpeed = 4f;

    [Header("Âm thanh riêng")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip reloadClip;

    [Header("Reload Settings")]
    [SerializeField] private float reloadTime = 2f;
    private bool isReloading = false;

    void Start()
    {
        currentAmmo = maxAmmo;
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
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
        if (Input.GetMouseButtonDown(0) && currentAmmo > 0)
        {
            nextShot = Time.time + delayShot;

            float startAngle = -totalSpreadAngle / 2f;
            float angleStep = totalSpreadAngle / (pelletsPerShot - 1);

            for (int i = 0; i < pelletsPerShot; i++)
            {
                float angleOffset = startAngle + angleStep * i;
                Quaternion pelletRotation = firePos.rotation * Quaternion.Euler(0, 0, angleOffset);
                Instantiate(bulletPrefabs, firePos.position, pelletRotation);
            }

            currentAmmo--;
            UpdateAmmoText();

            if (audioSource && shootClip) audioSource.PlayOneShot(shootClip);

            transform.localPosition -= transform.right * recoilDistance;
        }
    }

    private System.Collections.IEnumerator ReloadCoroutine()
    {
        isReloading = true;

        if (audioSource && reloadClip) audioSource.PlayOneShot(reloadClip);

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
            ammoText.text = isReloading ? "RELOADING..." : (currentAmmo > 0 ? currentAmmo.ToString() : "EMPTY");
    }

    public void AddAmmo(float amount)
    {
        currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo);
        UpdateAmmoText();
    }

    // IGun
    public void SetEquipped(bool equipped) => isEquipped = equipped;
    public void SetAmmoText(TextMeshProUGUI text) => ammoText = text;
    public void SetAudioManager(AudioManager audio) { }
}
